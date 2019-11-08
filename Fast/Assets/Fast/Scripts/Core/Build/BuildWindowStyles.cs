using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Build
{
    class BuildWindowEditorStyles
    {
        private GUIStyle main_title_style = null;
        private GUIStyle dropdown_header_style = null;
        private GUIStyle dropdown_content_style = null;
        private GUIStyle build_button_style = null;
        private GUIStyle save_load_button_style = null;

        private Fast.Color header_idle_color = null;
        private Fast.Color header_show_color = null;

        public BuildWindowEditorStyles()
        {
            main_title_style = new GUIStyle(EditorStyles.miniBoldLabel);
            main_title_style.fontSize = 18;
            main_title_style.fontStyle = FontStyle.Bold;
            main_title_style.alignment = TextAnchor.MiddleCenter;
            main_title_style.fixedHeight = 35;
            main_title_style.normal.textColor = new Color32(20, 20, 20, 255);

            dropdown_header_style = new GUIStyle(GUI.skin.button);
            dropdown_header_style.alignment = TextAnchor.MiddleLeft;
            dropdown_header_style.fontStyle = FontStyle.Bold;
            dropdown_header_style.margin = new RectOffset(5, 5, 0, 0);

            dropdown_content_style = new GUIStyle(GUI.skin.textArea);
            dropdown_content_style.padding = new RectOffset(5, 5, 5, 5);
            dropdown_content_style.margin = new RectOffset(15, 5, 0, 0);

            build_button_style = new GUIStyle(EditorStyles.miniButtonMid);
            build_button_style.fontStyle = FontStyle.Bold;
            build_button_style.fixedHeight = 35;
            build_button_style.padding = new RectOffset(5, 5, 5, 5);
            build_button_style.fontSize = 12;

            save_load_button_style = new GUIStyle(EditorStyles.miniButton);
            save_load_button_style.fixedWidth = 45;

            header_idle_color = Fast.Color.FromHex("#457B9D");
            header_show_color = Fast.Color.FromHex("#E63946");
        }

        public GUIStyle MainTitleStyle
        {
            get { return main_title_style; }
        }

        public GUIStyle DropdownHeaderStyle
        {
            get { return dropdown_header_style; }
        }

        public GUIStyle DropdownContentStyle
        {
            get { return dropdown_content_style; }
        }

        public GUIStyle BuildButtonStyle
        {
            get { return build_button_style; }
        }

        public GUIStyle SaveLoadButtonStyle
        {
            get { return save_load_button_style; }
        }

        public Fast.Color HeaderIdleColor
        {
            get { return header_idle_color; }
        }

        public Fast.Color HeaderShowColor
        {
            get { return header_show_color; }
        }
    }
}

#endif
