using System;
using System.Net;
using System.Threading.Tasks;
/// <summary>
/// Student registration server that listens for input and coverts the messages into RegistrationStudent objects and saves them to a fake database.
/// </summary>
namespace StudentRegistrationServer
{
    class RegistrationServer
    {
        static void Main(string[] args)
        {
            //Retrieve the host name and port from the command line
            string ip = (args.Length > 0) ? args[0] : "192.168.0.1";
            int port = (args.Length > 0) ? int.Parse(args[1]) : 7366;

            //Create the server
            LDTServer server = new LDTServer(ip, port);

            //Register the OnServerRequest callback to the server
            server.RequestEvent += OnServerRequest;

            //Start the Server
            server.StartServer();
        }

        //Parse the message and add it to the fake database
        public static void OnServerRequest(string message)
        {
            RegistrationStudent student = RegistrationStudent.DeserializeFromRosterStudent(message);
            Console.WriteLine(student.ToString());
            student.AddRecordToDB();
        }
    }
}
