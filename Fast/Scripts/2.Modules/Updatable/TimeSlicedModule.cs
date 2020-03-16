using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Modules
{
    public class TimeSlicedModule : UpdatableModule
    {
        private readonly TimeSliced.TimeSlicedController time_sliced_controller = null;

        public TimeSlicedModule()
        {
            Fast.Time.Timer timer_to_use = FastService.MTime.GeneralTimeContext.GetTimer();
            time_sliced_controller = new TimeSliced.TimeSlicedController(timer_to_use);
        }

        public override void Update()
        {
            time_sliced_controller.Update();
        }

        public TimeSpan MaxTimePerFrame
        {
            set { time_sliced_controller.MaxTimePerFrame = value; }
        }

        public void PushTask(Fast.TimeSliced.TimeSlicedTask task, int priority)
        {
            Contract.IsNotNull(task);

            time_sliced_controller.PushTask(task, priority);
        }

        public void CancelTask(Fast.TimeSliced.TimeSlicedTask task)
        {
            Contract.IsNotNull(task);

            time_sliced_controller.CancelTask(task);
        }
    }
}
