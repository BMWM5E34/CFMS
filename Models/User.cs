using System;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using Crypto;
using NBitcoin;

namespace CFMS.Models
{
    public class User
    {
        [JsonProperty]
        private string Username { get; set; }
        [JsonProperty]
        private string Password { get; set; }
        [JsonProperty]
        private string MnemonicPhrase { get; set; }
        public User(string username, string password)
        {
            Username = username;
            Password = password;
            MnemonicPhrase = btc.GenerateMnemonicPhrase();
        }

        public static string GetUsername()
        {
            User user = UtilsFunc.LoadFromJson();
            return user.Username;
        }

        public static string GetUserPassword()
        {
            User user = UtilsFunc.LoadFromJson();
            return user.Password;
        }

        public static string GetUserMnemonic()
        {
            User user = UtilsFunc.LoadFromJson();
            return user.MnemonicPhrase;
        }
        public static void SaveUser(User user)
        {
            string json = JsonConvert.SerializeObject(user);
            File.WriteAllText(UtilsFunc.FilePath, json);
        }

        public static string GetAddress(string coinName)
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filePath = Path.Combine(directory, "CFMS_Addresses.json");

            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);
            Dictionary<string, string> addressesObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            if (addressesObject.ContainsKey(coinName))
            {
                return addressesObject[coinName];
            }

            return null;
        }
        public static decimal GetBalance(string bitcoinAddress)
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
    }
}