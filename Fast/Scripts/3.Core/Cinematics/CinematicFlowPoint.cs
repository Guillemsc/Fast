using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Cinematics
{ 
    public class CinematicFlowPoint
    {
        private readonly List<CinematicAction> actions = new List<CinematicAction>();

        private bool started = false;
        private bool finished = false;

        public bool Started => started;
        public bool Finished => finished;

        public CinematicFlowPoint(List<CinematicAction> actions)
        {
            this.actions = actions;
        }

        public void Start()
        {
            if (!started)
            {
                started = true;
                finished = false;

                for (int i = 0; i < actions.Count; ++i)
                {
                    CinematicAction curr_action = actions[i];

                    curr_action.Start();
                }
            }
        }

        public void Update()
        {
            if (started && !finished)
            {
                int finished_actions = 0;

                for (int i = 0; i < actions.Count; ++i)
                {
                    CinematicAction curr_action = actions[i];

                    if (!curr_action.Finished)
                    {
                        curr_action.Update();
                    }
                    else
                    {
                        ++finished_actions;
                    }
                }

                if (finished_actions == actions.Count)
                {
                    started = false;
                    finished = true;
                }
            }
        }
    }
}
