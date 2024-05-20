using NBitcoin;
using System;
using System.IO;
using Newtonsoft.Json;
using CFMS.Crypto;
using System.Net;   
using NBitcoin.Protocol;
using CFMS;
using System.Security.Policy;
using Newtonsoft.Json.Linq;


namespace Crypto
{
    public class btc
    {
        private static Network network = Network.TestNet;

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

            Other.WriteAddressToJson("bitcoin", address.ToString());
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
