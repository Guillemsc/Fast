using System;
using UnityEngine;

namespace Fast.Noise
{
    public class PerlinNoise
    {
        public static float Get(float x, float y, float width, float heigth, float offset_x, float offset_y, float scale)
        {
            float ret = 0.0f;

            float final_width = width * scale;
            float final_height = heigth * scale;

            float x_val = x / final_width;
            float y_val = y / final_height;

            x_val += offset_x;
            y_val += offset_y;

            ret = Mathf.PerlinNoise(x_val, y_val);

            return ret;
        }
    }
}
