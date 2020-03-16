using System;
using System.Collections.Generic;

namespace Fast.Time
{
    public class TimeController : Fast.IController, Fast.IUpdatable
    {
        private List<TimeContext> time_contexts = new List<TimeContext>();

        public TimeContext CreateTimeContext()
        {
            TimeContext ret = new TimeContext();

            time_contexts.Add(ret);

            return ret;
        }

        public void Update()
        {
            float dt = UnityEngine.Time.unscaledDeltaTime;

            for (int i = 0; i < time_contexts.Count; ++i)
            {
                TimeContext curr_time_context = time_contexts[i];

                curr_time_context.Tick(dt);
            }
        }
    }
}
