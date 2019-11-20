using System;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Build
{
    class AdvancedSettings
    {
        public static void OnGUI(BuildWindow window, BuildWindowEditorStyles editor_style, ref BuildWindowData data)
        {
            EditorGUILayout.BeginVertical(editor_style.DropdownContentStyle);
            {
                data.settings.advanced_settings.development_build = EditorGUILayout.Toggle("Development build",
                    data.settings.advanced_settings.development_build);

                data.settings.advanced_settings.mute_other_audio_devices = EditorGUILayout.Toggle("Mute other audio devices",
                    data.settings.advanced_settings.mute_other_audio_devices);
            }
            EditorGUILayout.EndVertical();
        }
    }
}

#endif
