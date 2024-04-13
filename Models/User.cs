using System;
using System.IO;
using Newtonsoft.Json;
using NBitcoin;

namespace CFMS.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string MnemonicPhrase { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            MnemonicPhrase = GenerateMnemonicPhrase();
        }

        private string GenerateMnemonicPhrase()
        {
            Mnemonic mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
            return mnemonic.ToString();
        }

        public void SaveToJson()
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filePath = Path.Combine(directory, "user.json");

            var userObj = new
            {
                Username,
                Password,
                MnemonicPhrase
            };

            string json = JsonConvert.SerializeObject(userObj);

            File.WriteAllText(filePath, json);
        }

        public static User LoadFromJson()
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filePath = Path.Combine(directory, "user.json");

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "{}");
            }

            string json = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<User>(json);
        }

        public static string GetUsername()
        {
            User user = LoadFromJson();
            return user.Username;
        }

        public static string GetUserPassword()
        {
            User user = LoadFromJson();
            return user.Password;
        }

        public static string GetUserMnemonic()
        {
            User user = LoadFromJson();
            return user.MnemonicPhrase;
        }
    }
}