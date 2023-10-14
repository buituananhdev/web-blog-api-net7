using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Data.Models;

namespace WebBlog.Utility.Languages
{
    public class GetText
    {
        public static dynamic GetCodeStatus(string inputCode)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.Combine(currentDirectory, "");

            string jsonContent = System.IO.File.ReadAllText("../WebBlog.Utility/Languages/languages.json");
            JObject jsonObject = JObject.Parse(jsonContent);

            if (jsonObject.TryGetValue(inputCode, out var codeObject))
            {
                string status = codeObject["status"].ToString();
                string message = codeObject["message"].ToString();

                return new { res_code = inputCode, Status = status, Message = message };
            }

            return null;
        }

        public static dynamic GetCodeStatus(string inputCode, string additionalMessage)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.Combine(currentDirectory, "");

            string jsonContent = System.IO.File.ReadAllText("../WebBlog.Utility/Languages/languages.json");
            JObject jsonObject = JObject.Parse(jsonContent);

            if (jsonObject.TryGetValue(inputCode, out var codeObject))
            {
                string status = codeObject["status"].ToString();
                string message = additionalMessage + codeObject["message"].ToString();
                return new { res_code = inputCode, Status = status, Message = message };
            }

            return null;
        }
    }
}
