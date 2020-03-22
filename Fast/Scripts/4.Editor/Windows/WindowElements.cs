using System;
using UnityEngine;

namespace Fast.Editor.Windows
{

#if UNITY_EDITOR

    public class WindowElements
    {
        public static void DropdownHeader(string content, ref bool show_dropdown, WindowStyle style)
        {
            string label = "";

            UnityEngine.Color last_background_color = GUI.backgroundColor;

            if (show_dropdown)
            {
                label += "▼ ";

                GUI.backgroundColor = style.DropdownHeaderShowing.UnityColor;
            }
            else
            {
                label += "► ";

                GUI.backgroundColor = style.DropdownHeaderBase.UnityColor;
            }

            label += content;

            if (GUILayout.Button(label, style.DropdownHeaderButtonStyle))
            {
                show_dropdown = !show_dropdown;

                GUIUtility.keyboardControl = 0;
            }

            GUI.backgroundColor = last_background_color;
        }
    }

#endif

}
