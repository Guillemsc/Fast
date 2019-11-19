using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.EditorUI
{
    public class Styles
    {
        private GUIStyle main_title_style = null;

        public Styles()
        {
            main_title_style = new GUIStyle(EditorStyles.miniBoldLabel);
            main_title_style.fontSize = 18;
            main_title_style.fontStyle = FontStyle.Bold;
            main_title_style.alignment = TextAnchor.MiddleCenter;
            main_title_style.fixedHeight = 35;
            main_title_style.normal.textColor = new Color32(20, 20, 20, 255);
        }

        public GUIStyle MainTitleStyle
        {
            get { return main_title_style; }
        }
    }
}

#endif
