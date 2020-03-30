using System;

namespace Fast.Modules
{
    public class TimeModule : UpdatableModule
    {
        private readonly Fast.Time.TimeController time_controller = new Time.TimeController(); 

        private Fast.Time.TimeContext general_time_context = null;

        public override void Awake()
        {
            general_time_context = CreateTimeContext();

            general_time_context.TimeScale = 0.5f;
        }

        public Fast.Time.TimeContext GeneralTimeContext => general_time_context;

        public override void Update()
        {
            time_controller.Update();
        }

        public Fast.Time.TimeContext CreateTimeContext()
        {
            return time_controller.CreateTimeContext();
        }
    }
}
