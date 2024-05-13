using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

class TCPServer
{
    static void Main(string[] args)
    {
        Console.Title = "TCP Server";
        // Specify the port number to listen on
        int port = 8888;

        // Create a TCP listener
        TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
        tcpListener.Start();
        Console.WriteLine("TCP server started on port " + port);
        while (true)
        {
            try
            {
                while (true)
                {
                    // Accept a client connection
                    TcpClient client = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Client connected: " + client.Client.RemoteEndPoint);

                    while (client.Connected)
                    {
                        // Get the network stream for reading/writing
                        NetworkStream networkStream = client.GetStream();

                        // Receive data from the client
                        byte[] buffer = new byte[1024]; // Buffer to hold received data
                        int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                        string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                        // Display the received message
                        Console.WriteLine("Received from client " + client.Client.RemoteEndPoint + ": " + receivedMessage);

                        // Process the received message (optional)
                        string responseMessage = "Message received successfully!"; // Create a response message

                        // Convert the response message to bytes
                        byte[] responseBytes = Encoding.ASCII.GetBytes(responseMessage);

                        // Send the response back to the client
                        networkStream.Write(responseBytes, 0, responseBytes.Length);
                        Console.WriteLine("Sent response to client");
                    }



                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message+"\n");

            }
            /*
            finally
            {
                // Stop listening for new clients
                tcpListener.Stop();
            }
            */
        }
    }
}
