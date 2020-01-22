using System;
using System.Diagnostics;
using UnityEngine;

namespace Fast.Networking
{
    public class Timer
    {
        private Stopwatch stopwatch = new Stopwatch();
        bool started = false;

        public void Start()
        {
            Reset();

            stopwatch.Start();

            started = true;
        }

        public void Reset()
        {
            started = false;

            stopwatch = new Stopwatch();
        }

        public float ReadTime()
        {
            float ret = 0.0f;

            if (started)
            {
                TimeSpan ts = stopwatch.Elapsed;

                ret = (float)ts.TotalSeconds;
            }

            return ret;
        }

        public bool Started
        {
            get { return started; }
        }
    }
}

