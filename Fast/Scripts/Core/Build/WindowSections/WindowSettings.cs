using System;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Build
{
    class WindowSettings
    {
        public static void OnGUI(BuildWindow window, BuildWindowEditorStyles editor_style, ref BuildWindowData data)
        {
            EditorGUILayout.BeginVertical(editor_style.DropdownContentStyle);
            {
               
            }
            EditorGUILayout.EndVertical();
        }
    }
}

#endif