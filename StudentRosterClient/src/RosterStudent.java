import java.lang.reflect.Field;

/**
 * SWENG 568
 * RosterStudent.java
 * Purpose: Plain Old Data Object simulating an ORM returning an object from a database
 *
 * @author Sean Davis
 */
public class RosterStudent {

	//Public fields
	public int stuID;
	public String name;
	public String ssn;
	public String emailAddress;
	public String homePhone;
	public String homeAddr;
	public String localAddr;
	public String emergenctContact;
	public int programID;
	public String paymentID;
	public int academicStatus;
	
	/**
	 * Factory method for simulating retrieving information from a database
	 * 
	 * @param id Dummy value to make it seem like we are pulling from a database
	 * @return a new RosterStudent object
	 */
	public static RosterStudent createFromDB (int id) {
		
		RosterStudent student = new RosterStudent();
		
		//Set the member variables to the test data
		//to simulate an ORM retrieving an object from the DB
		student.stuID = 1111;
		student.name = "Bob Smith";
		student.ssn = "222-333-1111";
		student.emailAddress = "bsmith@yahoo.com";
		student.homePhone = "215-777-8888";
		student.homeAddr = "123 Tulip Road, Ambler, PA 19002";
		student.localAddr = "321 Maple Avenue, Lion Town, PA 16800";
		student.emergenctContact = "John Smith (215-222-6666)";
		student.programID = 206;
		student.paymentID = "1111-206";
		student.academicStatus = 1;
		
		return student;
	}
	
	/**
	 * Serialize this Java object into the LDTP message body format. 
	 * LDTP message bodies have their type declared in the first key value pair
	 * followed by key value pairs of all their public non static fields
	 * 
	 * Keys and values are separated by a tab escape.
	 * Key value pairs are delimited by a line feed.
	 * 
	 * @param obj Any Java object. The serializer only serializes public fields and the class name
	 * @return a line feed delimited string of the RosterStudent
	 */
	public String serializeToLDTP() {
		StringBuilder builder = new StringBuilder();
		
		//The first key value pair is the object type name
		builder.append("type");
		builder.append("\t");
		builder.append(this.getClass().getSimpleName());
		
		//Loop through public fields and append the field name and value
		for (Field field : this.getClass().getDeclaredFields()) {
			try {
				builder.append("\n");
				builder.append(field.getName());
				builder.append("\t");
				builder.append(field.get(this));
			} catch (IllegalArgumentException | IllegalAccessException e) {
				System.out.println("Error serializing "+this.getClass().getName()+" to the LDTP format");
				e.printStackTrace();
			}
		}	
		return builder.toString();	
	}
}
