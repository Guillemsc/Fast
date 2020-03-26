#if UNITY_EDITOR

using System;
using UnityEngine;

using UnityEditor;

namespace Fast.Editor.Windows
{
    public class WindowHelper<T> : EditorWindow where T : WindowHelperData
    {
        private bool fist_time = true;

        private WindowStyle style = null;

        private T helper_data = default(T);

        public WindowStyle Style => style;

        protected void SetWindowData(Vector2 min_size)
        {
            minSize = min_size;
        }

        protected T Data => helper_data;

        private void Awake()
        {
            LoadHelperData();
            SaveHelperData();
        }

        private void OnLostFocus()
        {
            LoadHelperData();
            SaveHelperData();
        }

        private void OnDestroy()
        {
            LoadHelperData();
            SaveHelperData();
        }

        private void OnEnable()
        {
            LoadHelperData();
            SaveHelperData();
        }

        private void OnGUI()
        {
            StartDrawGUI();

            OnDrawGui();

            FinishDrawGUI();
        }

        protected virtual void OnDrawGui()
        {

        }

        private void StartDrawGUI()
        {
            if(fist_time)
            {
                fist_time = false;

                style = new WindowStyle();
            }

            Vector2 scroll_pos = EditorGUILayout.BeginScrollView(helper_data.ScrollPos.ToVector2());
            helper_data.ScrollPos = scroll_pos.ToFloat2();
        }

        protected void FinishDrawGUI()
        {
            EditorGUILayout.EndScrollView();
        }

        private void LoadHelperData()
        {
            if(helper_data != null)
            {
                return;
            }

            string persistent_path = $"{Application.dataPath}/Editor/Fast/Windows/{GetType().Name}/data";
            bool could_load = Fast.Serializers.JSONSerializer.DeSerializeFromPath(persistent_path, out helper_data);

            if(could_load)
            {
                return;
            }
        }

        private void SaveHelperData()
        {
            if(helper_data == null)
            {
                helper_data = Activator.CreateInstance<T>();
            }

            string persistent_path = $"{Application.dataPath}/Editor/Fast/Windows/{GetType().Name}/data";
            Fast.Serializers.JSONSerializer.SerializeToPath(persistent_path, helper_data);
        }
    }
}

#endif
