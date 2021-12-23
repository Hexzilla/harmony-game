using System;
using System.Threading.Tasks;
using Nethereum.Web3;

namespace Minter
{
    class Program
    {
        const string HMY_RPC_URL = "https://api.s0.b.hmny.io";

        const string publicKey = "0xa25f853b33c6b72e42e39d7a8b90a336d9602cc4";

        static void Main(string[] args)
        {
            GetAccountBalance().Wait();
            Console.ReadLine();
        }

        static async Task GetAccountBalance()
        {
            var web3 = new Web3(HMY_RPC_URL);
            var balance = await web3.Eth.GetBalance.SendRequestAsync(publicKey);
            Console.WriteLine($"Balance in Wei: {balance.Value}");

            var etherAmount = Web3.Convert.FromWei(balance.Value);
            Console.WriteLine($"Balance in Ether: {etherAmount}");
        }
    }
}
