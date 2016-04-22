/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package test;

/**
 *
 * @author sbalabanov
 */
import java.util.List;
import java.util.Scanner;
import javax.smartcardio.Card;
import javax.smartcardio.CardChannel;
import javax.smartcardio.CardException;
import javax.smartcardio.CardTerminal;
import javax.smartcardio.CommandAPDU;
import javax.smartcardio.ResponseAPDU;
import javax.smartcardio.TerminalFactory;
import javax.xml.bind.DatatypeConverter;

public class TestPCSC {

    public static void main(String[] args) throws CardException {

        TerminalFactory tf = TerminalFactory.getDefault();
        List< CardTerminal> terminals = tf.terminals().list();
        System.out.println("Available Readers:");
        System.out.println(terminals + "\n");
    
        Scanner scanner = new Scanner(System.in);
        System.out.print("Which reader do you want to send your commands to? (0 or 1 or ...): ");
        String input = scanner.nextLine();
        int readerNum = Integer.parseInt(input);
        CardTerminal cardTerminal = (CardTerminal) terminals.get(readerNum);
        Card connection = cardTerminal.connect("DIRECT");
        CardChannel cardChannel = connection.getBasicChannel();

        System.out.println("Write your commands in Hex form, without '0x' or Space charaters.");
        System.out.println("\n---------------------------------------------------");
        System.out.println("Pseudo-APDU Mode:");
        System.out.println("---------------------------------------------------");
        while (true) {
            System.out.println("Pseudo-APDU command: (Enter 0 to send APDU command)");
            String cmd = scanner.nextLine();
            if (cmd.equals("0")) {
                break;
            }
            System.out.println("Command  : " + cmd);
            byte[] cmdArray = hexStringToByteArray(cmd);
            byte[] resp = connection.transmitControlCommand(CONTROL_CODE(), cmdArray);
            String hex = DatatypeConverter.printHexBinary(resp);
            System.out.println("Response : " + hex + "\n");
        }

        System.out.println("\n---------------------------------------------------");
        System.out.println("APDU Mode:");
        System.out.println("---------------------------------------------------");

        while (true) {
            System.out.println("APDU command: (Enter 0 to exit)");
            String cmd = scanner.nextLine();
            if (cmd.equals("0")) {
                break;
            }
            System.out.println("Command  : " + cmd);
            byte[] cmdArray = hexStringToByteArray(cmd);
            ResponseAPDU resp = cardChannel.transmit(new CommandAPDU(cmdArray));
            byte[] respB = resp.getBytes();
            String hex = DatatypeConverter.printHexBinary(respB);
            System.out.println("Response : " + hex + "\n");
        }

        connection.disconnect(true);

    }

    public static int CONTROL_CODE() {
        String osName = System.getProperty("os.name").toLowerCase();
        if (osName.indexOf("windows") > -1) {
            /* Value used by both MS' CCID driver and SpringCard's CCID driver */
            return (0x31 << 16 | 3500 << 2);
        } else {
            /* Value used by PCSC-Lite */
            return 0x42000000 + 1;
        }

    }

    public static byte[] hexStringToByteArray(String s) {
        int len = s.length();
        byte[] data = new byte[len / 2];
        for (int i = 0; i < len; i += 2) {
            data[i / 2] = (byte) ((Character.digit(s.charAt(i), 16) << 4)
                    + Character.digit(s.charAt(i + 1), 16));
        }
        return data;
    }

}