using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Blockchain
{
    private List<Block> _chain = new List<Block>();
    private List<Transaction> _transactionPool = new List<Transaction>();
    private object _lock = new object();

    public Blockchain()
    {
        _chain.Add(CreateGenesisBlock());
    }


    public List<Transaction> TransactionPool
    {
        get { return _transactionPool; }
    }
    public Block CreateGenesisBlock()
    {
        return new Block(0, 0, new byte[0], new string[0]);
    }

    public void AddTransaction(Transaction transaction)
    {
        lock (_lock)
        {
            _transactionPool.Add(transaction);
        }
    }

    public void MineBlock()
    {
        lock (_lock)
        {
            var transactions = _transactionPool.Take(10).ToArray();
            _transactionPool.RemoveRange(0, 10);

            var previousBlock = Chain.LastOrDefault();
            var newBlock = new Block((int)DateTime.UtcNow.Ticks, previousBlock.Index + 1, new byte[0], transactions.Select(t => t.ToString()).ToArray());
            Chain.Add(newBlock);

            Console.WriteLine($"Mined block {newBlock.Timestamp} with {transactions.Length} transactions");

        }
    }

    public List<Block> GetChain()
    {
        return _chain;
    }

    public List<Block> Chain
    {
        get { return _chain; }
    }
    public void AddBlock(Block block)
    {
        lock (_lock)
        {
            block.Index = _chain.Count;
            block.PreviousHash = _chain[_chain.Count - 1].Hash;
            _chain.Add(block);
        }
    }


    public string[] GetTransactionsAsStringArray()
    {
        return _transactionPool.Select(transaction => transaction.ToString()).ToArray();
    }
}