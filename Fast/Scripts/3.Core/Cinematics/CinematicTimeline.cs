using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Cinematics
{
    public class CinematicTimeline
    {
        private readonly List<CinematicFlowPoint> flow_points = new List<CinematicFlowPoint>();

        private Queue<CinematicFlowPoint> flow_points_to_run = new Queue<CinematicFlowPoint>();

        private bool finished = false;

        public bool Finished => finished;

        public CinematicTimeline(List<CinematicFlowPoint> flow_points)
        {
            this.flow_points = flow_points;
        }

        public void Start()
        {
            finished = false;

            for (int i = 0; i < flow_points.Count; ++i)
            {
                flow_points_to_run.Enqueue(flow_points[i]);
            }
        }

        public void Update()
        {
            if(flow_points_to_run.Count > 0)
            {
                CinematicFlowPoint curr_flow_point = flow_points_to_run.Peek();

                if(!curr_flow_point.Started)
                {
                    curr_flow_point.Start();
                }

                if(!curr_flow_point.Finished)
                {
                    curr_flow_point.Update();
                }
                else
                {
                    flow_points_to_run.Dequeue();
                }
            }
            else
            {
                finished = true;
            }
        }
    }
}
