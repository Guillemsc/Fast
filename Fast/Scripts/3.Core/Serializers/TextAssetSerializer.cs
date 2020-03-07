using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Serializers
{
    public class TextAssetSerializer
    {
        public static bool SerializeToAssetsPath(string assets_filepath, string to_serialize)
        {
            bool ret = false;

            string path = Application.dataPath + "/" + assets_filepath;

            FileInfo info = new FileInfo(path);

            Directory.CreateDirectory(info.Directory.FullName);

            File.WriteAllText(path, to_serialize);

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();

            ret = true;

            return ret;
        }
    }
}

#endif
