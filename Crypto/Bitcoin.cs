using NBitcoin;
using System;
using System.IO;
using Newtonsoft.Json;
using CFMS.Crypto;
using System.Net;
using NBitcoin.Protocol;
using CFMS;
using System.Security.Policy;

namespace Crypto
{
    public class Bitcoin
    {
        private static Network networkType = Network.TestNet;

        public static void GenerateBitcoinAddressFromSeed(string mnemonicPhrase)
        {
            ExtKey extendedKey = new Mnemonic(mnemonicPhrase).DeriveExtKey();

            var publicKey = extendedKey.PrivateKey.PubKey;
            var address = publicKey.GetAddress(ScriptPubKeyType.Segwit, networkType);

            Other.WriteAddressToJson("bitcoin", address.ToString());
        }
        public static decimal GetBitcoinBalance(string bitcoinAddress)
        {
            string url = $"https://blockstream.info/testnet/api/address/{bitcoinAddress}/utxo";
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                decimal totalBalance = 0;
                foreach (var utxo in data)
                {
                    decimal utxoValue = (decimal)utxo["value"];
                    totalBalance += utxoValue;
                }

                totalBalance /= 100000000;

                return totalBalance;
            }
        }

        public static Key GenerateKeyFromMnemonic(string mnemonicPhrase)
        {
            Mnemonic mnemonic = new Mnemonic(mnemonicPhrase);

            ExtKey masterKey = mnemonic.DeriveExtKey();

            Key privateKey = masterKey.PrivateKey;
            
            return privateKey;
        }
        public static decimal GetRateBtc()
        {
            try
            {
                string url = "https://www.blockchain.com/ru/ticker";
                using (WebClient client = new WebClient())
                {
                    string json = client.DownloadString(url);
                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                    decimal rate = data["USD"]["15m"];
                    

                    return rate;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error BTC: " + ex.Message);
                return -1;
            }
        }
        public static void SendBitcoin(string senderPrivateKey, string receiverAddress, decimal amountToSend)
        {

        }
    }
}
