using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Build
{
    [System.Serializable]
    public class BuildWindowData
    {
        public BuildSettings settings = new BuildSettings();

        public bool basic_settings_dropdown = false;
        public bool advanced_settings_dropdown = false;
        public bool windows_settings_dropdown = false;
        public bool android_settings_dropdown = false;
        public bool enhance_android_settings_dropdown = false;
        public bool enable_builds_dropdown = false;

        public bool enhance_android_chartboost_dropdown = false;
    }

    class BuildWindow : Fast.EditorTools.SerializableEditorWindow<BuildWindowData>
    {
        [SerializeField] private BuildWindowData serialized_data = new BuildWindowData();

        private BuildWindowEditorStyles editor_style = null;

        private PlatformBuilderController builder = null;

        private Vector2 scroll_pos = new Vector2();

        private string curr_build_progress = "";

        BuildWindow() : base("Fast-Build-BuildWindow")
        {
            
        }

        [MenuItem("Fast/Build")]
        public static void ShowWindow()
        {
            GetWindow<BuildWindow>("Fast Build");
        }

        private void Awake()
        {
            DeSerialize(ref serialized_data);
        }

        private void OnLostFocus()
        {
            Serialize(serialized_data);
        }

        private void OnDestroy()
        {
            Serialize(serialized_data);
        }

        private void OnEnable()
        {
            DeSerialize(ref serialized_data);
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
                Fast.EditorTools.Utils.DropdownHeader("Basic Settings", ref serialized_data.basic_settings_dropdown, 
                    editor_style.HeaderIdleColor, editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);
            }
            EditorGUILayout.EndHorizontal();

            if (serialized_data.basic_settings_dropdown)
            {
                BasicSettings.OnGUI(this, editor_style, ref serialized_data);
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                Fast.EditorTools.Utils.DropdownHeader("Advanced Settings", ref serialized_data.advanced_settings_dropdown, editor_style.HeaderIdleColor,
                    editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);
            }
            EditorGUILayout.EndHorizontal();

            if (serialized_data.advanced_settings_dropdown)
            {
                AdvancedSettings.OnGUI(this, editor_style, ref serialized_data);
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                Fast.EditorTools.Utils.DropdownHeader("Windows Settings", ref serialized_data.windows_settings_dropdown, 
                    editor_style.HeaderIdleColor, editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);
            }
            EditorGUILayout.EndHorizontal();

            if (serialized_data.windows_settings_dropdown)
            {
                WindowSettings.OnGUI(this, editor_style, ref serialized_data);
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                Fast.EditorTools.Utils.DropdownHeader("Android Settings", ref serialized_data.android_settings_dropdown, 
                    editor_style.HeaderIdleColor, editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);
            }
            EditorGUILayout.EndHorizontal();

            if (serialized_data.android_settings_dropdown)
            {
                AndroidSettings.OnGUI(this, editor_style, ref serialized_data);
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                Fast.EditorTools.Utils.DropdownHeader("Enhance Android Settings", ref serialized_data.enhance_android_settings_dropdown, 
                    editor_style.HeaderIdleColor, editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);
            }
            EditorGUILayout.EndHorizontal();

            if (serialized_data.enhance_android_settings_dropdown)
            {
                EnhanceSettings.OnGUI(this, editor_style, ref serialized_data);
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                Fast.EditorTools.Utils.DropdownHeader("Enable Builds", ref serialized_data.enable_builds_dropdown, 
                    editor_style.HeaderIdleColor, editor_style.HeaderShowColor, editor_style.DropdownHeaderStyle);
            }
            EditorGUILayout.EndHorizontal();

            if (serialized_data.enable_builds_dropdown)
            {
                EnableBuilds.OnGUI(this, editor_style, ref serialized_data);
            }

            GUILayout.Space(10);

            EditorGUI.BeginDisabledGroup(builder != null);
            if (GUILayout.Button("Perform All Enabled Builds", editor_style.BuildButtonStyle))
            {
                curr_build_progress = "";

                serialized_data.settings.basic_settings.scenes_to_build = new List<string> { "Assets/Scenes/SampleScene.unity" };

                builder = new PlatformBuilderController();

                builder.OnProgressUpdate.Subscribe(delegate (string progress)
                {
                    curr_build_progress = progress;

                    this.Repaint();
                });

                List<string> errors = new List<string>();

                builder.BuildAll(serialized_data.settings, 
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
    }
}

#endif
