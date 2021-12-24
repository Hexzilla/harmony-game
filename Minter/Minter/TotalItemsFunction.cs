using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Minter
{
    [Function("totalItems")]
    public class TotalItemsFunction : FunctionMessage
    {
        
    }

    [FunctionOutput]
    public class TotalItemsFunctionDTO : IFunctionOutputDTO
    {
        [Parameter("uint256", "totalItems", 1)]
        public BigInteger TotalItems { get; set; }
    }
}
