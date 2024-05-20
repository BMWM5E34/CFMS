using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMS.Models
{
    internal static class Utils
    {
        public static readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CFMS_User.json");

        public static User LoadFromJson()
        {
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "{}");
            }
                
            string json = File.ReadAllText(FilePath);

            return JsonConvert.DeserializeObject<User>(json) ?? new User(string.Empty, string.Empty);
        }
    }
}
