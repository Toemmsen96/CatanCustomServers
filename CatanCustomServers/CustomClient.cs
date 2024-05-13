using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Logging;

public class CustomClient : MonoBehaviour
{
    private const string serverIP = "127.0.0.1"; // IP address of the server (localhost for testing)
    private const int serverPort = 8888; // Port number of the server
    ManualLogSource logger = CatanCustomServers.CatanCustomServers.logger;
    private TcpClient tcpClient;
    private NetworkStream stream;
    private Task receiveTask; // Task for asynchronous receiving
    private byte[] receiveBuffer = new byte[1024];


    public CustomClient()
    {
        // Start listening for incoming messages from the server
        // Initialize TCP client and connect to server
        ConnectToServer();
        Task listener = Task.Run(() => StartListening());
        if (listener.IsCompleted)
        {
            logger.LogInfo("Listener task completed");
        }
    }

    internal void ConnectToServer()
    {
        try
        {
            tcpClient = new TcpClient(serverIP, serverPort);
            stream = tcpClient.GetStream();
            logger.LogInfo("Connected to server: " + serverIP + ":" + serverPort);

            // Send a connection message to the server
            string connectMessage = "Hello server, I'm a client!";
            byte[] data = Encoding.ASCII.GetBytes(connectMessage);
            stream.Write(data, 0, data.Length);
            logger.LogInfo("Sent connection message to server");
        }
        catch (Exception e)
        {
            logger.LogError("Failed to connect to server: " + e.Message);
        }
    }

    private async void StartListening()
    {
        try
        {
            logger.LogInfo("Start listening for incoming messages from the server");

            while (tcpClient != null && tcpClient.Connected)
            {
                Array.Clear(receiveBuffer, 0, receiveBuffer.Length);

                // Wait asynchronously to receive data from the server
                int bytesRead = await stream.ReadAsync(receiveBuffer, 0, receiveBuffer.Length);

                if (bytesRead > 0)
                {
                    string receivedMessage = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);
                    CatanCustomServers.CatanCustomServers.logger.LogInfo("Received from server: " + receivedMessage);
                    ProcessServerMessage(receivedMessage);
                }
                else
                {
                    CatanCustomServers.CatanCustomServers.logger.LogWarning("No data received. Server might have closed the connection.");
                    break;
                }
            }

            logger.LogInfo("Server connection closed.");
        }
        catch (Exception e)
        {
            logger.LogError("Error receiving data from server: " + e.Message);
        }
    }


    private void ProcessServerMessage(string message)
    {
        // Implement message processing logic here
        logger.LogInfo("Processing server message: " + message);
        return;
    }

    public void SendMessageToServer(string message)
    {
        if (tcpClient == null || !tcpClient.Connected)
        {
            logger.LogWarning("Client is not connected to the server.");
            return;
        }

        try
        {
            // Send message to the server
            stream = tcpClient.GetStream();
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            CatanCustomServers.CatanCustomServers.logger.LogInfo("Sent message to server: " + message);
        }
        catch (Exception e)
        {
            logger.LogError("Error sending message to server: " + e.Message);
        }
    }

    private void OnDestroy()
    {
        CloseConnection();
    }

    public void CloseConnection()
    {
        if (tcpClient != null && tcpClient.Connected)
        {
            try
            {
                // Send a disconnect message to the server
                string disconnectMessage = "Goodbye server, I'm closing!";
                byte[] data = Encoding.ASCII.GetBytes(disconnectMessage);
                stream.Write(data, 0, data.Length);

                // Close the TCP client and stream
                tcpClient.Close();
                stream.Close();
                logger.LogInfo("Connection closed");
            }
            catch (Exception e)
            {
                logger.LogError("Error closing connection: " + e.Message);
            }
        }
        else
        {
            logger.LogWarning("Client is already closed or not connected.");
        }
    }
}
