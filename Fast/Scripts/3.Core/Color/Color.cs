﻿using UnityEngine;

namespace Fast
{
    public class Color
    {
        private UnityEngine.Color color;

        public Color()
        {

        }

        public Color(float r, float g, float b)
        {
            this.color = new UnityEngine.Color(r, g, b);
        }

        public Color(UnityEngine.Color color)
        {
            this.color = color;
        }

        public static Color FromHex(string hex_color)
        {
            Color ret = new Color();

            ColorUtility.TryParseHtmlString(hex_color, out ret.color);

            return ret;
        }

        public UnityEngine.Color UnityColor
        {
            get { return color; }
        }
    }
}
