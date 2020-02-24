using UnityEngine;

namespace Fast
{
    public class UnityTimer
    {
        bool started = false;
        private float start_time = 0.0f;
        private float start_unscaled_time = 0.0f;

        public void Start()
        {
            started = true;
            start_time = Time.timeSinceLevelLoad;
            start_unscaled_time = Time.realtimeSinceStartup;
        }

        public void Reset()
        {
            started = false;
            start_time = 0.0f;
            start_unscaled_time = 0.0f;
        }

        public float ReadTime()
        {
            float ret = 0.0f;

            if (started)
            {
                ret = Time.timeSinceLevelLoad - start_time;
            }

            return ret;
        }

        public float ReadUnscaledTime()
        {
            float ret = 0.0f;

            if (started)
            {
                ret = Time.realtimeSinceStartup - start_unscaled_time;
            }

            return ret;
        }

        public void AddTime(float time)
        {
            start_time -= time;
        }

        public bool Started
        {
            get { return started; }
        }
    }
}

