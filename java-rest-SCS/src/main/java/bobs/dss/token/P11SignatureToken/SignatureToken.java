/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package bobs.dss.token.P11SignatureToken;

import eu.europa.esig.dss.DSSException;
import eu.europa.esig.dss.token.AbstractSignatureTokenConnection;
import eu.europa.esig.dss.token.DSSPrivateKeyEntry;
import java.security.cert.CertificateException;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.apache.commons.lang3.StringUtils;

/**
 *
 * @author sbalabanov
 */
public class SignatureToken extends AbstractSignatureTokenConnection {

    CertificatesListGetter certGetter;
    public boolean getCache=true;
    public SignatureToken(CertificatesListGetter certGetter) throws CertificateException {
        this.certGetter = certGetter;

    }

    public SignatureToken() throws CertificateException {
        this(new CertificatesListJson());
    }

    @Override
    public void close() {
        
    }
    
    /**
     *
     * @param serialNumber  - HEX Certificate Serial Number 
     * @param issuer - Name Issuer CN=B-Trust TEST Operational || NULL
     * @return
     * @throws CertificateException
     */
    public PKCS11PrivateKeyEntry getKeyBySerialIssuer(String serialNumber,String issuer) throws CertificateException,DSSException{
        serialNumber=serialNumber.replaceAll("^[0]+","");
        serialNumber=StringUtils.deleteWhitespace(serialNumber.toUpperCase());
        
        CertInfo[] certsInfo = certGetter.getCertsInfo();
        PKCS11PrivateKeyEntry key=null;
        for (CertInfo certInfo : certsInfo) {
            String certSerialNumber=certInfo.getSerialNumber().replaceAll("^[0]+","");
           if (serialNumber.equals(certSerialNumber) && (issuer==null || certInfo.getIssuer().contains(issuer))){
               key=new PKCS11PrivateKeyEntry(certInfo);
               break;
           }
        }
        if (key==null){
            throw new DSSException("Not found key by SerialNumber:"+serialNumber+" & Issuer"+issuer);
        }
        return key;
    }
    public PKCS11PrivateKeyEntry getKeyByThumbprint(String thumbprint) throws CertificateException{
        thumbprint=StringUtils.deleteWhitespace(thumbprint.toUpperCase());
        CertInfo[] certsInfo = certGetter.getCertsInfo();
        PKCS11PrivateKeyEntry key=null;
        for (CertInfo certInfo : certsInfo) {
           if (thumbprint.equals(certInfo.getThumbprint())){
               key=new PKCS11PrivateKeyEntry(certInfo);
               break;
           }
        }
        if (key==null){
            throw new DSSException("Not found key by thumbprint:"+thumbprint);
        }
        return key;
    }
    @Override
    public List<DSSPrivateKeyEntry> getKeys() throws DSSException {
        final List<DSSPrivateKeyEntry> list = new ArrayList<>();
        CertInfo[] certsInfo = certGetter.getCertsInfo();
        for (CertInfo certInfo : certsInfo) {
            try {
                list.add(new PKCS11PrivateKeyEntry(certInfo));
            } catch (CertificateException ex) {
                Logger.getLogger(SignatureToken.class.getName()).log(Level.SEVERE, null, ex);
            }
        }
        return list;
    }

}
