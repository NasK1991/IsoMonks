using System.Net;
using System.Net.Sockets;

public class ConnectionProperties {

    private UdpClient socketClient;
    private UdpClient socketServer;
    private IPEndPoint sender;

    public ConnectionProperties() {
        string IP = "127.0.0.1";
        int exitPort = 9876;
        int enterPort = 9877;

        socketClient = new UdpClient(IP, exitPort);
        socketServer = new UdpClient(new IPEndPoint(IPAddress.Any, enterPort));
        sender = new IPEndPoint(IPAddress.Any, exitPort);
    }

    public UdpClient getSocketClient() {
        return socketClient;
    }

    public UdpClient getSocketServer() {
        return socketServer;
    }

    public IPEndPoint getSender() {
        return sender;
    }

}
