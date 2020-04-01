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

        private readonly Fast.Callback<float> on_time_scale_changed = new Callback<float>();

        public float TimeScale
        {
            get { return time_scale; }
            set
            {
                bool value_changed = false;

                if(time_scale != value)
                {
                    value_changed = true;
                }

                time_scale = value;

                if(value_changed)
                {
                    on_time_scale_changed.Invoke(time_scale);
                }
            }
        }

        public float DeltaTime => delta_time;
        public float UnscaledDeltaTime => unscaled_delta_time;

        public float CurrentTime => current_time;
        public float UnscaledCurrentTime => unscaled_current_time;

        public Fast.Callback<float> OnTimeScaleChanged => on_time_scale_changed;

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
