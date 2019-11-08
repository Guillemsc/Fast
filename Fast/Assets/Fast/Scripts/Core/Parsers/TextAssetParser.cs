using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Fast.Parsers
{
    class TextAssetParser
    {
        public static List<string> Parse(TextAsset asset)
        {
            List<string> ret = new List<string>();

            if (asset != null)
            {
                ret = asset.text.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n').ToList();
            }
            else
            {
                Debug.LogError("[Fast.Parsers.TextAssetParser] Text asset is null and cannot be parsed");
            }

            return ret;
        }
    }
}
