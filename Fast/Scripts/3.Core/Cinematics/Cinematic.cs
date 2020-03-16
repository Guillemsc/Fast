using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Cinematics
{
    public class Cinematic
    {
        private readonly List<CinematicTimeline> timelines = new List<CinematicTimeline>();

        private bool started = false;
        private bool finished = false;

        public bool Started => started;
        public bool Finished => finished;

        public Cinematic(List<CinematicTimeline> timelines)
        {
            this.timelines = timelines;
        }

        public void Start()
        {
            if (!started)
            {
                started = true;
                finished = false;

                for (int i = 0; i < timelines.Count; ++i)
                {
                    CinematicTimeline curr_timeline = timelines[i];

                    curr_timeline.Start();
                }
            }
        }

        public void Update()
        {
            if (!finished)
            {
                int finished_timelines = 0;

                for (int i = 0; i < timelines.Count; ++i)
                {
                    CinematicTimeline curr_timeline = timelines[i];

                    curr_timeline.Update();

                    if (curr_timeline.Finished)
                    {
                        ++finished_timelines;
                    }
                }

                if (finished_timelines == timelines.Count)
                {
                    started = false;
                    finished = true;
                }
            }
        }
    }
}
