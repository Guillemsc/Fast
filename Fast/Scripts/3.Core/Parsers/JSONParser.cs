using System;

namespace Fast.Parsers
{
    class JSONParser
    {
        public static Newtonsoft.Json.Linq.JObject Parse(string json_data)
        {
            return Newtonsoft.Json.Linq.JObject.Parse(json_data);
        }

        public static Newtonsoft.Json.Linq.JArray ParseArray(string json_data)
        {
            return Newtonsoft.Json.Linq.JArray.Parse(json_data);
        }

        public static T ParseObject<T>(string json_data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json_data);
        }

        public static string ComposeObject(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
