package bobs.paraf.signer;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.util.Enumeration;

import javax.security.auth.callback.Callback;
import javax.security.auth.callback.CallbackHandler;
import javax.security.auth.callback.PasswordCallback;
import javax.security.auth.callback.UnsupportedCallbackException;
import javax.security.auth.login.LoginException;

import java.security.KeyStore;
import java.security.KeyStore.PrivateKeyEntry;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;
import java.security.Security;
import java.security.UnrecoverableEntryException;
public class CertUtils {
	private static final String PKCS11_LIB = "C:\\work\\PKCS11\\idprimepkcs11.dll";
	private static final String NAME = "idprimepkcs11";
	private static final String SLOT = "0";
	private static final String PIN = "1234";
	private static final String ALIAS = "73cf0e3f-91af-1877-cd8a-3d48bd153fca";
	private static CallbackHandler myCallbackHandler = new CallbackHandler() {
	    @Override
	    public void handle(Callback[] callbacks) throws IOException,
	            UnsupportedCallbackException {
	        for (Callback callback : callbacks) {
	            if (callback instanceof PasswordCallback) {
	                PasswordCallback passwordCallback = (PasswordCallback) callback;
	                System.out.println(passwordCallback.getPrompt() + PIN);
	                passwordCallback.setPassword(PIN.toCharArray());
	            }
	        }
	    }
	};
	public static void main(String[] args) throws LoginException, KeyStoreException, NoSuchAlgorithmException, UnrecoverableEntryException {
		String gemalutoPKCS11="C:\\work\\PKCS11\\64\\idprimepkcs11.dll";
	//	Pkcs11SignatureToken pkcs11=new Pkcs11SignatureToken(gemalutoPKCS11,"3494".toCharArray());
	//	List<DSSPrivateKeyEntry> keys = pkcs11.getKeys();
		
	//	System.out.println(keys.size());
//		 
		String configString = "name = "
				  + NAME.replace(' ', '_')
				  + "\n"
				  + "library = "
				  + PKCS11_LIB
				  + "\n slot = "
				  + SLOT
				  + " "
				  + "\n attributes = compatibility \n"
				  + "attributes(*,*,*)=\n{\nCKA_TOKEN=true\nCKA_LOCAL=true\n}";
				ByteArrayInputStream configStream = new ByteArrayInputStream(
				    configString.getBytes());
				sun.security.pkcs11.SunPKCS11 pkcs11Provider0 = new sun.security.pkcs11.SunPKCS11(configStream);
				pkcs11Provider0.login(null, myCallbackHandler);
				Security.addProvider(pkcs11Provider0);
				KeyStore.CallbackHandlerProtection chp = new KeyStore.CallbackHandlerProtection(
				    myCallbackHandler);
				KeyStore.Builder ksbuilder0 = KeyStore.Builder.newInstance(
				    "PKCS11", pkcs11Provider0, chp);
				KeyStore keyStore = ksbuilder0.getKeyStore();
				Enumeration<String> aliases = keyStore.aliases();
				while (aliases.hasMoreElements()) {
					final String alias = aliases.nextElement();
					if (keyStore.isKeyEntry(alias)) {
					//	final PrivateKeyEntry entry = (PrivateKeyEntry) keyStore.getEntry(alias, null);
						System.out.println(alias);
					}
				}
                                PrivateKeyEntry entry = (PrivateKeyEntry) keyStore.getEntry(ALIAS, null);
                                entry.getPrivateKey();
				//X509Certificate cert0 = (X509Certificate) ks0.getCertificate(ALIAS);
				// System.out.println("Cert " + cert0.toString());
			//	Principal p = cert0.getSubjectDN();
			System.out.println("End");
	}

}
