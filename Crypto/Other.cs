using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMS.Crypto
{
    internal class Other
    {
        public static void WriteAddressToJson(string coinName, string address)
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filePath = Path.Combine(directory, "CFMS_Addresses.json");

            Dictionary<string, string> addressesObject = new Dictionary<string, string>();
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                addressesObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }

            if (addressesObject.ContainsKey(coinName))
            {
                return;
            }

            addressesObject[coinName] = address;

            string updatedJson = JsonConvert.SerializeObject(addressesObject, Formatting.Indented);

            File.WriteAllText(filePath, updatedJson);
        }

    }
}
