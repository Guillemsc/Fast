using System;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Fast.Serializers
{
    public class JSONSerializer
    {
        public static bool SerializeToPath(string filepath, object to_serialize)
        {
            bool ret = false;

            FileUtils.CreateAllFilepathDirectories(filepath);

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(to_serialize, Newtonsoft.Json.Formatting.Indented);

            FileUtils.DeleteFileIfExists(filepath);

            File.WriteAllBytes(filepath, new byte[0]);
            File.WriteAllText(filepath, data);

            ret = true;

            return ret;
        }

        public static async Task<bool> SerializeToPathAsync(string filepath, object to_serialize)
        {
            FileUtils.CreateAllFilepathDirectories(filepath);

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(to_serialize, Newtonsoft.Json.Formatting.Indented);

            FileUtils.DeleteFileIfExists(filepath);

            StreamWriter writer = File.CreateText(filepath);

            Task write_task = writer.WriteAsync(data);
            AwaitResult write_result = await AwaitUtils.AwaitTask(write_task);

            writer.Dispose();

            if (write_result.HasErrors)
            {
                return false;
            }

            return true;
        }

        public static bool DeSerializeFromPath<T>(string filepath, out T deserialized_object)
        {
            bool ret = false;

            deserialized_object = default;

            if (File.Exists(filepath))
            {
                string text = File.ReadAllText(filepath);

                deserialized_object = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(text);

                if (deserialized_object != null)
                {
                    ret = true;
                }
            }
            
            return ret;
        }


        public static async Task<T> DeSerializeFromPathAsync<T>(string filepath)
        {
            FileUtils.CreateAllFilepathDirectories(filepath);

            if (!File.Exists(filepath))
            {
                return default(T);
            }

            StreamReader reader = new StreamReader(filepath);

            Task<string>read_task = reader.ReadToEndAsync();
            AwaitResult read_result = await AwaitUtils.AwaitTask(read_task);

            if(read_result.HasErrors)
            {
                return default(T);
            }

            T deserialized_object = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(read_task.Result);

            reader.Dispose();

            return deserialized_object;
        }

        public static async Task<bool> SerializeToPersistentPathAsync(string persistent_path, object to_serialize)
        {
            string filepath = Application.persistentDataPath + Path.DirectorySeparatorChar + persistent_path;

            return await SerializeToPathAsync(filepath, to_serialize);
        }

        public static async Task<T> DeSerializeFromPersistentPathAsync<T>(string persistent_path)
        {
            string filepath = Application.persistentDataPath + Path.DirectorySeparatorChar + persistent_path;

            return await DeSerializeFromPathAsync<T>(filepath);
        }

#if UNITY_EDITOR

        public static bool SerializToAssetsPath(string assets_filepath, object to_serialize, bool refresh_asset_database = true)
        {
            bool ret = false;

            string filepath = Application.dataPath + Path.DirectorySeparatorChar + assets_filepath;

            ret = SerializeToPath(filepath, to_serialize);

            if (refresh_asset_database)
            {
                AssetDatabase.SaveAssets();

                AssetDatabase.Refresh();
            }

            ret = true;

            return ret;
        }

        public static bool DeSerializeFromAssetsPath<T>(string assets_filepath, out T deserialized_object)
        {
            bool ret = false;

            string path = Application.dataPath + Path.DirectorySeparatorChar + assets_filepath;

            ret = DeSerializeFromPath(path, out deserialized_object);

            return ret;
        }

#endif

    }
}
