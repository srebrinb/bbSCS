/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package test;

import java.util.Collection;
import java.util.List;
import java.util.Scanner;
import javax.smartcardio.ATR;
import javax.smartcardio.Card;
import javax.smartcardio.CardChannel;
import javax.smartcardio.CardException;
import javax.smartcardio.CardTerminal;
import javax.smartcardio.CommandAPDU;
import javax.smartcardio.ResponseAPDU;
import javax.smartcardio.TerminalFactory;
import javax.xml.bind.DatatypeConverter;
import org.apache.commons.codec.*;
import org.apache.commons.codec.binary.Hex;
public class ScanSmartCard {
final protected static char[] hexArray = "0123456789ABCDEF".toCharArray();
public static String bytesToHex(byte[] bytes) {
    char[] hexChars = new char[bytes.length * 2];
    for ( int j = 0; j < bytes.length; j++ ) {
        int v = bytes[j] & 0xFF;
        hexChars[j * 2] = hexArray[v >>> 4];
        hexChars[j * 2 + 1] = hexArray[v & 0x0F];
    }
    return new String(hexChars);
}
    public static void main(String[] args) throws CardException {
        TerminalFactory tf = TerminalFactory.getDefault();
        List< CardTerminal> terminals = tf.terminals().list();
        System.out.println("Available Readers:");
        System.out.println(terminals + "\n");
        for (int readerNum = 0; readerNum < terminals.size(); readerNum++) {
            CardTerminal terminal = (CardTerminal) terminals.get(readerNum);
            if (terminal.isCardPresent()) {
                Card card = terminal.connect("*");
                ATR atr = card.getATR();
                System.out.print("reader "+readerNum);
               CardChannel channel = card.getBasicChannel();

	        String stratr=Hex.encodeHexString(atr.getBytes());
                
                System.out.println("ATR:"+stratr);
                Collection<String> descs = AtrUtils.getDescription(stratr);
                for(String desc : descs){
                    System.out.println("\t"+desc);
                }
                System.out.println(card.getProtocol());
            }
        }
    }
}
