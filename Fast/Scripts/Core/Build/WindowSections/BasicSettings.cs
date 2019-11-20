using System;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Build
{
    class BasicSettings
    {
        public static void OnGUI(BuildWindow window, BuildWindowEditorStyles editor_style, ref BuildWindowData data)
        {
            EditorGUILayout.BeginVertical(editor_style.DropdownContentStyle);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Game name", GUILayout.MaxWidth(80));

                    data.settings.basic_settings.game_name =
                        EditorGUILayout.TextField(data.settings.basic_settings.game_name);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Company name", GUILayout.MaxWidth(100));

                    data.settings.basic_settings.company_name =
                        EditorGUILayout.TextField(data.settings.basic_settings.company_name);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Version", GUILayout.MaxWidth(70));

                    data.settings.basic_settings.version =
                        EditorGUILayout.IntField(data.settings.basic_settings.version);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Base Build folder", GUILayout.MaxWidth(105));

                    EditorGUILayout.TextField(data.settings.basic_settings.base_build_folder);

                    if (GUILayout.Button("...", GUILayout.ExpandWidth(false)))
                    {
                        string last_path = data.settings.basic_settings.base_build_folder;

                        data.settings.basic_settings.base_build_folder = EditorUtility.OpenFolderPanel("Select folder", "", "");

                        if (last_path != data.settings.basic_settings.base_build_folder)
                        {
                            window.Repaint();
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
    }
}

#endif
