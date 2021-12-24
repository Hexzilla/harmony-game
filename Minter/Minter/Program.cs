using System;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.RPC.TransactionTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Minter
{
    class Program
    {
        const string HMY_RPC_URL = "https://api.s0.b.hmny.io";

        const string publicKey = "0xa25f853b33c6B72E42E39d7A8B90A336D9602cC4";

        const string privateKey = "429c8fc033c2eabc9298f59c2aa1cb464efe12d9b16c3c9d7c8ad83a4bfb2eb9";

        const string contractAddress = "0x98fc8D8699636202d467247a2A28298e18BBC2d9";
        //"0x0D18612444F9Ac877A70643f322a1d31a16d2aBa";//"0x568CF0715BDd0E8eAd6bC5257925EB5D9E054B50";

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine($"Address: {publicKey}");
                Console.WriteLine($"ContractAddress: {contractAddress}");

                //GetAccountBalance().Wait();

                //Transfer().Wait();
                GetTotalItems().Wait();
                AddItem().Wait();
                GetTotalItems().Wait();
                //TransferEther().Wait();

                Console.WriteLine($"Completed");
                Console.ReadLine();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static async Task GetAccountBalance()
        {
            var web3 = new Web3(HMY_RPC_URL);
            var balance = await web3.Eth.GetBalance.SendRequestAsync(publicKey);
            Console.WriteLine($"Balance in Wei: {balance.Value}");

            var etherAmount = Web3.Convert.FromWei(balance.Value);
            Console.WriteLine($"Balance in Ether: {etherAmount}");
        }


        static async Task TransferEther()
        {
            var account = new Account(privateKey, 2);
            var web3 = new Web3(account, HMY_RPC_URL);

            var toAddress = "0x386ddae99782e2a627b7449bbcf354567d122db5";
            Console.WriteLine($"Receiver: {toAddress}");

            var transaction = await web3.Eth.GetEtherTransferService()
                .TransferEtherAndWaitForReceiptAsync(toAddress, 5);
            Console.WriteLine($"AddItem: {transaction.Status}, {transaction.BlockHash}");
        }

        static async Task AddItem()
        {
            //1666700000
            var account = new Account(privateKey, 2);
            var web3 = new Web3(account, HMY_RPC_URL);
            var contractHandler = web3.Eth.GetContractHandler(contractAddress);

            var abi = @"[{""constant"":true,""inputs"":[],""name"":""totalItems"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":"""",""type"":""address""}],""name"":""contributions"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""account"",""type"":""address""}],""name"":""addMinter"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":false,""inputs"":[],""name"":""renounceMinter"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""account"",""type"":""address""}],""name"":""isMinter"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""hrc20"",""outputs"":[{""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""name"":""_owner"",""type"":""address""},{""name"":""_hrc20"",""type"":""address""},{""name"":""_hrc721"",""type"":""address""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""account"",""type"":""address""}],""name"":""MinterAdded"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""account"",""type"":""address""}],""name"":""MinterRemoved"",""type"":""event""},{""constant"":false,""inputs"":[{""name"":""to"",""type"":""address""},{""name"":""index"",""type"":""uint256""}],""name"":""mint"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""limit"",""type"":""uint64""},{""name"":""price"",""type"":""uint128""},{""name"":""url"",""type"":""string""}],""name"":""addItem"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""limit"",""type"":""uint64""},{""name"":""price"",""type"":""uint128""},{""name"":""url"",""type"":""string""}],""name"":""register"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""limit"",""type"":""uint64""},{""name"":""price"",""type"":""uint128""},{""name"":""url"",""type"":""string""},{""name"":""to"",""type"":""address""}],""name"":""addItemAndMint"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""index"",""type"":""uint256""}],""name"":""getMinted"",""outputs"":[{""name"":"""",""type"":""uint64""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""index"",""type"":""uint256""}],""name"":""getLimit"",""outputs"":[{""name"":"""",""type"":""uint64""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""index"",""type"":""uint256""}],""name"":""getPrice"",""outputs"":[{""name"":"""",""type"":""uint128""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""index"",""type"":""uint256""}],""name"":""getUrl"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""tokenId"",""type"":""uint256""}],""name"":""getTokenData"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""}]";
            var contract = web3.Eth.GetContract(abi, contractAddress);

            /*var registerFunction = contract.GetFunction("register");
            var r2 = await registerFunction.SendTransactionAndWaitForReceiptAsync(account.Address, null, 1, 1000, "testlink");
            //var r3 = await registerFunction.("10", "1000", "testurl11111111");*/

            var transfer = new AddItemFunction()
            {
                //From = publicKey,
                //FromAddress = account.Address,
                TransactionType = 2,
                GasPrice = 1000000000,//Web3.Convert.ToWei(1, Nethereum.Util.UnitConversion.EthUnit.Gwei),
                Gas = 5000000,
                Limit = 1,
                Price = 1000,
                Url = "https://docs.nethereum.com/en/latest/nethereum-smartcontrats-gettingstarted"
            };

            var transferHandler = web3.Eth.GetContractTransactionHandler<AddItemFunction>();
            //var estimate = await transferHandler.EstimateGasAsync(contractAddress, transfer);
            //Console.WriteLine($"Elstimate: {estimate}");
            //transfer.Gas = estimate;

            //var signedTransaction = await transferHandler.SignTransactionAsync(contractAddress, transfer);
            
            var ret = await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress, transfer);
            //var ret = await contractHandler.SendRequestAndWaitForReceiptAsync(transfer);
            Console.WriteLine($"AddItem: {ret.Status.Value}");
        }

        static async Task GetTotalItems()
        {
            var account = new Account(privateKey, 1666700000);
            var web3 = new Web3(account, HMY_RPC_URL);

            var totalItemsFunctionMessage = new TotalItemsFunction()
            {
                Gas = 4000000,
                GasPrice = Web3.Convert.ToWei(1)
            };

            var totalItemHandler = web3.Eth.GetContractQueryHandler<TotalItemsFunction>();
            var ret2 = await totalItemHandler.QueryAsync<TotalItemsFunctionDTO>(contractAddress, totalItemsFunctionMessage);
            //BigInteger totalItems = await contractHandler.QueryAsync<TotalItemsFunction, BigInteger>(totalItemsFunctionMessage);
            Console.WriteLine($"TotalItems: {ret2.TotalItems}");
        }

        static async Task Transfer()
        {
            //1666700000
            var account = new Account(privateKey, 1666700000);
            var web3 = new Web3(account, HMY_RPC_URL);

            var receiverAddress = "0xde0B295669a9FD93d5F28D9Ec85E40f4cb697BAe";
            var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();
            var transfer = new TransferFunction()
            {
                FromAddress = publicKey,
                Gas = 5000000,
                To = receiverAddress,
                TokenAmount = 100
            };
            var transactionReceipt2 = await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress, transfer);
            var transactionHash = transactionReceipt2.TransactionHash;
            Console.WriteLine($"TransactionHash: {transactionHash}");
        }
    }
}
