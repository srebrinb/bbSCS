/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.btrust.swing;

import bobs.dss.token.P11SignatureToken.CertInfo;
import bobs.dss.token.P11SignatureToken.CertificatesListJson;
import bobs.dss.token.P11SignatureToken.SignatureToken;
import java.security.cert.CertificateException;
import org.junit.After;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 *
 * @author sbalabanov
 */
public class CertListDialogTest {

    public CertListDialogTest() {
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
     * Test of getCertSelectedIndex method, of class CertListDialog.
     */
    @Test
    public void testGetCertSelectedIndex() throws CertificateException {
        System.out.println("getCertSelectedIndex");

        CertificatesListJson.getCertFromCache = true;
        SignatureToken signToken = new SignatureToken();
        CertListDialog cld = new CertListDialog(signToken.getCertsInfos());
        cld.setCertSelectedIndex(-1);
        cld.setModal(true);
        cld.setVisible(true);
        CertInfo selCert = cld.getSelectedCertInfo();
        System.out.println( selCert.getThumbprint());
    }

}
