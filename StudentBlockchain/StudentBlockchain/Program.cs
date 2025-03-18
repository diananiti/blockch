using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        var blockchain = new Blockchain();
        var consensusAlgorithm = new ConsensusAlgorithm(blockchain);
        var node = new Node(blockchain, 8080);
        var blockchainStorage = new BlockchainStorage("blockchain.dat");

        consensusAlgorithm.Start();
        node.Start();

        // Simulate a client connection
        using (var client = new TcpClient())
        {
            client.Connect("localhost", 8080);
            using (var stream = client.GetStream())
            {
                var message = "transaction:bob,mary,23";
                var buffer = System.Text.Encoding.ASCII.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        while (true)
        {
            Console.WriteLine("Enter transaction (e.g. 'name1,name2,10'): ");
            var input = Console.ReadLine();
            var parts = input.Split(',');
            var transaction = new Transaction(parts[0], parts[1], int.Parse(parts[2]));
            blockchain.AddTransaction(transaction);

            Thread.Sleep(1000);
        }
    }
}