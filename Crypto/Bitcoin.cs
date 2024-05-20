using NBitcoin;
using System;
using System.IO;
using Newtonsoft.Json;
using CFMS.Crypto;
using CFMS.Models;
using System.Net;   
using NBitcoin.Protocol;
using CFMS;
using System.Security.Policy;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Crypto
{
    public class btc
    {
        public static Network network = Network.TestNet;

        private static readonly string blockcypherApiKey = "2ecd9a86427048389c23c17b106ffec9";
        public static string GenerateMnemonicPhrase()
        {
            Mnemonic mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
            return mnemonic.ToString();
        }
        public static void GenerateBitcoinAddressFromSeed(string mnemonicPhrase)
        {
            ExtKey extendedKey = new Mnemonic(mnemonicPhrase).DeriveExtKey();

            var publicKey = extendedKey.PrivateKey.PubKey;
            var address = publicKey.GetAddress(ScriptPubKeyType.Legacy, network);

            UtilsFunc.WriteAddressToJson("bitcoin", address.ToString());
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

        public static async Task SendTransaction(Network network, string receiverAddress, decimal amountToSend)
        {
            string blockcypherApiKey = btc.blockcypherApiKey;
            string senderAddress = User.GetUserAddress("bitcoin");
            string mnemonic = User.GetUserMnemonic();


            var prevOutDate = await GetPrevOuts(senderAddress, blockcypherApiKey);
            var (txHash, outputIndex, inputValue) = prevOutDate.Value;

            Key key = GenerateKeyFromMnemonic(mnemonic);

            var secret = new BitcoinSecret(key, network);
            var tx = Transaction.Create(network);

            var prevOut = new OutPoint(new uint256(txHash), outputIndex);
            var input = new TxIn(prevOut);
            input.ScriptSig = secret.PubKey.ScriptPubKey;
            tx.Inputs.Add(input);

            var destination = BitcoinAddress.Create(receiverAddress, network);
            var amount = Money.Coins(amountToSend);
            var fee = Money.Satoshis(5000);

            Console.WriteLine($"Send: {amount} BTC");
            Console.WriteLine($"Fee: {fee.ToDecimal(MoneyUnit.BTC)} BTC");

            var output = new TxOut(amount, destination.ScriptPubKey);
            tx.Outputs.Add(output);

            var changeAmount = inputValue - amount - fee;
            if (changeAmount > Money.Zero)
            {
                var changeAddress = secret.GetAddress(ScriptPubKeyType.Legacy);
                var changeOutput = new TxOut(changeAmount, changeAddress.ScriptPubKey);
                tx.Outputs.Add(changeOutput);
            }

            tx.Sign(secret, new Coin(new OutPoint(new uint256(txHash), outputIndex), new TxOut(inputValue, BitcoinAddress.Create(senderAddress, network).ScriptPubKey)));

            var txHex = tx.ToHex();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                var content = new StringContent("{\"tx\":\"" + txHex + "\"}", Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync($"https://api.blockcypher.com/v1/btc/test3/txs/push?token={blockcypherApiKey}", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Transaction sent successfully.");
                }
                else
                {
                    Console.WriteLine("Error sending transaction:");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
            }
        }

        public static async Task<(string txHash, int outputIndex, Money value)?> GetPrevOuts(string senderAddress, string blockcypherApiKey)
        {
            using (var httpClient = new HttpClient())
            {
                string url = $"https://api.blockcypher.com/v1/btc/test3/addrs/{senderAddress}?token={blockcypherApiKey}";

                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(jsonResponse);

                    if (json["txrefs"] != null)
                    {
                        var tx = json["txrefs"].First;
                        if (tx != null)
                        {
                            var txHash = tx["tx_hash"].ToString();
                            var outputIndex = tx["tx_output_n"].ToObject<int>();
                            var value = Money.Satoshis(tx["value"].ToObject<long>());

                            return (txHash, outputIndex, value);
                        }
                    }
                    return null;
                }
                else
                {
                    Console.WriteLine("Error retrieving transaction history:");
                    Console.WriteLine(response.ReasonPhrase);
                    return null;
                }
            }
        }
    }
}
