﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Modules
{
    class TimeSlicedModule : UpdatableModule
    {
        private Fast.TimeSliced.TimeSlicedController time_sliced_controller 
            = new TimeSliced.TimeSlicedController();

        public override void Update()
        {
            time_sliced_controller.Update();
        }

        public void PushTask(Fast.TimeSliced.TimeSlicedTask task, int weight)
        {
            time_sliced_controller.PushTask(task, weight);
        }

        public void CancelTask(Fast.TimeSliced.TimeSlicedTask task)
        {
            time_sliced_controller.CancelTask(task);
        }
    }
}