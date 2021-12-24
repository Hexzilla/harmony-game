using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Minter
{
    [Function("register")]
    public class AddItemFunction : FunctionMessage
    {
        [Parameter("uint64", "limit", 1)]
        public BigInteger Limit { get; set; }

        [Parameter("uint128", "price", 2)]
        public BigInteger Price { get; set; }

        [Parameter("string", "url", 3)]
        public string Url { get; set; }
    }
}
