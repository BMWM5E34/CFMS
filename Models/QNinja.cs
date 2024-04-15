using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NBitcoin;
using QBitNinja.Client;
using QBitNinja.Client.Models;

namespace CFMS.Models
{
    internal class QNinja
    {
        public static async Task<decimal> GetBitcoinBalanceAsync(string bitcoinAddress)
        {
            var client = new QBitNinjaClient(Network.Main);

            var address = BitcoinAddress.Create(bitcoinAddress, Network.Main);


            var balance = await client.GetBalance(address, true).ConfigureAwait(false);

            decimal totalBalance = 0;
            foreach (var operation in balance.Operations)
            {
                totalBalance += operation.Amount.ToDecimal(MoneyUnit.BTC);
            }

            return totalBalance;
        }
    }
}
