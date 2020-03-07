using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast
{
    [System.Serializable]
    public class RandomGenerator
    {
        private System.Random rand = new System.Random();

        public int GetInt()
        {
            return rand.Next(int.MinValue, int.MaxValue);
        }

        public int GetInt(int min, int max)
        {
            return rand.Next(min, max);
        }

        public float GetFloat()
        {
            float ret = 0.0f;

            double range = (double)float.MaxValue - (double)float.MinValue;
            double sample = rand.NextDouble();
            double scaled = (sample * range) + float.MinValue;

            ret = (float)scaled;

            return ret;
        }

        public float GetFloat(float min, float max)
        {
            float ret = 0.0f;

            double range = (double)min - (double)max;
            double sample = rand.NextDouble();
            double scaled = (sample * range) + float.MinValue;

            ret = (float)scaled;

            return ret;
        }
    }
}
