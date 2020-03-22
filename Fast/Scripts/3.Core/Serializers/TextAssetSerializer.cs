using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Serializers
{
    public class TextAssetSerializer
    {
        public static bool SerializeToAssetsPath(string assets_filepath, string data, bool refresh_asset_database = true)
        {
            bool ret = false;

            string filepath = Application.dataPath + Path.DirectorySeparatorChar + assets_filepath;

            FileUtils.CreateAllFilepathDirectories(filepath);

            File.WriteAllText(filepath, data);

            if (refresh_asset_database)
            {
                AssetDatabase.SaveAssets();

                AssetDatabase.Refresh();
            }

            ret = true;

            return ret;
        }
    }
}

#endif
