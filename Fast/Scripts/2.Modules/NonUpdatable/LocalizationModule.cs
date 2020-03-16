using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Modules
{
    public class LocalizationModule : Module
    {
        private Localization.LanguageData curr_language_data = null;
        private Dictionary<string, Localization.LanguageData> languages_data 
            = new Dictionary<string, Localization.LanguageData>();

        private List<Localization.LocalizationText> texts = new List<Localization.LocalizationText>();

        private string string_variable = "{$}";

        public string StringVariable
        {
            get { return string_variable; }
            set { string_variable = value; }
        }

        public void AddLanguageData(Localization.LanguageData data)
        {
            languages_data[data.Language] = data;
        }

        public bool SetCurrentLanguage(string set)
        {
            bool language_found = languages_data.TryGetValue(set, out curr_language_data);

            if (language_found)
            {
                UpdateTexts();
            }

            return language_found;
        }

        public string GetLocalizedText(string key, List<string> args)
        {
            string ret = "";

            if (curr_language_data != null)
            {
                string current_data = curr_language_data.GetData(key);

                ret = ReplaceArgs(current_data, args);
            }
            else
            {
                ret = "LANGUAGE_NOT_FOUND_" + key;
            }

            return ret;
        }

        public string GetLocalizedText(string key)
        {
            return GetLocalizedText(key, null);
        }

        private string ReplaceArgs(string data, List<string> args)
        {
            string ret = "";

            if (args != null && args.Count > 0)
            {
                int current_arg_found_index = 0;
                string current_arg_fount_str = "";

                int current_data_index = 0;

                for (int i = 0; i < args.Count; ++i)
                {
                    string curr_arg_to_replace = args[i];

                    while (current_data_index < data.Length)
                    {
                        char curr_char = data[current_data_index];

                        if (curr_char == string_variable[current_arg_found_index])
                        {
                            current_arg_fount_str += curr_char;
                            ++current_arg_found_index;

                            if (current_arg_found_index == string_variable.Length)
                            {
                                ret += curr_arg_to_replace;

                                current_arg_fount_str = "";
                                current_arg_found_index = 0;

                                ++current_data_index;

                                break;
                            }
                        }
                        else
                        {
                            ret += current_arg_fount_str + curr_char;

                            current_arg_fount_str = "";
                            current_arg_found_index = 0;
                        }

                        ++current_data_index;
                    }
                }

                if (current_data_index < data.Length)
                {
                    ret += data.Substring(current_data_index, data.Length - current_data_index);
                }
            }
            else
            {
                ret = data;
            }

            return ret;
        }

        public void AddLocalizationText(Localization.LocalizationText text)
        {
            texts.Add(text);
        }

        private void UpdateTexts()
        {
            for (int i = 0; i < texts.Count; ++i)
            {
                texts[i].UpdateText();
            }
        }
    }
}
