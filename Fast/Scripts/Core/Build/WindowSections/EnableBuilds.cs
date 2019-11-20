using System;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Build
{
    class EnableBuilds
    {
        public static void OnGUI(BuildWindow window, BuildWindowEditorStyles editor_style, ref BuildWindowData data)
        {
            EditorGUILayout.BeginVertical(editor_style.DropdownContentStyle);
            {
                data.settings.enable_builds.build_windows =
                    EditorGUILayout.Toggle("Windows", data.settings.enable_builds.build_windows);
                data.settings.enable_builds.build_android =
                    EditorGUILayout.Toggle("Android", data.settings.enable_builds.build_android);

                EditorGUILayout.Separator();

                EditorGUI.BeginDisabledGroup(!data.settings.enable_builds.build_android);
                if (data.settings.enable_builds.build_android)
                {
                    data.settings.enable_post_builds.post_build_enhance_android = EditorGUILayout.Toggle("Enhance Android",
                        data.settings.enable_post_builds.post_build_enhance_android);
                }
                else
                {
                    EditorGUILayout.Toggle("Enhance Android", false);
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();
        }
    }
}

#endif