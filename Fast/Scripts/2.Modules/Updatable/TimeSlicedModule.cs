using System;

namespace Fast.Modules
{
    public class TimeSlicedModule : UpdatableModule
    {
        private readonly TimeSliced.TimeSlicedController time_sliced_controller = new TimeSliced.TimeSlicedController();

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
