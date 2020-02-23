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
            if (started)
                return Time.timeSinceLevelLoad - start_time;
            else
                return 0.0f;
        }

        public float ReadUnscaledTime()
        {
            if (started)
                return Time.realtimeSinceStartup - start_unscaled_time;
            else
                return 0.0f;
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

