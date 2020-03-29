#if UNITY_EDITOR

using System;
using UnityEngine;

using UnityEditor;

namespace Fast.Editor
{
    public class EditorStyle
    {
        private GUIStyle main_title_style = null;
        private GUIStyle bold_text_style = null;
        private GUIStyle big_button_style = null;

        // ---------

        private GUIStyle horizontal_line = null;
        private Fast.Color horizontal_line_base = null;

        private GUIStyle big_dropdown_header_button_style = null;
        private Fast.Color big_dropdown_header_base = null;
        private Fast.Color big_dropdown_header_showing = null;

        private GUIStyle dropdown_header_button_style = null;
        private Fast.Color dropdown_header_base = null;
        private Fast.Color dropdown_header_showing = null;

        public EditorStyle()
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

            horizontal_line = new GUIStyle();
            horizontal_line.normal.background = EditorGUIUtility.whiteTexture;
            horizontal_line.margin = new RectOffset(0, 0, 4, 4);
            horizontal_line.fixedHeight = 2;
            horizontal_line_base = new Color(0.4f, 0.4f, 0.4f);

            big_dropdown_header_button_style = new GUIStyle(EditorStyles.toolbarButton);
            big_dropdown_header_button_style.fontSize = 15;
            big_dropdown_header_button_style.fixedHeight = 25;
            big_dropdown_header_button_style.fontStyle = FontStyle.Bold;
            big_dropdown_header_button_style.alignment = TextAnchor.MiddleLeft;
            big_dropdown_header_base = new Color(0.8f, 0.8f, 0.8f);
            big_dropdown_header_showing = new Color(0.6f, 0.6f, 0.6f);

            dropdown_header_button_style = new GUIStyle(EditorStyles.toolbarButton);
            dropdown_header_button_style.fontSize = 12;
            dropdown_header_button_style.fixedHeight = 22;
            dropdown_header_button_style.fontStyle = FontStyle.Normal;
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

        public GUIStyle HorizontalLine => horizontal_line;
        public Fast.Color HorizontalLineBase => horizontal_line_base;

        public GUIStyle BigDropdownHeaderButtonStyle => big_dropdown_header_button_style;
        public Fast.Color BigDropdownHeaderBase => big_dropdown_header_base;
        public Fast.Color BigDropdownHeaderShowing => big_dropdown_header_showing;

        public GUIStyle DropdownHeaderButtonStyle => dropdown_header_button_style;
        public Fast.Color DropdownHeaderBase => dropdown_header_base;
        public Fast.Color DropdownHeaderShowing => dropdown_header_showing;
    }
}

#endif
