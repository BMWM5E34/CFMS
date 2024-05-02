using NBitcoin;
using System;
using System.IO;
using Newtonsoft.Json;
using CFMS.Crypto;
using System.Net;
using NBitcoin.Protocol;

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
        public static Key GenerateKeyFromMnemonic(string mnemonicPhrase)
        {
            Mnemonic mnemonic = new Mnemonic(mnemonicPhrase);

            ExtKey masterKey = mnemonic.DeriveExtKey();

            Key privateKey = masterKey.PrivateKey;
            
            return privateKey;
        }
        public static void SendBitcoin(string senderPrivateKey, string receiverAddress, decimal amountToSend)
        {
            // Підключення до мережі Bitcoin
            using (var node = Node.ConnectToLocal(Network.Main))
            {
                node.VersionHandshake(); // Відправка та прийом версії

                // Створення транзакції
                Transaction transaction = Transaction.Create(Network.Main);

                // Вибір виходу для транзакції (вхідного виходу)
                var outPoint = new OutPoint(); // Потрібно вибрати відповідний вхід, можливо, зберігаючи його заздалегідь

                // Вибір виходу для підпису
                var scriptPubKey = BitcoinAddress.Create(receiverAddress, Network.Main).ScriptPubKey;
                var coin = new Coin(outPoint.Hash, outPoint.N, Money.Coins(amountToSend), scriptPubKey);

                // Додавання входу до транзакції
                transaction.Inputs.Add(new TxIn(coin.Outpoint));

                // Підпис транзакції
                BitcoinSecret senderKey = new BitcoinSecret(senderPrivateKey, Network.Main);
                transaction.Sign(senderKey, coin);

                // Відправка транзакції
                node.SendMessage(new InvPayload(transaction));
                node.SendMessage(new TxPayload(transaction));

                Console.WriteLine("Transaction sent: " + transaction.GetHash());
            }
        }
    }
}
