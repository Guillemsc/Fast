using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Tooling
{
    class SerializableEditorWindow<T> : EditorWindow
    {
        private string window_uid = "";

        private string assets_path = "";

        public SerializableEditorWindow(string window_uid)
        {
            this.window_uid = window_uid;

            assets_path = "Editor/Fast/SerializableEditorWindows/" + window_uid + ".json";
        }

        public void Serialize(T serializable_object)
        {
            Fast.Serializers.JSONSerializer.SerializeObjectAssetsPath(assets_path, serializable_object, false);
        }

        public void DeSerialize(ref T serializable_object)
        {
            Fast.Serializers.JSONSerializer.DeSerializeObjectAssetsPath(assets_path, ref serializable_object);
        }
    }
}

#endif
