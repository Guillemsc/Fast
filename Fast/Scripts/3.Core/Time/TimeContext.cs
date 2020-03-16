using System;

namespace Fast.Time
{
    public class TimeContext
    {
        private float time_scale = 1.0f;

        private float delta_time = 0.0f;
        private float unscaled_delta_time = 0.0f;

        private float current_time = 0.0f;
        private float unscaled_current_time = 0.0f;

        public float TimeScale
        {
            get { return time_scale; }
            set { time_scale = value; }
        }

        public float DeltaTime => delta_time;
        public float UnscaledDeltaTime => unscaled_delta_time;

        public float CurrentTime => current_time;
        public float UnscaledCurrentTime => unscaled_current_time;

        public void Tick(float delta_time)
        {
            this.delta_time = delta_time * time_scale;
            this.unscaled_delta_time = delta_time;

            current_time += this.delta_time;
            unscaled_current_time += this.unscaled_delta_time;
        }

        public Timer GetTimer()
        {
            return new Timer(this);
        }
    }
}
