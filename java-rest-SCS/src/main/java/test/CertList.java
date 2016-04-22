/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package test;

import bobs.dss.token.P11SignatureToken.CertificatesListJson;
import bobs.dss.token.P11SignatureToken.SignatureToken;
import bobs.paraf.gui.CertificateDialog;
import eu.europa.esig.dss.token.DSSPrivateKeyEntry;
import java.security.cert.Certificate;
import java.security.cert.CertificateException;
import java.util.ArrayList;
import java.util.List;
import javax.swing.JDialog;
import javax.swing.JPanel;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;

/**
 *
 * @author sbalabanov
 */
public class CertList extends JDialog
        implements ListSelectionListener {
    CertList(){}
    @Override
    public void valueChanged(ListSelectionEvent e) {

    }
    private void Show() throws Exception{
        CertificatesListJson.getCertFromCache = true;
        SignatureToken instance = new SignatureToken();
        List<DSSPrivateKeyEntry> result = instance.getKeys();
        List<Certificate> certList=new ArrayList();
        for (DSSPrivateKeyEntry certEntry : result) {
           certList.add( certEntry.getCertificate().getCertificate());
        }
       Certificate[] certs=new Certificate[certList.size()];
       certs=certList.toArray(certs);
         CertificateDialog.showCertificates(this, certs, 0, 3);
    }
    public static void main(String[] args) throws Exception{

       CertList certList=new CertList();
       certList.Show();

    }
}
