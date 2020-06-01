using System;
using System.Collections.Generic;

namespace Fast.Time
{
    public class Timer
    {
        private readonly TimeContext context = null;

        private bool started = false;

        private float start_time = 0.0f;
        private float unscaled_start_time = 0.0f;

        public bool Started => started;
        public float DeltaTime => context.DeltaTime;
        public float UnscaledDeltaTime => context.UnscaledDeltaTime;

        public Timer(TimeContext context)
        {
            Contract.IsNotNull(context);

            this.context = context;
        }

        public void Start()
        {
            started = true;

            start_time = context.CurrentTime;
            unscaled_start_time = context.UnscaledCurrentTime;
        }

        public void Reset()
        {
            started = false;

            start_time = 0.0f;
            unscaled_start_time = 0.0f;
        }

        public void Restart()
        {
            Reset();
            Start();
        }

        public TimeSpan ReadTime()
        {
            float passed_time = context.CurrentTime - start_time;

            return TimeSpan.FromSeconds(passed_time);
        }

        public TimeSpan ReadUnscaledTime()
        {
            float passed_time = context.UnscaledCurrentTime - unscaled_start_time;

            return TimeSpan.FromSeconds(passed_time);
        }

        public bool HasReached(float seconds)
        {
            bool ret = false;

            if(ReadTime().TotalSeconds > seconds)
            {
                ret = true;
            }

            return ret;
        }

        public bool UnscaledHasReached(float seconds)
        {
            bool ret = false;

            if (ReadUnscaledTime().TotalSeconds > seconds)
            {
                ret = true;
            }

            return ret;
        }
    }
}
