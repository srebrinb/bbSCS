package org.btrust.swing;

import java.awt.*;
import java.awt.event.*;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.io.IOException;
import java.security.cert.CertificateException;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.*;
import javax.swing.border.*;


/*
 * Created by JFormDesigner on Mon Jun 04 15:26:49 EEST 2007
 */
/**
 * @author Dessy V
 */
public class CertDataDialog extends JDialog {

    public static int res = 0;
    private char[] pin;

    public int serialNumber = -1;
    public String issuerName = null;
    public long validToLong = -1;
    public String subjectCN = null;

    private JPasswordField pinText;
    private MouseEvent e;

    public CertDataDialog(String subjectCN_in, String issuerName_in, int serialNumber_in, long validTo_in) {
        this.subjectCN = subjectCN_in;
        this.issuerName = issuerName_in;
        this.serialNumber = serialNumber_in;
        this.validToLong = validTo_in;
        initComponents();
        this.setModal(true);
        this.setVisible(true);
    }

    private void chooseNewCertButtonMouseClicked(MouseEvent e) throws IOException, CertificateException {
        this.e = e;
        res = 1;
        this.dispose();
    }

    private void signButtonMouseClicked(MouseEvent e) {
        this.e = e;
        if (pinText.getPassword().length == 0) {
            JOptionPane.showMessageDialog(this, "Моля, въведете ПИН код!", "ПРЕДУПРЕЖДЕНИЕ", JOptionPane.WARNING_MESSAGE);
            return;
        }
        pin = pinText.getPassword();
        res = 2;
        this.dispose();
    }

    private void chooseSignButtonKeyPressed(KeyEvent e) {
        if (e.getKeyCode() == 10) {
            signButtonMouseClicked(null);
        }
    }

    public char[] getPIN() {
        return this.pin;
    }

    private void initComponents() {
        // JFormDesigner - Component initialization - DO NOT MODIFY  //GEN-BEGIN:initComponents
        // Generated using JFormDesigner Evaluation license - Dessy V
        JLabel serNumLabel = new JLabel();
        JLabel authorLabel = new JLabel();
        JLabel issuerLabel = new JLabel();
        JLabel validToLabel = new JLabel();
        JPanel certDataPanel = new JPanel();
        JLabel serNumText = new JLabel();
        JLabel authorText = new JLabel();
        JLabel issuerText = new JLabel();
        JLabel validToText = new JLabel();
        JLabel pinLabel = new JLabel();
        pinText = new JPasswordField();
        JButton chooseNewCertButton = new JButton();
        JButton signButton = new JButton();
        JLabel emptyLabel = new JLabel();

        //======== this ========
        setBackground(UIManager.getColor("window"));
        setDefaultCloseOperation(WindowConstants.DISPOSE_ON_CLOSE);
        setResizable(false);
        setTitle("Избран сертификат за подписване:");
        Container contentPane = getContentPane();
        contentPane.setLayout(null);

        //---- SerNumLabel ----
        serNumLabel.setText("Сериен номер:");
        contentPane.add(serNumLabel);
        serNumLabel.setBounds(25, 30, 90, serNumLabel.getPreferredSize().height);

        //---- AuthorLabel ----
        authorLabel.setText("Автор:");
        contentPane.add(authorLabel);
        authorLabel.setBounds(25, 45, 90, 14);

        //---- ValidFromLabel ----
        issuerLabel.setText("Издател:");
        contentPane.add(issuerLabel);
        issuerLabel.setBounds(25, 60, 90, 14);

        //---- ValidToLabel ----
        validToLabel.setText("Валиден до:");
        contentPane.add(validToLabel);
        validToLabel.setBounds(25, 75, 90, 14);

        //======== CertDataPanel ========
        {
            certDataPanel.setBorder(new TitledBorder("Данни за сертификата:"));

            certDataPanel.setLayout(null);

            //---- SerNumText ----
            serNumText.setText(Integer.toString(serialNumber));
            certDataPanel.add(serNumText);
            serNumText.setBounds(120, 20, 450, serNumText.getPreferredSize().height);

            //---- AuthorText ----
            authorText.setText(subjectCN.replaceAll("CN=", "").trim());
            certDataPanel.add(authorText);
            authorText.setBounds(120, 35, 450, 14);

            //---- IssuerText ----
            issuerText.setText(issuerName.replaceAll("CN=", "").trim());
            certDataPanel.add(issuerText);
            issuerText.setBounds(120, 50, 450, 14);

            //---- ValidToText ----
            validToText.setText(new SimpleDateFormat("dd/MM/yyyy HH:mm:ss").format(new Date(validToLong)));
            certDataPanel.add(validToText);
            validToText.setBounds(120, 65, 450, 14);
        }
        contentPane.add(certDataPanel);
        certDataPanel.setBounds(5, 10, 575, 115);

        //---- PINLabel ----
        pinLabel.setText("Потребителски ПИН код за достъп до смарт картата:");
        contentPane.add(pinLabel);
        pinLabel.setBounds(10, 130, 315, pinLabel.getPreferredSize().height);

        //---- PINText ----
        pinText.setColumns(16);
        contentPane.add(pinText);
        pinText.setBounds(330, 130, 105, pinText.getPreferredSize().height);

        pinText.addKeyListener(new KeyAdapter() {
            @Override
            public void keyPressed(KeyEvent e) {
                chooseSignButtonKeyPressed(e);
            }
        });

        //---- ChooseNewCertButton ----
        chooseNewCertButton.setText("Избери нов сертификат за подписване");
        chooseNewCertButton.addMouseListener(new MouseAdapter() {
            @Override
            public void mouseClicked(MouseEvent e) {
                
                try {
                    chooseNewCertButtonMouseClicked(e);
                } catch (IOException ex) {
                    Logger.getLogger(CertDataDialog.class.getName()).log(Level.SEVERE, null, ex);
                } catch (CertificateException ex) {
                    Logger.getLogger(CertDataDialog.class.getName()).log(Level.SEVERE, null, ex);
                }
                
            }
        });
        contentPane.add(chooseNewCertButton);
        chooseNewCertButton.setBounds(new Rectangle(new Point(60, 175), chooseNewCertButton.getPreferredSize()));

        //---- SignButton ----
        signButton.setText("Подпиши");
        signButton.addMouseListener(new MouseAdapter() {
            @Override
            public void mouseClicked(MouseEvent e) {
                signButtonMouseClicked(e);
            }
        });
        signButton.addKeyListener(new KeyAdapter() {
            @Override
            public void keyPressed(KeyEvent e) {
                chooseSignButtonKeyPressed(e);
            }
        });
        contentPane.add(signButton);
        signButton.setBounds(new Rectangle(new Point(405, 175), signButton.getPreferredSize()));

        //---- EmptyLabel ----
        emptyLabel.setText(" ");
        contentPane.add(emptyLabel);
        emptyLabel.setBounds(0, 200, 585, emptyLabel.getPreferredSize().height);

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
        pack();
        setLocationRelativeTo(getOwner());
        // JFormDesigner - End of component initialization  //GEN-END:initComponents

        pinText.requestFocusInWindow();

        this.setAlwaysOnTop(true);

    }

    public MouseEvent getE() {
        return e;
    }
}
