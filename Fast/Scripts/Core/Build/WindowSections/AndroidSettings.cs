using System;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Build
{
    class AndroidSettings
    {
        public static void OnGUI(BuildWindow window, BuildWindowEditorStyles editor_style, ref BuildWindowData data)
        {
            EditorGUILayout.BeginVertical(editor_style.DropdownContentStyle);
            {
                GUILayout.Space(3);

                EditorGUILayout.HelpBox("Keystore and Alias need to be setup on the PlayerSettings", MessageType.None);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Keystore Password", GUILayout.MaxWidth(120));

                    data.settings.android_settings.keystore_password =
                        EditorGUILayout.PasswordField(data.settings.android_settings.keystore_password);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Alias Password", GUILayout.MaxWidth(110));

                    data.settings.android_settings.alias_password =
                        EditorGUILayout.PasswordField(data.settings.android_settings.alias_password);
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(3);

                EditorGUILayout.HelpBox("Needs to follow the 'com.Company.ProductName' structure", MessageType.None);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Package Name", GUILayout.MaxWidth(100));

                    data.settings.android_settings.package_name =
                        EditorGUILayout.TextField(data.settings.android_settings.package_name);
                }
                EditorGUILayout.EndHorizontal();

                data.settings.android_settings.google_play_ready = EditorGUILayout.Toggle("GooglePlay Ready",
                    data.settings.android_settings.google_play_ready);
            }
            EditorGUILayout.EndVertical();
        }
    }
}

#endif