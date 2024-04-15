using NBitcoin;
using System;
using System.IO;
using Newtonsoft.Json;
using CFMS.Crypto;
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

    }
}
