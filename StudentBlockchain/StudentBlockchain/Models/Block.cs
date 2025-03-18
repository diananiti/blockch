using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

public class Block
{
    public int Index { get; set; }
    public int Timestamp { get; set; }
    public byte[] PreviousHash { get; set; }
    public string[] Transactions { get; set; }
    public byte[] Hash { get; set; }

    public Block(int index, int timestamp, byte[] previousHash, string[] transactions)
    {
        Index = index;
        Timestamp = timestamp;
        PreviousHash = previousHash;
        Transactions = transactions;
        Hash = CalculateHash();
    }

    private byte[] CalculateHash()
    {
        using (var sha256 = SHA256.Create())
        {
            var input = $"{Timestamp}{BitConverter.ToString(PreviousHash)}{string.Join("", Transactions)}";
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            return sha256.ComputeHash(inputBytes);
        }
    }
}
