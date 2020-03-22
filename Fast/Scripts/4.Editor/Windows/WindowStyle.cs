using System;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Editor.Windows
{
    public class WindowStyle
    {
        private GUIStyle main_title_style = null;
        private GUIStyle bold_text_style = null;
        private GUIStyle big_button_style = null;

        // ---------

        private GUIStyle dropdown_header_button_style = null;
        private Fast.Color dropdown_header_base = null;
        private Fast.Color dropdown_header_showing = null;

        public WindowStyle()
        {
            main_title_style = new GUIStyle(EditorStyles.miniBoldLabel);
            main_title_style.fontSize = 18;
            main_title_style.fontStyle = FontStyle.Bold;
            main_title_style.alignment = TextAnchor.MiddleCenter;
            main_title_style.fixedHeight = 35;
            main_title_style.normal.textColor = new Color32(20, 20, 20, 255);

            bold_text_style = new GUIStyle();
            bold_text_style.fontStyle = FontStyle.Bold;

            big_button_style = new GUIStyle(EditorStyles.miniButton);
            big_button_style.fontSize = 15;
            big_button_style.fixedHeight = 25;

            dropdown_header_button_style = new GUIStyle(EditorStyles.toolbarButton);
            dropdown_header_button_style.fontSize = 15;
            dropdown_header_button_style.fixedHeight = 25;
            dropdown_header_button_style.fontStyle = FontStyle.Bold;
            dropdown_header_button_style.alignment = TextAnchor.MiddleLeft;

            dropdown_header_base = new Color(0.8f, 0.8f, 0.8f);
            dropdown_header_showing = new Color(0.6f, 0.6f, 0.6f);
        }

        public GUIStyle MainTitleStyle
        {
            get { return main_title_style; }
        }

        public GUIStyle BoldTextStyle
        {
            get { return bold_text_style; }
        }

        public GUIStyle BigButtonStyle => big_button_style;

        public GUIStyle DropdownHeaderButtonStyle => dropdown_header_button_style;
        public Fast.Color DropdownHeaderBase => dropdown_header_base;
        public Fast.Color DropdownHeaderShowing => dropdown_header_showing;
    }
}

#endif
