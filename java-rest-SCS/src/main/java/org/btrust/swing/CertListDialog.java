package org.btrust.swing;

import bobs.dss.token.P11SignatureToken.CertInfo;
import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.text.SimpleDateFormat;
import java.util.List;

import com.sun.java.swing.plaf.windows.WindowsLookAndFeel;

/**
 * @author Dessy V
 */
public class CertListDialog extends JDialog {

    /**
     *
     */
    private static final long serialVersionUID = 1L;
    private int certSelectedIndex;
    public static boolean exitOnCancel = true;
    private List<CertInfo> certBeanList;
    private CertInfo selectedCertInfo;
    public String language = "bg";
    private JList CertList;

    // JFormDesigner - End of variables declaration  //GEN-END:variables

    /**
     *
     * @param certInfoList
     */
    public CertListDialog(List<CertInfo> certInfoList) {
        try {
            UIManager.setLookAndFeel(new WindowsLookAndFeel());
        } catch (UnsupportedLookAndFeelException e) {
            //   e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
        }
        this.certBeanList = certInfoList;
        initComponents();
        try {
            UIManager.setLookAndFeel(new WindowsLookAndFeel());
        } catch (UnsupportedLookAndFeelException e) {
            // e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
        }
    }

    private void ChooseCertMouseClicked() {
        certSelectedIndex = CertList.getSelectedIndex();
        if (certSelectedIndex < 0) {
            // Modal dialog with OK button
            JOptionPane.showMessageDialog(this, "Моля, изберете сертификат!", "ПРЕДУПРЕЖДЕНИЕ", JOptionPane.WARNING_MESSAGE);
        } else {
            selectedCertInfo = certBeanList.get(certSelectedIndex);
            this.dispose();
        }
    }

    private void ViewCertMouseClicked() {
        certSelectedIndex = CertList.getSelectedIndex();
        if (certSelectedIndex < 0) {
            JOptionPane.showMessageDialog(this, "Моля, изберете сертификат!", "ПРЕДУПРЕЖДЕНИЕ", JOptionPane.WARNING_MESSAGE);
        } else {
            CertInfo cb = certBeanList.get(certSelectedIndex);
            //Auxilary.viewCert(cb.getCert());
        }
    }

    private void ChooseCertButtonKeyPressed(KeyEvent e) {
        if (e.getKeyCode() == 10) {
            ChooseCertMouseClicked();
        }
    }

    private void initComponents() {
        // JFormDesigner - Component initialization - DO NOT MODIFY  //GEN-BEGIN:initComponents
        // Generated using JFormDesigner Evaluation license - Dessy V
        JScrollPane scrollPane1 = new JScrollPane();
        CertList = new JList();
        JButton chooseCertButton = new JButton();
        JButton viewCertButton = new JButton();
        JLabel certsListLabel = new JLabel();
        JLabel emptyLabel = new JLabel();

        //======== this ========
        //setTitle("����, �������� ���������� �� ���������� ����������:");
        if (language.equalsIgnoreCase("bg")) {
            setTitle("Моля, изберете сертификат за електронно подписване:");
        } else {
            setTitle("Please, choose certificate for signing:");
        }
        setBackground(UIManager.getColor("window"));

        setDefaultCloseOperation(WindowConstants.DISPOSE_ON_CLOSE);
        setModal(true);
        Container contentPane = getContentPane();
        contentPane.setLayout(null);

        //======== scrollPane1 ========
        {

            //---- CertList ----
            CertList.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
            scrollPane1.setViewportView(CertList);
        }
        contentPane.add(scrollPane1);
        scrollPane1.setBounds(5, 25, 435, 235);

        //---- ChooseCertButton ----
        if (language.equalsIgnoreCase("bg")) {
            chooseCertButton.setText("Изберете сертификат");
        } else {
            chooseCertButton.setText("Choose certificate");
        }
        chooseCertButton.addMouseListener(new MouseAdapter() {
            @Override
            public void mouseClicked(MouseEvent e) {
                ChooseCertMouseClicked();
            }
        });

        contentPane.add(chooseCertButton);
        chooseCertButton.setBounds(240, 270, 180, chooseCertButton.getPreferredSize().height);

        //---- ViewCertButton ----
        if (language.equalsIgnoreCase("bg")) {
            viewCertButton.setText("Вижте сертификат");
        } else {
            viewCertButton.setText("View certificate");
        }
        viewCertButton.addActionListener(new ActionListener() {

            public void actionPerformed(ActionEvent e) {
                try {
                    ViewCertMouseClicked();
                } catch (Exception e1) {
                    
                }
            }
        });
        chooseCertButton.addKeyListener(new KeyAdapter() {
            @Override
            public void keyPressed(KeyEvent e) {
                ChooseCertButtonKeyPressed(e);
            }
        });

        contentPane.add(viewCertButton);
        viewCertButton.setBounds(25, 270, 180, viewCertButton.getPreferredSize().height);

        /*
        		//---- ViewCertButton ----
		viewCertButton.setText("����� ����������");
		viewCertButton.addMouseListener(new MouseAdapter() {
			@Override
			public void mouseClicked(MouseEvent e) {
                try {
                    ViewCertMouseClicked();
                } catch (PKCS7SignerException e1) {
                    Auxilary.wDebug(e.toString(), -1);
                    //do nothing
                }
            }
		});
        chooseCertButton.addKeyListener(new KeyAdapter() {
               @Override
               public void keyPressed(KeyEvent e) {
                    ChooseCertButtonKeyPressed(e);
               }
          });

        contentPane.add(viewCertButton);
		viewCertButton.setBounds(25, 270, 180, viewCertButton.getPreferredSize().height);
         */
        //---- CertsListLabel ----
        if (language.equalsIgnoreCase("bg")) {
            certsListLabel.setText("[сериен номер - валиден до] Автор на сертификата [Oрганизация]");
        } else {
            certsListLabel.setText("[serial number - valid to] Certificate owner [Organization]");
        }
        certsListLabel.setHorizontalAlignment(SwingConstants.CENTER);
        certsListLabel.setLabelFor(CertList);
        contentPane.add(certsListLabel);
        certsListLabel.setBounds(5, 5, 435, certsListLabel.getPreferredSize().height);

        //---- EmptyLabel ----
        emptyLabel.setText("                                ");
        contentPane.add(emptyLabel);
        emptyLabel.setBounds(0, 295, 445, emptyLabel.getPreferredSize().height);

        { // compute preferred size
            Dimension preferredSize = new Dimension();
            for (int i = 0; i < contentPane.getComponentCount(); i++) {
                Rectangle bounds = contentPane.getComponent(i).getBounds();
                preferredSize.width = Math.max(bounds.x + bounds.width, preferredSize.width);
                preferredSize.height = Math.max(bounds.y + bounds.height, preferredSize.height);
            }
            Insets insets = contentPane.getInsets();
            preferredSize.width += insets.right;
            preferredSize.height += insets.bottom;
            contentPane.setMinimumSize(preferredSize);
            contentPane.setPreferredSize(preferredSize);
        }
        addWindowListener(new WindowAdapter() {
            public void windowClosing(WindowEvent we) {

                //dispose();
            }
        });
        pack();
        setLocationRelativeTo(getOwner());
        // JFormDesigner - End of component initialization  //GEN-END:initComponents
        CertList.setListData(getCertsFromStore());
        CertList.requestFocusInWindow();
        this.setAlwaysOnTop(true);
    }

    private String[] getCertsFromStore() {
        String[] certList = new String[certBeanList.size()];

        for (int i = 0; i < certBeanList.size(); i++) {
            CertInfo cb = certBeanList.get(i);
            String subjectCn=cb.getSubject();
            try{
                subjectCn=cb.getSubjectCn().trim() ;
            }catch(Exception e){
                
            }
            certList[i] = "[ " + cb.getSerialNumber() + " - " + cb.getDateTimeNotAfter() + " ] " +subjectCn ;
        }

        return certList;
    }

    public int getCertSelectedIndex() {
        return certSelectedIndex;
    }

    public void setCertSelectedIndex(int certSelectedIndex) {
        this.certSelectedIndex = certSelectedIndex;
    }

    /**
     *
     * @return
     */
    public CertInfo getSelectedCertInfo() {
        return selectedCertInfo;
    }

}
