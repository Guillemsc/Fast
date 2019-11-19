using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Build
{
    class BuildWindow : EditorWindow
    {
        [SerializeField] private BuildSettings settings = new BuildSettings();

        private BuildWindowEditorStyles editor_style = null;

        private PlatformBuilderController builder = null;

        [SerializeField] private bool basic_settings_dropdown = false;
        [SerializeField] private bool advanced_settings_dropdown = false;
        [SerializeField] private bool windows_settings_dropdown = false;
        [SerializeField] private bool android_settings_dropdown = false;
        [SerializeField] private bool enhance_android_settings_dropdown = false;
        [SerializeField] private bool enable_builds_dropdown = false;

        [SerializeField] private bool enhance_android_chartboost_dropdown = false;

        private Vector2 scroll_pos = new Vector2();

        private string curr_build_progress = "";

        [MenuItem("Fast/Build")]
        public static void ShowWindow()
        {
            Debug.Log("Hi");

            GetWindow<BuildWindow>("Fast Build");
        }

        private void InitStyles()
        {
            if (editor_style == null)
                editor_style = new BuildWindowEditorStyles();
        }

        private void OnGUI()
        {
            InitStyles();

            this.minSize = new Vector2(275, this.minSize.y);

            scroll_pos = EditorGUILayout.BeginScrollView(scroll_pos);

            GUILayout.Label("FAST BUILD", editor_style.MainTitleStyle);

            EditorGUILayout.BeginHorizontal();
            {
                Fast.EditorUI.Utils.DropdownHeader("Basic Settings", ref basic_settings_dropdown, editor_style.HeaderIdleColor,
                    editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);

                if (GUILayout.Button("Save", editor_style.SaveLoadButtonStyle))
                {
                    string path_to_save = EditorUtility.SaveFilePanel("Save", "", "BasicSettings", settings.basic_settings.Extension);

                    Serializers.JSONSerializer.SerializeObject(path_to_save, settings.basic_settings);
                }

                if (GUILayout.Button("Load", editor_style.SaveLoadButtonStyle))
                {
                    string file_to_load = EditorUtility.OpenFilePanel("Load", "", settings.basic_settings.Extension);

                    Serializers.JSONSerializer.DeSerializeObject(file_to_load, ref settings.basic_settings);

                    this.Repaint();
                }
            }
            EditorGUILayout.EndHorizontal();

            if (basic_settings_dropdown)
            {
                DrawBasicSettings();
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                Fast.EditorUI.Utils.DropdownHeader("Advanced Settings", ref advanced_settings_dropdown, editor_style.HeaderIdleColor,
                    editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);

                if (GUILayout.Button("Save", editor_style.SaveLoadButtonStyle))
                {
                    string path_to_save = EditorUtility.SaveFilePanel("Save", "", "AdvancedSettings", settings.advanced_settings.Extension);

                    Serializers.JSONSerializer.SerializeObject(path_to_save, settings.advanced_settings);
                }

                if (GUILayout.Button("Load", editor_style.SaveLoadButtonStyle))
                {
                    string file_to_load = EditorUtility.OpenFilePanel("Load", "", settings.basic_settings.Extension);

                    Serializers.JSONSerializer.DeSerializeObject(file_to_load, ref settings.basic_settings);

                    this.Repaint();
                }
            }
            EditorGUILayout.EndHorizontal();

            if (advanced_settings_dropdown)
            {
                DrawAdvancedSettings();
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                Fast.EditorUI.Utils.DropdownHeader("Windows Settings", ref windows_settings_dropdown, editor_style.HeaderIdleColor,
                editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);

                if (GUILayout.Button("Save", editor_style.SaveLoadButtonStyle))
                {
                    string path_to_save = EditorUtility.SaveFilePanel("Save", "", "WindowsSettings", settings.windows_settings.Extension);

                    Serializers.JSONSerializer.SerializeObject(path_to_save, settings.windows_settings);
                }

                if (GUILayout.Button("Load", editor_style.SaveLoadButtonStyle))
                {
                    string file_to_load = EditorUtility.OpenFilePanel("Load", "", settings.windows_settings.Extension);

                    Serializers.JSONSerializer.DeSerializeObject(file_to_load, ref settings.windows_settings);

                    this.Repaint();
                }
            }
            EditorGUILayout.EndHorizontal();

            if (windows_settings_dropdown)
            {
                DrawWindowsSettings();
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                Fast.EditorUI.Utils.DropdownHeader("Android Settings", ref android_settings_dropdown, editor_style.HeaderIdleColor,
                editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);

                if (GUILayout.Button("Save", editor_style.SaveLoadButtonStyle))
                {
                    string path_to_save = EditorUtility.SaveFilePanel("Save", "", "AndroidSettings", settings.android_settings.Extension);

                    Serializers.JSONSerializer.SerializeObject(path_to_save, settings.windows_settings);
                }

                if (GUILayout.Button("Load", editor_style.SaveLoadButtonStyle))
                {
                    string file_to_load = EditorUtility.OpenFilePanel("Load", "", settings.android_settings.Extension);

                    Serializers.JSONSerializer.DeSerializeObject(file_to_load, ref settings.windows_settings);

                    this.Repaint();
                }
            }
            EditorGUILayout.EndHorizontal();

            if (android_settings_dropdown)
            {
                DrawAndroidSettings();
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                Fast.EditorUI.Utils.DropdownHeader("Enhance Android Settings", ref enhance_android_settings_dropdown, editor_style.HeaderIdleColor,
                editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);

                if (GUILayout.Button("Save", editor_style.SaveLoadButtonStyle))
                {
                    string path_to_save = EditorUtility.SaveFilePanel("Save", "", "EnhanceAndroidSettings", settings.enhance_android_settings.Extension);

                    Serializers.JSONSerializer.SerializeObject(path_to_save, settings.windows_settings);
                }

                if (GUILayout.Button("Load", editor_style.SaveLoadButtonStyle))
                {
                    string file_to_load = EditorUtility.OpenFilePanel("Load", "", settings.enhance_android_settings.Extension);

                    Serializers.JSONSerializer.DeSerializeObject(file_to_load, ref settings.windows_settings);

                    this.Repaint();
                }
            }
            EditorGUILayout.EndHorizontal();

            if (enhance_android_settings_dropdown)
            {
                DrawEnhanceAndroidSettings();
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                Fast.EditorUI.Utils.DropdownHeader("Enable Builds", ref enable_builds_dropdown, editor_style.HeaderIdleColor,
                editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);

                if (GUILayout.Button("Save", editor_style.SaveLoadButtonStyle))
                {
                    string path_to_save = EditorUtility.SaveFilePanel("Save", "", "EnableBuilds", settings.enable_builds.Extension);

                    Serializers.JSONSerializer.SerializeObject(path_to_save, settings.enable_builds);
                }

                if (GUILayout.Button("Load", editor_style.SaveLoadButtonStyle))
                {
                    string file_to_load = EditorUtility.OpenFilePanel("Load", "", settings.enable_builds.Extension);

                    Serializers.JSONSerializer.DeSerializeObject(file_to_load, ref settings.enable_builds);

                    this.Repaint();
                }
            }
            EditorGUILayout.EndHorizontal();

            if (enable_builds_dropdown)
            {
                DrawEnableBuilds();
            }

            GUILayout.Space(10);

            EditorGUI.BeginDisabledGroup(builder != null);
            if (GUILayout.Button("Perform All Enabled Builds", editor_style.BuildButtonStyle))
            {
                curr_build_progress = "";

                settings.basic_settings.scenes_to_build = new List<string> { "Assets/Scenes/SampleScene.unity" };

                builder = new PlatformBuilderController();

                builder.OnProgressUpdate.Subscribe(delegate (string progress)
                {
                    curr_build_progress = progress;

                    this.Repaint();
                });

                List<string> errors = new List<string>();

                builder.BuildAll(settings, 
                delegate()
                {
                    builder = null;

                    curr_build_progress = "Build finished with " + errors.Count.ToString() + " errors";

                    this.Repaint();
                });
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.LabelField("Progress: " + curr_build_progress);

            EditorGUILayout.EndScrollView();
        }

        private void DrawBasicSettings()
        {
            EditorGUILayout.BeginVertical(editor_style.DropdownContentStyle);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Game name", GUILayout.MaxWidth(80));

                    settings.basic_settings.game_name = EditorGUILayout.TextField(settings.basic_settings.game_name);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Company name", GUILayout.MaxWidth(100));

                    settings.basic_settings.company_name = EditorGUILayout.TextField(settings.basic_settings.company_name);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Version", GUILayout.MaxWidth(70));

                    settings.basic_settings.version = EditorGUILayout.IntField(settings.basic_settings.version);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Base Build folder", GUILayout.MaxWidth(105));

                    EditorGUILayout.TextField(settings.basic_settings.base_build_folder);

                    if (GUILayout.Button("...", GUILayout.ExpandWidth(false)))
                    {
                        string last_path = settings.basic_settings.base_build_folder;

                        settings.basic_settings.base_build_folder = EditorUtility.OpenFolderPanel("Select folder", "", "");

                        if (last_path != settings.basic_settings.base_build_folder)
                        {
                            this.Repaint();
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawAdvancedSettings()
        {
            EditorGUILayout.BeginVertical(editor_style.DropdownContentStyle);
            {
                settings.advanced_settings.development_build = EditorGUILayout.Toggle("Development build",
                    settings.advanced_settings.development_build);

                settings.advanced_settings.mute_other_audio_devices = EditorGUILayout.Toggle("Mute other audio devices",
                    settings.advanced_settings.mute_other_audio_devices);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawWindowsSettings()
        {
            EditorGUILayout.BeginVertical(editor_style.DropdownContentStyle);
            {

            }
            EditorGUILayout.EndVertical();
        }

        private void DrawAndroidSettings()
        {
            EditorGUILayout.BeginVertical(editor_style.DropdownContentStyle);
            {
                GUILayout.Space(3);

                EditorGUILayout.HelpBox("Keystore and Alias need to be setup on the PlayerSettings", MessageType.None);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Keystore Password", GUILayout.MaxWidth(120));

                    settings.android_settings.keystore_password = EditorGUILayout.PasswordField(settings.android_settings.keystore_password);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Alias Password", GUILayout.MaxWidth(110));

                    settings.android_settings.alias_password = EditorGUILayout.PasswordField(settings.android_settings.alias_password);
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(3);

                EditorGUILayout.HelpBox("Needs to follow the 'com.Company.ProductName' structure", MessageType.None);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Package Name", GUILayout.MaxWidth(100));

                    settings.android_settings.package_name = EditorGUILayout.TextField(settings.android_settings.package_name);
                }
                EditorGUILayout.EndHorizontal();

                settings.android_settings.google_play_ready = EditorGUILayout.Toggle("GooglePlay Ready", 
                    settings.android_settings.google_play_ready);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawEnhanceAndroidSettings()
        {
            EditorGUILayout.BeginVertical(editor_style.DropdownContentStyle);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Enhance Key", GUILayout.MaxWidth(90));

                    settings.enhance_android_settings.enhance_key = EditorGUILayout.TextField(settings.enhance_android_settings.enhance_key);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Enhance Secret", GUILayout.MaxWidth(100));

                    settings.enhance_android_settings.enhance_secret = EditorGUILayout.TextField(settings.enhance_android_settings.enhance_secret);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    settings.enhance_android_settings.chartboost = EditorGUILayout.Toggle("", settings.enhance_android_settings.chartboost, GUILayout.MaxWidth(10));

                    Fast.EditorUI.Utils.DropdownHeader("ChartBoost", ref enhance_android_chartboost_dropdown, editor_style.HeaderIdleColor,
                    editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);
                }
                EditorGUILayout.EndHorizontal();


                if (enhance_android_chartboost_dropdown)
                {
                    EditorGUILayout.LabelField("App ID", GUILayout.MaxWidth(100));
                    settings.enhance_android_settings.chartboost_app_id = EditorGUILayout.TextField(settings.enhance_android_settings.chartboost_app_id);

                    EditorGUILayout.LabelField("App Signature", GUILayout.MaxWidth(100));
                    settings.enhance_android_settings.chartboost_app_signature = EditorGUILayout.TextField(settings.enhance_android_settings.chartboost_app_signature);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawEnableBuilds()
        {
            EditorGUILayout.BeginVertical(editor_style.DropdownContentStyle);
            {
                settings.enable_builds.build_windows = EditorGUILayout.Toggle("Windows", settings.enable_builds.build_windows);
                settings.enable_builds.build_android = EditorGUILayout.Toggle("Android", settings.enable_builds.build_android);

                EditorGUILayout.Separator();

                EditorGUI.BeginDisabledGroup(!settings.enable_builds.build_android);
                if (settings.enable_builds.build_android)
                {
                    settings.enable_post_builds.post_build_enhance_android = EditorGUILayout.Toggle("Enhance Android",
                        settings.enable_post_builds.post_build_enhance_android);
                }
                else
                {
                    EditorGUILayout.Toggle("Enhance Android", false);
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();
        }

        // EditorGUILayout.HelpBox("Despasito", MessageType.Info);
        // settings.BaseBuildFolder = EditorUtility.OpenFolderPanel("Select folder", "", "");
        // EditorGUILayout.LabelField("Base Build folder", GUILayout.MaxWidth(100));
        // EditorGUILayout.TextField(settings.BaseBuildFolder);
    }
}

#endif
