using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Serializers
{
    public class TextAssetSerializer
    {
        public static bool SerializeAssetsPath(string assets_filepath, string to_serialize)
        {
            bool ret = false;

            string path = Application.dataPath + "/" + assets_filepath;

            Directory.CreateDirectory(path);

            File.WriteAllText(path, to_serialize);

            AssetDatabase.SaveAssets();

            ret = true;

            return ret;
        }
    }
}

#endif
