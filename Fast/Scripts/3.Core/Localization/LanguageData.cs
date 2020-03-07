using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Localization
{
    public class LanguageData
    {
        private string language = "";

        private Dictionary<string, string> keyed_data = new Dictionary<string, string>();

        public LanguageData(string language)
        {
            this.language = language;
        }

        public string Language
        {
            get { return language; }
        }

        public void AddData(string key, string data)
        {
            keyed_data[key] = data;
        }

        public string GetData(string key)
        {
            string ret = "";

            bool found = false;
            foreach (KeyValuePair<string, string> entry in keyed_data)
            {
                bool equal = string.Equals(entry.Key, key, StringComparison.InvariantCultureIgnoreCase);

                if (equal)
                {
                    ret = entry.Value;

                    found = true;

                    break;
                }
            }

            if (!found)
            {
                ret = "!(" + key + ")(" + language + ")!";
            }

            return ret;
        }
    }
}
