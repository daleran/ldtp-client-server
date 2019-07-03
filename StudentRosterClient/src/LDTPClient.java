import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.net.Socket;
import java.nio.charset.Charset;

/**
 * SWENG 568
 * RosterClient.java
 * Purpose: TCP Client that sends ASCII message packets utilizing the
 * Lion Data Transfer Protocol (LDTP)
 *
 * @author Sean Davis
 */
public class LDTPClient {
	final char ACK = (char)06;
	final char EOT = (char)04;
	
	String ip;
	int port;
	
	
	Socket tcpClient = null;
	BufferedWriter out = null;
	BufferedReader in = null;
	
	/**
	 * Instantiate a new instance of the client
	 * 
	 * @param ip The IP address of the server.
	 * @param port Port the server is listening on. Default port for LDTP is 7366
	 */
	public LDTPClient(String ip, int port ) {
		this.ip = ip;
		this.port = port;
		startClient();
	}
	
	//Create a new TCP socket connection on the specified port and IO streams
	private void startClient() {
		try {
			tcpClient = new Socket(ip, port);
			out = new BufferedWriter(new OutputStreamWriter(tcpClient.getOutputStream(), Charset.forName("ASCII")));
			in = new BufferedReader(new InputStreamReader(tcpClient.getInputStream()));	
		} catch (IOException e) {
			System.out.println("Error connectin client to server on "+ip+" on port "+port);
			e.printStackTrace();
			close();
		} 
		System.out.println("Connected the server on "+ip+" on port "+port);
	}
	
	/**
	 * Send an ASCII string message to the server
	 * 
	 * @param message The message to send
	 * @return True if the message was sent successfully and a response was acknowledged. 
	 * False if the message failed to send 
	 */
	public boolean send(String message) {
		sendPacket(message);
		boolean success = waitForServerResponse();
		logSuccess(success);
		return success;
	}
	
	//Send the ASCII message followed by the EOT terminator
	private void sendPacket(String message) {
		try {
			out.write(message+EOT);
			out.flush();
			System.out.println("Message sent.");
			
		} catch (IOException e) {
			System.out.println("Error sending message to server.");
			e.printStackTrace();
			
		} 
	}
	
	//Waits for the server to send the ACK code to acknowledge a successful transmission
	//Return failure if it takes longer than the timeout to respond
	private boolean waitForServerResponse() {
		try {
			String response = null;	
			while(true) {	
				//Check the incoming stream for any bytes
				response = in.readLine();

				//If an ACK ASCII code is received return a successful response
				if (response.indexOf(ACK) != -1) {
					System.out.println("Recieved ACK from server.");
					return true;
				}
			}		
		} catch (IOException e) {
			e.printStackTrace();
			close();
		}
		return false;
	}
	
	//Log if the transmission was successful
	private void logSuccess(boolean success) {
		if (success) {
			System.out.println("Successful transmission.");
		} else {
			System.out.println("Failed transmission.");
		}
	}
	
	/**
	 * Close the client server connection and IO streams.
	 */
	public void close() {
		try {
			out.close();
			in.close();
			tcpClient.close();
		} catch (IOException e) {
			System.out.println("Error shutting down client.");
			e.printStackTrace();
		}
	}

}
