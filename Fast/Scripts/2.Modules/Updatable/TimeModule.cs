using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Modules
{
    public class TimeModule : UpdatableModule
    {
        private readonly Fast.Time.TimeController time_controller = null;

        private readonly Fast.Time.TimeContext general_time_context = null;

        public TimeModule()
        {
            time_controller = new Time.TimeController(); 

            general_time_context = CreateTimeContext();
        }

        public override void Update()
        {
            time_controller.Update();
        }

        public Fast.Time.TimeContext CreateTimeContext()
        {
            return time_controller.CreateTimeContext();
        }

        public Fast.Time.TimeContext GeneralTimeContext => general_time_context;
    }
}
