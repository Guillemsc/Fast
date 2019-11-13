﻿using System;
using System.Threading.Tasks;
using System.Timers;

namespace Fast.Networking
{
    class Timer
    {
        private System.Timers.Timer timer = null;

        private Callback<Timer> on_elapsed = new Callback<Timer>();

        public void Start(float time, bool auto_reset, Action<Timer> on_tick = null)
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