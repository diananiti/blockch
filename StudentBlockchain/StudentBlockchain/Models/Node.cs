using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Node
{
    private Blockchain _blockchain;
    private TcpListener _listener;

    public Node(Blockchain blockchain, int port)
    {
        _blockchain = blockchain;
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
    }

    public void Start()
    {
        while (true)
        {
            var client = _listener.AcceptTcpClient();
            HandleClient(client);
        }
    }

    private void HandleClient(TcpClient client)
    {
        using (var stream = client.GetStream())
        {
            var buffer = new byte[1024];
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            var message = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);

            if (message.StartsWith("transaction:"))
            {
                var transaction = DeserializeTransaction(message.Substring(11));
                _blockchain.AddTransaction(transaction);
            }
            else if (message.StartsWith("block:"))
            {
                var block = DeserializeBlock(message.Substring(6));
                _blockchain.AddBlock(block);
            }
        }
    }

    private Transaction DeserializeTransaction(string message)
    {
        var parts = message.Split(',');
        return new Transaction(parts[0], parts[1], int.Parse(parts[2]));
    }

    private Block DeserializeBlock(string message)
    {
        var parts = message.Split(',');
        return new Block(int.Parse(parts[0]), int.Parse(parts[1]), Convert.FromBase64String(parts[2]), parts[3].Split(';'));
    }
}