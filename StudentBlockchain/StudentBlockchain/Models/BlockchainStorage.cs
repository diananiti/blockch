using System;
using System.IO;
using System.Text;

public class BlockchainStorage
{
    private string _filePath;

    public BlockchainStorage(string filePath)
    {
        _filePath = filePath;
    }

    public void SaveBlockchain(Blockchain blockchain)
    {
        
            using (var writer = new StreamWriter(_filePath, false, Encoding.UTF8))
            {
                writer.WriteLine($"ChainCount: {blockchain.Chain.Count}");
                foreach (var block in blockchain.Chain)
                {
                    writer.WriteLine($"Index: {block.Index}");
                    writer.WriteLine($"Timestamp: {block.Timestamp}");
                    writer.WriteLine($"PreviousHash: {BitConverter.ToString(block.PreviousHash).Replace("-", string.Empty)}");
                    writer.WriteLine($"Transactions: {string.Join(",", block.Transactions)}");
                    writer.WriteLine("---"); // separator intre blocuri
                }
            }
        
    }

    public Blockchain LoadBlockchain()
    {
        var blockchain = new Blockchain();

        using (var reader = new StreamReader(_filePath, Encoding.UTF8))
        {
            var blockCount = int.Parse(reader.ReadLine());
            for (int i = 0; i < blockCount; i++)
            {
                var timestamp = int.Parse(reader.ReadLine().Split(':')[1]);
                var previousHash = Convert.FromHexString(reader.ReadLine().Split(':')[1]);
                var transactions = reader.ReadLine().Split(':')[1].Split(',').ToArray();

                blockchain.Chain.Add(new Block(timestamp, 0, previousHash, transactions));
            }
        }

        return blockchain;
    }
}