using NBitcoin;
using System;
using System.IO;
using Newtonsoft.Json;
using CFMS.Crypto;
using System.Net;
namespace Crypto
{
    public class Bitcoin
    {
        public static void GenerateBitcoinAddressFromSeed(string mnemonicPhrase)
        {
            ExtKey extendedKey = new Mnemonic(mnemonicPhrase).DeriveExtKey();

            var publicKey = extendedKey.PrivateKey.PubKey;
            var address = publicKey.GetAddress(ScriptPubKeyType.Segwit, Network.Main);

            Other.WriteAddressToJson("bitcoin", address.ToString());
        }   
        public static decimal GetBitcoinBalance(string bitcoinAddress)
        {
            string url = $"https://blockchain.info/rawaddr/{bitcoinAddress}";
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                decimal balance = data.final_balance / 100000000.0m;
                return balance;
            }
        }
    }
}
