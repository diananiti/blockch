using System;
using System.Threading;

public class ConsensusAlgorithm
{
    private Blockchain _blockchain;
    private object _lock = new object();

    public ConsensusAlgorithm(Blockchain blockchain)
    {
        _blockchain = blockchain;
    }

    public void Start()
    {
        while (true)
        {
            lock (_lock)
            {
                if (_blockchain.TransactionPool.Count >= 10)
                {
                    _blockchain.MineBlock();
                }
            }

            Thread.Sleep(1000);
        }
    }
}