/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package bobs.dss.token.P11SignatureToken;

import eu.europa.esig.dss.DSSException;
import eu.europa.esig.dss.EncryptionAlgorithm;
import eu.europa.esig.dss.token.DSSPrivateKeyEntry;
import eu.europa.esig.dss.token.PasswordInputCallback;
import eu.europa.esig.dss.token.PrefilledPasswordCallback;
import eu.europa.esig.dss.x509.CertificateToken;
import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.PrintWriter;
import java.io.StringWriter;
import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;
import java.security.PrivateKey;
import java.security.Provider;
import java.security.ProviderException;
import java.security.Security;
import java.security.cert.CertificateException;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;
import java.util.ArrayList;
import java.util.Base64;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.crypto.Cipher;
import javax.crypto.NoSuchPaddingException;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;
import javax.security.auth.callback.Callback;
import javax.security.auth.callback.CallbackHandler;
import javax.security.auth.callback.PasswordCallback;
import javax.security.auth.callback.UnsupportedCallbackException;

/**
 *
 * @author sbalabanov
 */
public class PKCS11PrivateKeyEntry implements DSSPrivateKeyEntry {

    private final CertificateToken certificate;
    private final CertificateToken[] certificateChain;
    private final TokenInfo tokenInfo;
    private final String _pkcs11Path;
    public final String thumbprint;
    private Provider _pkcs11Provider;
    private KeyStore _keyStore;
    private PasswordInputCallback callback;
    private final CertInfo certInfo;
    private boolean incorrectPin = false;
    private String cryptPin;

    private class ProtectPasswordInputCallback implements PasswordInputCallback {

        private final String protectedData;
        private String key;
        private Cipher c;

        ProtectPasswordInputCallback(String protectedData) throws NoSuchAlgorithmException, NoSuchPaddingException {
            this.protectedData = protectedData;
            c = Cipher.getInstance("DES/CBC/PKCS5Padding");
        }

        public void setKey(String key) {
            this.key = key.substring(0, 8);
        }

        @Override
        public char[] getPassword() {
            String strString = "";
            byte[] iv = key.getBytes();
            byte[] inkey = key.getBytes();

            SecretKeySpec key = new SecretKeySpec(inkey, "DES");
            IvParameterSpec dps = new IvParameterSpec(iv);
            try {
                c.init(Cipher.DECRYPT_MODE, key, dps);
                byte[] inEncrypted = org.apache.commons.codec.binary.Base64.decodeBase64(protectedData);
                byte tmp[] = c.doFinal(inEncrypted);
                //System.out.println(org.apache.commons.codec.binary.Hex.encodeHexString(tmp));
                //  System.out.println(new String(tmp));
                strString = new String(tmp);
                byte output[] = null;
                boolean forceTrim = false;
                if (tmp.length > 8 && (strString.substring(0, 7).matches("^.*([^0-9A-z\\-\\_\\s\\.]).*$") || forceTrim)) {
                    output = new byte[tmp.length - 8];
                    for (int i = 8; i < tmp.length; i++) {
                        output[i - 8] = tmp[i];
                    }
                    strString = new String(output);
                }
                return strString.toCharArray();
            } catch (Exception ex) {
                Logger.getLogger(PKCS11PrivateKeyEntry.class.getName()).log(Level.SEVERE, null, ex);
            }
            return strString.toCharArray();
        }

    }

    PKCS11PrivateKeyEntry(CertInfo certInfo) throws CertificateException {
        this.certInfo = certInfo;
        CertificateFactory cf = CertificateFactory.getInstance("X.509");
        List<String> strChain = certInfo.getChain();
        List<CertificateToken> certsChain = new ArrayList();
        for (String strCert : strChain) {
            byte[] decoded = Base64.getDecoder().decode(strCert);
            X509Certificate cert = (X509Certificate) cf.generateCertificate(new ByteArrayInputStream(decoded));
            certsChain.add(new CertificateToken(cert));
        }
        final CertificateToken[] certificateChain_ = new CertificateToken[certsChain.size()];
        certificateChain = certsChain.toArray(certificateChain_);
        certificate = certificateChain[0];
        tokenInfo = certInfo.getTokenInfo();
        _pkcs11Path = certInfo.getPKCS11modulePath();
        thumbprint = certInfo.getThumbprint();

    }

    public void setPin(String pin) {
        incorrectPin=false;
        callback = new PrefilledPasswordCallback(pin.toCharArray());
    }

    public void setProtectedPin(String cryptPin) throws NoSuchAlgorithmException, NoSuchPaddingException {
        if(this.cryptPin!=null && !this.cryptPin.equals(cryptPin)){
            incorrectPin=false;
        }
        this.cryptPin=cryptPin;
        ProtectPasswordInputCallback cb = new ProtectPasswordInputCallback(cryptPin);
        cb.setKey(certInfo.getThumbprint().toUpperCase());
        callback = cb;
    }

    @Override
    public CertificateToken getCertificate() {
        return certificate;
    }

    @Override
    public CertificateToken[] getCertificateChain() {
        return certificateChain;
    }

    @Override
    public EncryptionAlgorithm getEncryptionAlgorithm() throws DSSException {
        return EncryptionAlgorithm.RSA;
    }

    private String escapePath(String pathToEscape) {
        if (pathToEscape != null) {
            return pathToEscape.replace("\\", "\\\\");
        } else {
            return "";
        }
    }

    protected String getPkcs11Path() {

        return _pkcs11Path.replace("\\64", "");
    }

    private Provider getProvider() {
        try {
            if (_pkcs11Provider == null) {
                // check if the provider already exists
                final Provider[] providers = Security.getProviders();
                if (providers != null) {
                    for (final Provider provider : providers) {
                        final String providerInfo = provider.getInfo();
                        if (providerInfo.contains(getPkcs11Path())) {
                            _pkcs11Provider = provider;
                            return provider;
                        }
                    }
                }
                // provider not already installed

                installProvider();
            }
            return _pkcs11Provider;
        } catch (ProviderException ex) {
            throw new DSSException("Not a PKCS#11 library", ex);
        }
    }

    @SuppressWarnings("restriction")
    private void installProvider() {
        int smartCardNameIndex = 0;
        String aPKCS11LibraryFileName = escapePath(getPkcs11Path());
        Integer slotIndex = tokenInfo.getSlotID();
        String pkcs11ConfigSettings = "name = SmartCard" + smartCardNameIndex + "\n" + "library = \"" + aPKCS11LibraryFileName
                + "\"\nslotListIndex = " + slotIndex
                + "\n attributes = compatibility \n"
                + "attributes(*,*,*)=\n{\nCKA_TOKEN=true\nCKA_LOCAL=true\n}";
        // Logger.getLogger(PKCS11PrivateKeyEntry.class.getName()).log(Level.INFO, pkcs11ConfigSettings);
        byte[] pkcs11ConfigBytes = pkcs11ConfigSettings.getBytes();
        ByteArrayInputStream confStream = new ByteArrayInputStream(pkcs11ConfigBytes);

        sun.security.pkcs11.SunPKCS11 pkcs11 = new sun.security.pkcs11.SunPKCS11(confStream);
        _pkcs11Provider = pkcs11;
        Security.addProvider(_pkcs11Provider);
    }

    @SuppressWarnings("restriction")
    private KeyStore getKeyStore() throws KeyStoreException {
        if (incorrectPin) {
            throw new DSSException("Bad password for PKCS11");
        }
        if (_keyStore == null) {
            _keyStore = KeyStore.getInstance("PKCS11", getProvider());
            try {
                _keyStore.load(new KeyStore.LoadStoreParameter() {

                    @Override
                    public KeyStore.ProtectionParameter getProtectionParameter() {
                        return new KeyStore.CallbackHandlerProtection(new CallbackHandler() {

                            @Override
                            public void handle(Callback[] callbacks) throws IOException, UnsupportedCallbackException {
                                for (Callback c : callbacks) {
                                    Logger.getLogger(PKCS11PrivateKeyEntry.class.getName()).log(Level.INFO, "callback:" + c.getClass().getName());
                                    if (c instanceof PasswordCallback) {
                                        ((PasswordCallback) c).setPassword(callback.getPassword());
                                        return;
                                    }
                                }
                                throw new RuntimeException("No password callback");
                            }
                        });
                    }
                });
            } catch (Exception e) {
                Logger.getLogger(PKCS11PrivateKeyEntry.class.getName()).log(Level.SEVERE, null, e);
                StringWriter allMessage = new StringWriter();
                PrintWriter printWriter = new PrintWriter(allMessage);
                e.printStackTrace(printWriter);
                printWriter.close();
                if (allMessage.toString().contains("CKR_PIN_INCORRECT")) {
                    incorrectPin = true;
                    throw new DSSException("Bad password for PKCS11", e);
                }

                throw new KeyStoreException("Can't initialize Sun PKCS#11 security provider. Reason: " + e.getMessage(), e);
            }
        }
        return _keyStore;
    }

    public PrivateKey getPrivateKey() {
        try {
            final KeyStore keyStore = getKeyStore();
            String ALIAS = tokenInfo.getCkaLabel();
            KeyStore.PrivateKeyEntry entry = (KeyStore.PrivateKeyEntry) keyStore.getEntry(ALIAS, null);
            PrivateKey key = entry.getPrivateKey();
            return key;
        } catch (Exception e) {
            throw new DSSException("Can't getPrivateKey security " + "provider. Reason: " + e.getMessage(), e);
        }
    }
}
