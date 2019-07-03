/**
 * SWENG 568
 * RosterClient.java
 * Purpose: CLI program to simulate loading a student record from a database
 * and sending it to the Course Registration server.
 *
 * @author Sean Davis
 */
public class RosterClient {
	public static void main(String[] args) {
		//Retrieve the IP address, port, and student ID from the command line
		String ip = (args.length > 0 ) ? args[0] : "192.168.0.1";
		int port = (args.length > 1 ) ? Integer.parseInt(args[1]) : 7366;
		int studentID = (args.length > 2 ) ? Integer.parseInt(args[2]) : -1;
		
		//Simulate retrieving a student from the database
		RosterStudent student = RosterStudent.createFromDB(studentID);
		
		//Serialize the student object into the LDTP message body format
		String studentData = student.serializeToLDTP();

		//Create the client
		LDTPClient client = new LDTPClient(ip, port);
		
		//Send the student data
		client.send(studentData);
		
		//Close the connection if we are done sending data
		client.close();
	}
}
