/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package bobs.dss.token.P11SignatureToken;

import eu.europa.esig.dss.DSSException;
import eu.europa.esig.dss.DigestAlgorithm;
import eu.europa.esig.dss.EncryptionAlgorithm;
import eu.europa.esig.dss.SignatureAlgorithm;
import eu.europa.esig.dss.token.DSSPrivateKeyEntry;
import eu.europa.esig.dss.token.KSPrivateKeyEntry;
import java.security.PrivateKey;
import java.security.Signature;
import java.util.List;
import org.junit.After;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import static org.junit.Assert.*;
import org.junit.Test;

/**
 *
 * @author sbalabanov
 */
public class SignatureTokenTest {

    public SignatureTokenTest() {
    }

    @BeforeClass
    public static void setUpClass() {
    }

    @AfterClass
    public static void tearDownClass() {
    }

    @Before
    public void setUp() {
    }

    @After
    public void tearDown() {
    }

    /**
     * Test of close method, of class SignatureToken.
     */
    @org.junit.Test
    public void testClose() throws Exception {
        System.out.println("close");
        SignatureToken instance = new SignatureToken();
        instance.close();

    }

    /**
     * Test of getKeys method, of class SignatureToken.
     */
    @org.junit.Test
    public void testGetKeys() throws Exception {
        System.out.println("getKeys");
        CertificatesListJson.getCertFromCache = true;
        SignatureToken instance = new SignatureToken();
        List<DSSPrivateKeyEntry> expResult = null;
        List<DSSPrivateKeyEntry> result = instance.getKeys();
        assertNotEquals(0, result.size());
    }

    private PKCS11PrivateKeyEntry getCert(String certTmb) throws Exception {
        CertificatesListJson.getCertFromCache = true;
        SignatureToken instance = new SignatureToken();
        List<DSSPrivateKeyEntry> result = instance.getKeys();
        PKCS11PrivateKeyEntry selCert = null;
        for (DSSPrivateKeyEntry certEntry : result) {
            PKCS11PrivateKeyEntry cert = (PKCS11PrivateKeyEntry) certEntry;
            System.out.println(cert.thumbprint);
            if (certTmb.equals(cert.thumbprint)) {
                selCert = cert;
            }
        }
        return selCert;
    }

    private PrivateKey GetKey() throws Exception {
        PKCS11PrivateKeyEntry selCert = getCert("FD2E0A7DF3BF60BD4B5AE390AE008B6BA301250A");
        selCert.setPin("1234");
        PrivateKey privkey = selCert.getPrivateKey();
        return privkey;
    }

    @org.junit.Test
    public void testGetKeyByTmb() throws Exception {
        CertificatesListJson.getCertFromCache = true;
        String tmb = "FD2E0A7DF3BF60BD4B5AE390AE008B6BA301250A";
        SignatureToken instance = new SignatureToken();
        PKCS11PrivateKeyEntry selCert=instance.getKeyByThumbprint(tmb);
        selCert.setPin("1234");
        PrivateKey privkey = selCert.getPrivateKey();
    }

    @org.junit.Test
    public void testGetKey() throws Exception {
        GetKey();

    }

    @org.junit.Test
    public void testSign() throws Exception {
        PrivateKey privkey = GetKey();
        final SignatureAlgorithm signatureAlgorithm = SignatureAlgorithm.getAlgorithm(EncryptionAlgorithm.RSA, DigestAlgorithm.SHA256);
        final String javaSignatureAlgorithm = signatureAlgorithm.getJCEId();
        final Signature signature = Signature.getInstance(javaSignatureAlgorithm);
        signature.initSign(privkey);
        String dataForSign = "VGhpcyBpcyB0aGUgZGF0YSB0aGF0IGlzIGdvaW5nIHRvIGJlIGRpZ2l0YWxseSBzaWduZWQgYnkgdGhlIFNpZ25hdHVyZSBDcmVhdGlvbiBTZXJ2aWNlIChTQ1MpLiBNYWtlIHN1cmUgdGhhdCB5b3UgaGF2ZSB0aGUgU0NTIHNlcnZpY2UgcnVubmluZyAoc2VlIGJvdHRvbSBvZiB0aGlzIHBhZ2UpLiAgQ3VycmVudCBtYXhpbXVtIHNpemUgZm9yIHRoZSBkYXRhIGlzIDJNQi4gIFRpbWVzdGFtcDogVGh1IEFwciAxNCAyMDE2IDE4OjIyOjI2IEdNVCswMzAwIChGTEUgRGF5bGlnaHQgVGltZSk=";
        signature.update(org.apache.commons.codec.binary.Base64.decodeBase64(dataForSign));
        final byte[] signatureValue = signature.sign();
        String expResult = "Fl5eGQ/NbNzprx+6C2WuVA5u9jrVru4DjD4zw/mH+C9RDcNlLDaNatdWBmRUDDANcE72vdOJMqVuBGmz+uY95T9hvenLEc9h3oOIggmLWE9BWlFgPHY4d5A2Ve5mpXvXOw7B4bPpfGPU8o5aKb4YmgFRz8rIKjq/4ooB0iVPlK4hlWDfbLk/6/NAD7o779WPSD82iABf3kDjr/pwPAV7s4Vgr8WGvjnTnxmqR1CsZXSJ/lMrmbfP/sfG0but3XHOKN3y6G7SNxMWNrk7BhDBLM4aCgFoWUlxbF6uFoTcG7ujm7XpL3knLPPFbHsM4luve2vlHxj16lBquOYxXtQYbw==";
        String result = org.apache.commons.codec.binary.Base64.encodeBase64String(signatureValue);
        assertEquals(expResult.substring(0, 20), result.substring(0, 20));
    }

    @org.junit.Test
    public void testMultiSigns() throws Exception {
        PKCS11PrivateKeyEntry selCert = getCert("FD2E0A7DF3BF60BD4B5AE390AE008B6BA301250A");
        selCert.setPin("1234");

        PrivateKey privkey = selCert.getPrivateKey();

        final SignatureAlgorithm signatureAlgorithm = SignatureAlgorithm.getAlgorithm(EncryptionAlgorithm.RSA, DigestAlgorithm.SHA256);
        final String javaSignatureAlgorithm = signatureAlgorithm.getJCEId();
        final Signature signature = Signature.getInstance(javaSignatureAlgorithm);
        final Signature vsignature = Signature.getInstance(javaSignatureAlgorithm);
        vsignature.initVerify(selCert.getCertificate().getCertificate());
        signature.initSign(privkey);
        long startTime = System.currentTimeMillis();
        int count = 10;
        for (int i = 0; i < count; i++) {
            String toSign = "Hello word " + i;
            signature.update(toSign.getBytes());
            final byte[] signatureValue = signature.sign();
            String result = org.apache.commons.codec.binary.Base64.encodeBase64String(signatureValue);
            vsignature.update(toSign.getBytes());
            vsignature.verify(signatureValue);
        }
        long endTime = System.currentTimeMillis();
        long totalTime = endTime - startTime;
        System.out.println(totalTime / 100);
    }
    @org.junit.Test
    public void testSignProtectPin() throws Exception {
       
                CertificatesListJson.getCertFromCache = true;
        String tmb = "FD2E0A7DF3BF60BD4B5AE390AE008B6BA301250A";
        SignatureToken instance = new SignatureToken();
        PKCS11PrivateKeyEntry selCert=instance.getKeyByThumbprint(tmb);
        selCert.setProtectedPin("KMGqfVKYA3s=");
        PrivateKey privkey = selCert.getPrivateKey();

        final SignatureAlgorithm signatureAlgorithm = SignatureAlgorithm.getAlgorithm(EncryptionAlgorithm.RSA, DigestAlgorithm.SHA256);
        final String javaSignatureAlgorithm = signatureAlgorithm.getJCEId();
        final Signature signature = Signature.getInstance(javaSignatureAlgorithm);
        final Signature vsignature = Signature.getInstance(javaSignatureAlgorithm);
        vsignature.initVerify(selCert.getCertificate().getCertificate());
        signature.initSign(privkey);
        long startTime = System.currentTimeMillis();
        int count = 2;
        for (int i = 0; i < count; i++) {
            String toSign = "Hello word " + i;
            signature.update(toSign.getBytes());
            final byte[] signatureValue = signature.sign();
            String result = org.apache.commons.codec.binary.Base64.encodeBase64String(signatureValue);
            vsignature.update(toSign.getBytes());
            vsignature.verify(signatureValue);
        }
        long endTime = System.currentTimeMillis();
        long totalTime = endTime - startTime;
        System.out.println(totalTime / 100);
    }

    /**
     * Test of getKeyBySerialIssuer method, of class SignatureToken.
     */
    @Test
    public void testGetKeyBySerialIssuer() throws Exception {
        System.out.println("getKeyBySerialIssuer");
        String serialNumber = "9BA6E1";
        String issuer = null;
        SignatureToken instance = new SignatureToken();
        PKCS11PrivateKeyEntry expResult = null;
        PKCS11PrivateKeyEntry result = instance.getKeyBySerialIssuer(serialNumber, issuer);
    //    assertEquals(expResult.getCertificate()., result);
        
    }
@Test
    public void testGetKeyBySerialIssuerNotMatch() throws Exception {
        System.out.println("getKeyBySerialIssuer");
        String serialNumber = "009BA6E1";
        String issuer = "CN=NotB-Trust TEST Operational";
        SignatureToken instance = new SignatureToken();
       try{
        PKCS11PrivateKeyEntry result = instance.getKeyBySerialIssuer(serialNumber, issuer);
        System.out.println(result.getCertificate().getCertificate().toString());
       }catch(DSSException ex){
           assertEquals(ex.getMessage().substring(0, 30),"Not found key by SerialNumber:");
           return;
       }
       fail("Not throw DSSException");
       
    //    assertEquals(expResult.getCertificate()., result);
        
    }
    /**
     * Test of getKeyByThumbprint method, of class SignatureToken.
     */
    @Test
    public void testGetKeyByThumbprint() throws Exception {
        System.out.println("getKeyByThumbprint");
        String thumbprint = "FD2E0A7DF3BF60BD4B5AE390AE008B6BA301250A";
        SignatureToken instance = new SignatureToken();
        PKCS11PrivateKeyEntry expResult = null;
        PKCS11PrivateKeyEntry result = instance.getKeyByThumbprint(thumbprint);
       // assertEquals(expResult, result);
        // TODO review the generated test code and remove the default call to fail.
        
    }
}
