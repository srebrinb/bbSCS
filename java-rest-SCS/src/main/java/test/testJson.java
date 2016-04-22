package test;

import java.io.FileInputStream;
import java.io.IOException;
import java.util.List;

import org.apache.commons.io.IOUtils;
import org.springframework.boot.json.JacksonJsonParser;

import com.fasterxml.jackson.databind.ObjectMapper;
import bobs.dss.token.P11SignatureToken.CertInfo;
import java.io.ByteArrayInputStream;
import java.io.FileOutputStream;
import java.io.OutputStream;
import java.security.cert.CertificateException;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;
import java.util.Base64;

public class testJson {

    public static void main(String[] args) throws IOException, CertificateException {

        JacksonJsonParser jparser = new JacksonJsonParser();

        String certsStr = getCerts();

        ObjectMapper mapper = new ObjectMapper();
        CertInfo[] certsInfo = mapper.readValue(certsStr, CertInfo[].class);
        CertificateFactory cf = CertificateFactory.getInstance("X.509");
        for (CertInfo certInfo : certsInfo) {
            List<String> chain = certInfo.getChain();
            String strCert = chain.get(0);

            byte[] decoded = Base64.getDecoder().decode(strCert);
            X509Certificate cert = (X509Certificate) cf.generateCertificate(new ByteArrayInputStream(decoded));
            System.out.println(certInfo.getTokenInfo().getSlotID() + " \t " + cert.getSubjectDN());

        }
    }

    static String getCerts() throws IOException {
        try {
            Runtime rt = Runtime.getRuntime();
            String[] commands = {"C:\\work\\bbSCS\\PKCS11CertList\\bin\\Debug\\PKCS11CertList.exe", "C:\\work\\PKCS11\\64"};
            Process proc = rt.exec(commands);

            OutputStream certsFile = new FileOutputStream("certs.json");
            IOUtils.copy(proc.getInputStream(), certsFile);
            certsFile.close();
            OutputStream errorFile = new FileOutputStream("error.json");
            IOUtils.copy(proc.getErrorStream(), errorFile);
            errorFile.close();
        } catch (Exception e) {}
        
        return IOUtils.toString(new FileInputStream("certs.json"));
    }

}
