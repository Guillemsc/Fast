using System;
using System.Threading.Tasks;
using System.Timers;

namespace Fast
{
    class CallbackTimer
    {
        private System.Timers.Timer timer = null;

        private Callback<CallbackTimer> on_elapsed = new Callback<CallbackTimer>();

        public void Start(float time, bool auto_reset, Action<CallbackTimer> on_tick)
        {
            time = time * 1000.0f; // seconds to ms

            timer = new System.Timers.Timer(time);

            on_elapsed.UnSubscribeAll();
            on_elapsed.Subscribe(on_tick);

            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = auto_reset;

            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            on_elapsed.Invoke(this);
        }
    }
}
