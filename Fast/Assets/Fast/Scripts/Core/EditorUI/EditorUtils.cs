using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.EditorUI
{
    class Utils
    {
        public static void DropdownHeader(string content, ref bool show_dropdown, Fast.Color color_base = null,
           Fast.Color color_show = null, GUIStyle style = null)
        {
            string label = "";

            UnityEngine.Color last_background_color = GUI.backgroundColor;

            if (show_dropdown)
            {
                label += "▼ ";

                if (color_show != null)
                {
                    GUI.backgroundColor = color_show.UnityColor;
                }
            }
            else
            {
                label += "► ";

                if (color_base != null)
                {
                    GUI.backgroundColor = color_base.UnityColor;
                }
            }

            label += content;

            if (GUILayout.Button(label, style))
            {
                show_dropdown = !show_dropdown;
                GUIUtility.keyboardControl = 0;
            }

            GUI.backgroundColor = last_background_color;
        }
    }
}

#endif
