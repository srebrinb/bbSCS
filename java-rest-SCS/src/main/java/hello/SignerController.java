/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package hello;

import bobs.dss.token.P11SignatureToken.CertInfo;
import bobs.dss.token.P11SignatureToken.CertificatesListJson;
import bobs.dss.token.P11SignatureToken.PKCS11PrivateKeyEntry;
import bobs.dss.token.P11SignatureToken.SignatureToken;
import bobs.paraf.web.SignRequest;
import bobs.paraf.web.SignResponse;
import eu.europa.esig.dss.DigestAlgorithm;
import eu.europa.esig.dss.EncryptionAlgorithm;
import eu.europa.esig.dss.SignatureAlgorithm;
import java.security.PrivateKey;
import java.security.Signature;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.btrust.swing.CertListDialog;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.RequestBody;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

/**
 *
 * @author sbalabanov
 */
@RestController
@RequestMapping("/sign")
@CrossOrigin(origins = "*", maxAge = 3600)
public class SignerController {

    @RequestMapping("/sign")
    public SignResponse sign(@RequestParam(value = "content") String content, @RequestParam(value = "thumbprint") String thumbprint) {
        SignResponse res = new SignResponse();
        res.setReasonCode(200);
        res.setReasonText("test only");
        return res;
    }

    @RequestMapping(method = RequestMethod.POST)
    public SignResponse sign(@RequestBody SignRequest input) {
        SignResponse res = new SignResponse();
        try {

            String thumbprint = input.getSelector().getThumbprint();
            String protectPin = input.getProtectedPin();
            CertificatesListJson.getCertFromCache = true;
            SignatureToken signToken = new SignatureToken();
            PKCS11PrivateKeyEntry selCert = null;
            if (thumbprint != null) {
                selCert = signToken.getKeyByThumbprint(thumbprint);
            } else if (input.getSelector().getSerialNumber() != null) {
                selCert = signToken.getKeyBySerialIssuer(input.getSelector().getSerialNumber(), input.getSelector().getIssuers().get(0));
            }
            if (selCert == null) {

                CertListDialog cld = new CertListDialog(signToken.getCertsInfos());
                cld.setCertSelectedIndex(-1);
                cld.setModal(true);
                cld.setVisible(true);
                CertInfo selCertInfo = cld.getSelectedCertInfo();
                selCert = signToken.getKeyByThumbprint(selCertInfo.getThumbprint());

            }
            if (selCert == null) {
                res.setReasonCode(404);
                res.setReasonText("Not found cert");
                return res;
            }
            selCert.setProtectedPin(protectPin);

            PrivateKey privkey = selCert.getPrivateKey();
            DigestAlgorithm digestAlgorithm = DigestAlgorithm.forName(input.getHashAlgorithm().toString());
            final SignatureAlgorithm signatureAlgorithm = SignatureAlgorithm.getAlgorithm(EncryptionAlgorithm.RSA, digestAlgorithm);
            final String javaSignatureAlgorithm = signatureAlgorithm.getJCEId();
            final Signature signature = Signature.getInstance(javaSignatureAlgorithm);
            signature.initSign(privkey);
            SignRequest.ContentType contentType = input.getContentType();
            if (contentType != null && contentType != SignRequest.ContentType.DATA) {
                res.setReasonCode(400);
                res.setReasonText("Supported ContentType are \"data\".");
                return res;
            }

            String content = input.getContent();
            List<Object> contents = input.getContents();

            if (content != null && !content.isEmpty()) {
                byte[] toSign = org.apache.commons.codec.binary.Base64.decodeBase64(input.getContent());
                signature.update(toSign);
                final byte[] signatureValue = signature.sign();
                String result = org.apache.commons.codec.binary.Base64.encodeBase64String(signatureValue);
                res.setSignature(result);
            } else if (contents != null && !contents.isEmpty() && contents.size() > 0) {
                List<String> signatures = new ArrayList();
                for (Iterator<Object> it = contents.iterator(); it.hasNext();) {
                    String contentForSign = (String) it.next();
                    byte[] toSign = org.apache.commons.codec.binary.Base64.decodeBase64(contentForSign);
                    signature.update(toSign);
                    final byte[] signatureValue = signature.sign();
                    String result = org.apache.commons.codec.binary.Base64.encodeBase64String(signatureValue);
                    signatures.add(result);
                }
                res.setSignatures(signatures);
            } else {
                res.setReasonCode(400);
                res.setReasonText("parameter \"content\" or \"contents\" is required");
                return res;
            }

            res.setSignatureAlgorithm(javaSignatureAlgorithm);

            res.setChain(selCert.getCertificateChain());
            res.getSignatureType();
            res.setStatus("OK");
            res.setReasonCode(200);
            res.setReasonText("Signature generated");

        } catch (Exception ex) {
            Logger.getLogger(SignerController.class.getName()).log(Level.SEVERE, null, ex);
            res.setReasonCode(500);
            res.setReasonText(ex.getMessage());
        }
        return res;
    }

}
