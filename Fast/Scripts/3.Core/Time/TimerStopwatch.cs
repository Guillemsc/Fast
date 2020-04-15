using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Time
{
    public class TimerStopwatch
    {
        private readonly Stopwatch stopwatch = new Stopwatch();

        private bool started = false;

        public void Start()
        {
            stopwatch.Start();
        }

        public void Reset()
        {
            stopwatch.Reset();
        }

        public void Restart()
        {
            stopwatch.Restart();
        }

        public TimeSpan ReadTime()
        {
            return stopwatch.Elapsed;
        }
    }
}
