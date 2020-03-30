#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace Fast.Editor
{
    class EditorHelper : UnityEditor.Editor
    {
        private bool fist_time = true;

        private EditorStyle style = null;

        public EditorStyle Style => style;

        private void StartDrawGUI()
        {
            if (fist_time)
            {
                fist_time = false;

                style = new EditorStyle();
            }
        }

        private void EndDrawGUI()
        {

        }

        public override void OnInspectorGUI()
        {
            StartDrawGUI();

            OnDrawInspectorGUI();

            EndDrawGUI();
        }

        protected virtual void OnDrawInspectorGUI()
        {

        }
    }
}

#endif
