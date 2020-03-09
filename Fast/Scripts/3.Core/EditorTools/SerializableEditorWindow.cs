using System;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.EditorTools
{
    public class SerializableEditorWindow<T> : EditorWindow
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
            Fast.Serializers.JSONSerializer.SerializToAssetsPath(assets_path, serializable_object, false);
        }

        public void DeSerialize(out T serializable_object)
        {
            Fast.Serializers.JSONSerializer.DeSerializeFromAssetsPath(assets_path, out serializable_object);
        }
    }
}

#endif
