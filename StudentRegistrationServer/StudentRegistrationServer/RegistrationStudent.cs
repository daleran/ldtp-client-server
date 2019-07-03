using System;
using System.Collections.Generic;

namespace StudentRegistrationServer
{
    /// <summary>
    /// Data object to hold student information. Can be created by deserializing a LDTP message
    /// </summary>
    class RegistrationStudent
    {
        public int StudentID;
        public string StudentName;
        public string StudentSSN;
        public string StudentEmail;
        public string StudentPhone;

        //Pretend to add the record to the database
        public void AddRecordToDB()
        {
            Console.WriteLine("Record " + StudentName + " added to MS SQL database.");
        }

        //Returns a pretty string of the object
        public override string ToString()
        {
            return "StudentID: "+ StudentID + Environment.NewLine
                + "StudentName: " + StudentName + Environment.NewLine
                + "StudentSSN: " + StudentSSN + Environment.NewLine
                + "StudentEmail: " + StudentEmail + Environment.NewLine
                + "StudentPhone: " + StudentPhone;
        }

        // Parse a supplied LDPT message body string and map the values to a new Registration Student
        public static RegistrationStudent DeserializeFromRosterStudent(string LDTPMessage)
        {
            RegistrationStudent student = new RegistrationStudent();
            try
            {
                //Split the string into rows containing only the key value pair
                string[] keyValuePairs = LDTPMessage.Split('\n');
               
                //Dictionary for storing the key and value pairs of the 
                Dictionary<string, string> messageMap = new Dictionary<string, string>();

                //Loop
                for(int i=0; i<keyValuePairs.Length;i++)
                {
                    //Split the pair into two values
                    string[] pair = keyValuePairs[i].Split('\t');
                    messageMap.Add(pair[0], pair[1]);
                }

                //Set the relavent fields from the LDTP message if the correct type matches
                if(messageMap["type"]== "RosterStudent")
                {
                    student.StudentID = int.Parse(messageMap["stuID"]);
                    student.StudentName = messageMap["name"];
                    student.StudentSSN = messageMap["ssn"];
                    student.StudentEmail = messageMap["emailAddress"];
                    student.StudentPhone = messageMap["homePhone"];
                }
                else
                {
                    //Throw an error if the LDTP is not of the roster student type
                    throw new Exception("LDTP message is not of type Roster Student");
                }


            } catch (Exception e)
            {
                Console.WriteLine("Error deserializing student.");
                Console.WriteLine(e);
            }
            return student;
         }
    }
}
