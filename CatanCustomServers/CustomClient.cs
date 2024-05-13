using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class CustomClient : MonoBehaviour
{
    private const string serverIP = "127.0.0.1"; // IP address of the server (localhost for testing)
    private const int serverPort = 8888; // Port number of the server

    private UdpClient udpClient;
    private IPEndPoint serverEndPoint;

    public CustomClient()
    {
        udpClient = new UdpClient();
        serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
        CatanCustomServers.CatanCustomServers.logger.LogInfo("Client created");
        ConnectToServer();
    }

    public void ConnectToServer()
    {
        // Send a connection message to the server
        string connectMessage = "Hello server, I'm a client!";
        byte[] data = Encoding.ASCII.GetBytes(connectMessage);
        udpClient.Send(data, data.Length, serverEndPoint);

        CatanCustomServers.CatanCustomServers.logger.LogInfo("Sent connection message to server");
    }

    public void CloseConnection()
    {
        if (udpClient != null)
        {
            string disconnectMessage = "Goodbye server, I'm closing!";
            byte[] data = Encoding.ASCII.GetBytes(disconnectMessage);
            udpClient.Send(data, data.Length, serverEndPoint);
            udpClient.Close();
            CatanCustomServers.CatanCustomServers.logger.LogInfo("Client closed");
        }
        else
        {
            CatanCustomServers.CatanCustomServers.logger.LogWarning("Client is already closed!");
        }
    }

    private void OnDestroy()
    {
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }
}
