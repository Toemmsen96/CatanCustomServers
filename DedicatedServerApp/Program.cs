using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class UDPServer
{
    static void Main(string[] args)
    {
        // Specify the port number to listen on
        int port = 8888;

        // Create a UDP socket
        UdpClient udpServer = new UdpClient(port);
        Console.WriteLine("UDP server started on port " + port);

        try
        {
            while (true)
            {
                // Receive UDP datagram from any client
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receivedBytes = udpServer.Receive(ref clientEndPoint);

                // Convert the received bytes to a string
                string receivedMessage = Encoding.ASCII.GetString(receivedBytes);

                // Display the received message and client IP:port
                Console.WriteLine("Received from client at " + clientEndPoint + ": " + receivedMessage);

                // Optionally, send a response back to the client
                string responseMessage = "Message received successfully!";
                byte[] responseBytes = Encoding.ASCII.GetBytes(responseMessage);
                udpServer.Send(responseBytes, responseBytes.Length, clientEndPoint);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
        finally
        {
            // Close the UDP socket when done
            udpServer.Close();
        }
    }
}
