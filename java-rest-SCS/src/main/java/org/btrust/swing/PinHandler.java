/*
 * Created by JFormDesigner on Wed Mar 21 10:43:46 EET 2012
 */

package org.btrust.swing;

import bobs.dss.token.P11SignatureToken.CertInfo;
import java.awt.*;
import java.awt.event.*;
import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStreamReader;
import java.nio.charset.Charset;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.PropertyResourceBundle;

import javax.security.auth.callback.Callback;
import javax.security.auth.callback.CallbackHandler;
import javax.security.auth.callback.PasswordCallback;
import javax.security.auth.callback.UnsupportedCallbackException;
import javax.swing.*;
import javax.swing.border.*;

/**
 * @author Brainrain
 */
public class PinHandler extends JDialog implements CallbackHandler {
	
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private PropertyResourceBundle message;
	private boolean isNewCertificateRequested = false;
	
	public PinHandler(Frame owner, CertInfo cbIn) {
		super(owner, true);
		initComponents();
		
		this.serNumberValue.setText( cbIn.getSerialNumber() );
		this.authorValue.setText( cbIn.getSubjectCn().trim() );
		this.issuerValue.setText( cbIn.getIssuerCn().trim() );
		this.validToValue.setText( new SimpleDateFormat("dd/MM/yyyy HH:mm:ss").format(new Date(cbIn.getDateTimeNotAfter()) ) );
		
		this.afterAutoInit();
		this.pack();
//		this.setVisible(true);
	}

	public PinHandler(Dialog owner, CertInfo cbIn ) {
		super(owner, true);
		initComponents();
		
		this.serNumberValue.setText(cbIn.getSerialNumber() );
		this.authorValue.setText( cbIn.getSubjectCn() );
		this.issuerValue.setText( cbIn.getIssuerCn() );
		this.validToValue.setText( new SimpleDateFormat("dd/MM/yyyy HH:mm:ss").format(new Date(cbIn.getDateTimeNotAfter()) ) );
		
		this.afterAutoInit();
		this.pack();
//		this.setVisible(true);
		
	}
	
	private void afterAutoInit(){
		
    	BufferedReader fr = null;
    	try {
    		fr = new BufferedReader(new InputStreamReader( this.getClass().getResourceAsStream("TakingCert_" + new Locale("bg", "BG").getLanguage() +".properties"), Charset.forName("UTF-8")));
			this.message = new PropertyResourceBundle( fr );
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			try {
				if (fr != null) fr.close();
			} catch (IOException e) {
				e.printStackTrace();
			}
		}
    	
		this.setTitle( this.message.getString("cdd_title") );
		this.serNumberText.setText( this.message.getString("cdd_serNumLabel") );
		this.authorText.setText( this.message.getString("cdd_authorLabel") );
		this.issuerText.setText( this.message.getString("cdd_issuerLabel") );
		this.validToText.setText( this.message.getString("cdd_validToLabel") );
		( (TitledBorder)this.inner.getBorder() ).setTitle( this.message.getString("cdd_certDataPanel") );
		this.pinText.setText( this.message.getString("cdd_pinLabel") );
		this.newCertBtn.setText( this.message.getString("cdd_chooseNewCertButton") );
		this.okBtn.setText( this.message.getString("cdd_signButton") );
	}

	private void newCertBtnActionPerformed(ActionEvent e) {
        this.isNewCertificateRequested = true;
        this.dispose();
	}

	private void okBtnActionPerformed(ActionEvent e) {
		
        if( this.pinValue.getPassword().length==0){
            JOptionPane.showMessageDialog(this, this.message.getString("cdd_msg1"), this.message.getString("title1"), JOptionPane.WARNING_MESSAGE);
            return;
        }
        
        this.dispose();
	}

	private void passwordField1KeyPressed(KeyEvent e) {
        if(e.getKeyCode() == KeyEvent.VK_ENTER){
        	this.okBtnActionPerformed(null);
        }
	}
	
	@Override
	public void handle(Callback[] callbacks) throws IOException, UnsupportedCallbackException {
		
		for (Callback c : callbacks){
			if ( c instanceof  PasswordCallback ){
				PasswordCallback pass = (PasswordCallback)c;
				pass.clearPassword();
				
				this.setModalityType(Dialog.ModalityType.APPLICATION_MODAL);
				this.setVisible(true);
				
				if ( this.isNewCertificateRequested ) throw new UnsupportedCallbackException(pass, "CHANGE_CERTIFICATE");
				pass.setPassword( this.pinValue.getPassword() );
			}
		}
		
	}
	
	private void initComponents() {
		// JFormDesigner - Component initialization - DO NOT MODIFY  //GEN-BEGIN:initComponents
		outer = new JPanel();
		inner = new JPanel();
		serNumberText = new JLabel();
		serNumberValue = new JLabel();
		authorText = new JLabel();
		authorValue = new JLabel();
		issuerText = new JLabel();
		issuerValue = new JLabel();
		validToText = new JLabel();
		validToValue = new JLabel();
		pinText = new JLabel();
		pinValue = new JPasswordField();
		newCertBtn = new JButton();
		okBtn = new JButton();

		//======== this ========
		setDefaultCloseOperation(WindowConstants.DISPOSE_ON_CLOSE);
		Container contentPane = getContentPane();
		contentPane.setLayout(new BorderLayout());

		//======== outer ========
		{
			outer.setPreferredSize(new Dimension(530, 230));
			outer.setLayout(new GridBagLayout());
			((GridBagLayout)outer.getLayout()).columnWidths = new int[] {315, 190};
			((GridBagLayout)outer.getLayout()).rowHeights = new int[] {131, 30, 40};

			//======== inner ========
			{
				inner.setComponentOrientation(ComponentOrientation.LEFT_TO_RIGHT);
				inner.setBorder(new TitledBorder(null, "text", TitledBorder.LEADING, TitledBorder.TOP));
				inner.setLayout(new GridBagLayout());
				((GridBagLayout)inner.getLayout()).columnWidths = new int[] {0, 0, 0};
				((GridBagLayout)inner.getLayout()).rowHeights = new int[] {0, 0, 0, 0, 0};
				((GridBagLayout)inner.getLayout()).columnWeights = new double[] {1.0, 1.0, 1.0E-4};
				((GridBagLayout)inner.getLayout()).rowWeights = new double[] {1.0, 1.0, 1.0, 1.0, 1.0E-4};

				//---- serNumberText ----
				serNumberText.setText("text");
				inner.add(serNumberText, new GridBagConstraints(0, 0, 1, 1, 0.0, 0.0,
					GridBagConstraints.CENTER, GridBagConstraints.BOTH,
					new Insets(0, 0, 0, 0), 0, 0));

				//---- serNumberValue ----
				serNumberValue.setText("text");
				inner.add(serNumberValue, new GridBagConstraints(1, 0, 1, 1, 0.0, 0.0,
					GridBagConstraints.CENTER, GridBagConstraints.BOTH,
					new Insets(0, 0, 0, 0), 0, 0));

				//---- authorText ----
				authorText.setText("text");
				inner.add(authorText, new GridBagConstraints(0, 1, 1, 1, 0.0, 0.0,
					GridBagConstraints.CENTER, GridBagConstraints.BOTH,
					new Insets(0, 0, 0, 0), 0, 0));

				//---- authorValue ----
				authorValue.setText("text");
				inner.add(authorValue, new GridBagConstraints(1, 1, 1, 1, 0.0, 0.0,
					GridBagConstraints.CENTER, GridBagConstraints.BOTH,
					new Insets(0, 0, 0, 0), 0, 0));

				//---- issuerText ----
				issuerText.setText("text");
				inner.add(issuerText, new GridBagConstraints(0, 2, 1, 1, 0.0, 0.0,
					GridBagConstraints.CENTER, GridBagConstraints.BOTH,
					new Insets(0, 0, 0, 0), 0, 0));

				//---- issuerValue ----
				issuerValue.setText("text");
				inner.add(issuerValue, new GridBagConstraints(1, 2, 1, 1, 0.0, 0.0,
					GridBagConstraints.CENTER, GridBagConstraints.BOTH,
					new Insets(0, 0, 0, 0), 0, 0));

				//---- validToText ----
				validToText.setText("text");
				inner.add(validToText, new GridBagConstraints(0, 3, 1, 1, 0.0, 0.0,
					GridBagConstraints.CENTER, GridBagConstraints.BOTH,
					new Insets(0, 0, 0, 0), 0, 0));

				//---- validToValue ----
				validToValue.setText("text");
				inner.add(validToValue, new GridBagConstraints(1, 3, 1, 1, 0.0, 0.0,
					GridBagConstraints.CENTER, GridBagConstraints.BOTH,
					new Insets(0, 0, 0, 0), 0, 0));
			}
			outer.add(inner, new GridBagConstraints(0, 0, 2, 1, 0.0, 0.0,
				GridBagConstraints.CENTER, GridBagConstraints.BOTH,
				new Insets(0, 0, 0, 0), 0, 0));

			//---- pinText ----
			pinText.setText("text");
			pinText.setPreferredSize(new Dimension(20, 15));
			outer.add(pinText, new GridBagConstraints(0, 1, 1, 1, 0.0, 0.0,
				GridBagConstraints.CENTER, GridBagConstraints.BOTH,
				new Insets(0, 5, 0, 0), 0, 0));

			//---- pinValue ----
			pinValue.setPreferredSize(new Dimension(20, 25));
			pinValue.setMargin(new Insets(0, 0, 0, 0));
			pinValue.addKeyListener(new KeyAdapter() {
				@Override
				public void keyPressed(KeyEvent e) {
					passwordField1KeyPressed(e);
				}
			});
			outer.add(pinValue, new GridBagConstraints(1, 1, 1, 1, 0.0, 0.0,
				GridBagConstraints.CENTER, GridBagConstraints.BOTH,
				new Insets(0, 45, 0, 45), 0, 0));

			//---- newCertBtn ----
			newCertBtn.setText("text");
			newCertBtn.setMargin(new Insets(0, 0, 0, 0));
			newCertBtn.setPreferredSize(new Dimension(70, 25));
			newCertBtn.addActionListener(new ActionListener() {
				@Override
				public void actionPerformed(ActionEvent e) {
					newCertBtnActionPerformed(e);
				}
			});
			outer.add(newCertBtn, new GridBagConstraints(0, 2, 1, 1, 0.0, 0.0,
				GridBagConstraints.CENTER, GridBagConstraints.BOTH,
				new Insets(8, 25, 0, 25), 0, 0));

			//---- okBtn ----
			okBtn.setText("text");
			okBtn.setMargin(new Insets(0, 0, 0, 0));
			okBtn.setPreferredSize(new Dimension(70, 25));
			okBtn.addActionListener(new ActionListener() {
				@Override
				public void actionPerformed(ActionEvent e) {
					okBtnActionPerformed(e);
				}
			});
			outer.add(okBtn, new GridBagConstraints(1, 2, 1, 1, 0.0, 0.0,
				GridBagConstraints.CENTER, GridBagConstraints.BOTH,
				new Insets(8, 45, 0, 45), 0, 0));
		}
		contentPane.add(outer, BorderLayout.CENTER);
		pack();
		setLocationRelativeTo(getOwner());
		// JFormDesigner - End of component initialization  //GEN-END:initComponents
	}

	// JFormDesigner - Variables declaration - DO NOT MODIFY  //GEN-BEGIN:variables
	private JPanel outer;
	private JPanel inner;
	private JLabel serNumberText;
	private JLabel serNumberValue;
	private JLabel authorText;
	private JLabel authorValue;
	private JLabel issuerText;
	private JLabel issuerValue;
	private JLabel validToText;
	private JLabel validToValue;
	private JLabel pinText;
	private JPasswordField pinValue;
	private JButton newCertBtn;
	private JButton okBtn;
	// JFormDesigner - End of variables declaration  //GEN-END:variables

}
