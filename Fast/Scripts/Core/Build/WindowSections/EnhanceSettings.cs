using System;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Build
{
    class EnhanceSettings
    {
        public static void OnGUI(BuildWindow window, BuildWindowEditorStyles editor_style, ref BuildWindowData data)
        {
            EditorGUILayout.BeginVertical(editor_style.DropdownContentStyle);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Enhance Key", GUILayout.MaxWidth(90));

                    data.settings.enhance_android_settings.enhance_key =
                        EditorGUILayout.TextField(data.settings.enhance_android_settings.enhance_key);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Enhance Secret", GUILayout.MaxWidth(100));

                    data.settings.enhance_android_settings.enhance_secret =
                        EditorGUILayout.TextField(data.settings.enhance_android_settings.enhance_secret);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    data.settings.enhance_android_settings.chartboost =
                        EditorGUILayout.Toggle("", data.settings.enhance_android_settings.chartboost, GUILayout.MaxWidth(10));

                    Fast.EditorTools.Utils.DropdownHeader("ChartBoost", ref data.enhance_android_chartboost_dropdown,
                        editor_style.HeaderIdleColor, editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);
                }
                EditorGUILayout.EndHorizontal();


                if (data.enhance_android_chartboost_dropdown)
                {
                    EditorGUILayout.LabelField("App ID", GUILayout.MaxWidth(100));
                    data.settings.enhance_android_settings.chartboost_app_id =
                        EditorGUILayout.TextField(data.settings.enhance_android_settings.chartboost_app_id);

                    EditorGUILayout.LabelField("App Signature", GUILayout.MaxWidth(100));
                    data.settings.enhance_android_settings.chartboost_app_signature =
                        EditorGUILayout.TextField(data.settings.enhance_android_settings.chartboost_app_signature);
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}

#endif
