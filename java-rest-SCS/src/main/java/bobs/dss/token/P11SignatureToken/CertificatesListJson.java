/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package bobs.dss.token.P11SignatureToken;

import com.fasterxml.jackson.databind.ObjectMapper;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.apache.commons.io.IOUtils;

/**
 *
 * @author sbalabanov
 */
public class CertificatesListJson implements CertificatesListGetter {

    private final String[] commands;
    public static boolean getCertFromCache=false;
    public CertificatesListJson() {
        String pathToApps = "C:\\work\\bbSCS\\PKCS11CertList\\bin\\Debug\\PKCS11CertList.exe";
        String[] pkcs11Modules = {"C:\\work\\PKCS11\\64"};
        List<String> listCommands = new ArrayList<String>();
        listCommands.add(pathToApps);
        listCommands.addAll(Arrays.asList(pkcs11Modules));
        final String[] commands_=new String[listCommands.size()];
        commands = listCommands.toArray(commands_);
    }

    public CertificatesListJson(String pathToApps, String[] pkcs11Modules) {
        List<String> listCommands = new ArrayList();
        listCommands.add(pathToApps);
        listCommands.addAll(Arrays.asList(pkcs11Modules));
        final String[] commands_=new String[listCommands.size()];
        commands = listCommands.toArray(commands_);
    }

    @Override
    public CertInfo[] getCertsInfo() {
        CertInfo[] certsInfo=new CertInfo[0];
        try {
            String certsStr ="[]";
            if(getCertFromCache){
                Logger.getLogger(CertificatesListJson.class.getName()).log(Level.INFO, "Get Certs from cache");
                certsStr = IOUtils.toString(new FileInputStream("certs.json"));
            }else
            {
                certsStr = getCertsFromOS();
            }
            
            ObjectMapper mapper = new ObjectMapper();    
            certsInfo = mapper.readValue(certsStr, CertInfo[].class);
        } catch (IOException ex) {
            Logger.getLogger(CertificatesListJson.class.getName()).log(Level.SEVERE, null, ex);
        }
        return certsInfo;
    }

    String getCertsFromOS() throws IOException {
        try {
            Runtime rt = Runtime.getRuntime();

            Process proc = rt.exec(commands);
            OutputStream certsFile = new FileOutputStream("certs.json");
            IOUtils.copy(proc.getInputStream(), certsFile);
            certsFile.close();
            OutputStream errorFile = new FileOutputStream("error.json");
            IOUtils.copy(proc.getErrorStream(), errorFile);
            errorFile.close();
        } catch (Exception e) {
            Logger.getLogger(CertificatesListJson.class.getName()).log(Level.WARNING, null,e);
        }

        return IOUtils.toString(new FileInputStream("certs.json"));
    }

}
