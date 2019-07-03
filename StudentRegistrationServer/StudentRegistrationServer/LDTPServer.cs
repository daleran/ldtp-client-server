using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace StudentRegistrationServer
{
    /// <summary>
    /// A server that listens to a port for data using the LDTP
    /// Upon recipe of a message, the ACK character is sent
    /// </summary>
    class LDTServer
    {
        const char EOT = (char)4; //End of Message
        const char ACK = (char)6; //Acknowledge character

        //TCP connection info
        public IPAddress Ip { get; }
        public int Port { get; }

        //The max number of pending connections in the queue before the server refuses connections
        public int PendingConnectionQueueSize { get; set; }

        //Delegate used as callback to recieve data from the server
        public event Action<string> RequestEvent;

        //Combination of an IP and Port the server will be listening from
        IPEndPoint endpoint = null;

        //The socket that will listen to requests
        Socket listener = null;

        //Set the serve configuration upon creation
        public LDTServer(string ip, int port)
        {
            Ip = IPAddress.Parse(ip);
            Port = port;
            PendingConnectionQueueSize = 10;
            endpoint = new IPEndPoint(Ip, Port);
        }
        
        //Set up the main server socket and begin the syncronous blocking event loop
        public void StartServer()
        {
            try
            {
                listener = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(endpoint);
                listener.Listen(PendingConnectionQueueSize);
                Console.WriteLine("LDTP Server listending on {0} port {1}.", Ip.ToString(), Port);
                RunEventLoop();
                
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            } finally
            {
                //Shut down the socket if there are ever any issues
                Close();
            }
        }

        //Begin the syncronous blocking event loop to handle connectionr requests
        private void RunEventLoop()
        {
            //Byte and string request buffer
            byte[] buffer = new byte[1024];
            string request = null;

            while (true)
            {
                //Helper socket to accept a single connection
                Socket handler = null;

                try
                {
                    Console.WriteLine("Waiting for a connection...");
                    //Accept the incoming request
                    handler = listener.Accept();

                    //String for the completed request to be passed to the callback
                    string completedRequest = "";

                    //Handle the request and send back the ACK character
                    if(HandleRequest(handler, buffer, request, out completedRequest))
                    {
                        SendAcknowledgement(handler);

                        //Invoke the any registered callbacks with the data
                        RequestEvent?.Invoke(completedRequest);
                    }
                    
                } catch(Exception e)
                {
                    Console.WriteLine(e);
                } finally
                {
                    //Shutdown the helper socket if there are any exceptions
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            } 
        }

        //Hand the new incoming connection
        private bool HandleRequest(Socket handler, byte[] buffer, string request, out string completed)
        {
            //Set data string back to null
            request = null;
            bool success = false;
      
            //Wait untill we recive the EOT character
            while (true)
            {
                //Recieve bytes to the buffer and encode them onto the request string
                int bytesRec = handler.Receive(buffer);
                request += Encoding.ASCII.GetString(buffer, 0, bytesRec);

                //If the data string contains the EOT character
                if (request.IndexOf(EOT) > -1)
                {
                    //Trim the EOT
                    completed = request.Trim((char)4);
                    Console.WriteLine("Data recieved.");

                    //Set success to true and break out of the loop
                    success = true;
                    break;
                }
            }
            return success;
        }

        //Send back the ACK ASCII character to confirm a successful recipt of data
        private void SendAcknowledgement(Socket handler)
        {
            byte[] response = Encoding.ASCII.GetBytes(new char[] { ACK, '\n' });
            handler.Send(response);
            Console.WriteLine("Response Sent.");
        }

        //Try to close the listener
        public void Close()
        {
            try
            {
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            } catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
