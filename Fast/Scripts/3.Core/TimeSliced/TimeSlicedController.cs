using System;
using System.Collections.Generic;

namespace Fast.TimeSliced
{
    /// <summary>
    /// Controller of the time sliced tasks.
    /// </summary>
    class TimeSlicedController : IController, IUpdatable
    {
        private readonly Fast.Time.TimerStopwatch timer = new Time.TimerStopwatch();

        private Fast.Containers.PriorityQueue<TimeSlicedTask> tasks_queue
            = new Fast.Containers.PriorityQueue<TimeSlicedTask>();

        private TimeSpan max_time_per_frame = TimeSpan.FromMilliseconds(2);

        public void Update()
        {
            UpdateTasks();
        }

        public TimeSpan MaxTimePerFrame
        {
            get { return max_time_per_frame; }
            set { max_time_per_frame = value; }
        }

        public void PushTask(TimeSlicedTask task, int priority = 0)
        {
            if (task != null)
            {
                bool already_added = false;

                for (int i = 0; i < tasks_queue.Count; ++i)
                {
                    if (tasks_queue.At(i) == task)
                    {
                        already_added = true;

                        break;
                    }
                }

                if (!already_added)
                {
                    tasks_queue.Add(task, priority);
                }
            }
        }

        public void CancelTask(TimeSlicedTask task)
        {
            for (int i = 0; i < tasks_queue.Count; ++i)
            {
                if (tasks_queue.At(i) == task)
                {
                    tasks_queue.RemoveAt(i);

                    break;
                }
            }
        }

        private void UpdateTasks()
        {
            timer.Restart();

            bool finish = false;

            while (!finish)
            {
                if (tasks_queue.Count > 0)
                {
                    TimeSlicedTask curr_task = tasks_queue.At(0);

                    if (!curr_task.Running)
                    {
                        curr_task.Start();
                    }

                    curr_task.Update();

                    if (curr_task.Finished)
                    {
                        for (int y = 0; y < tasks_queue.Count; ++y)
                        {
                            if (tasks_queue.At(y) == curr_task)
                            {
                                tasks_queue.RemoveAt(y);

                                break;
                            }
                        }
                    }
                }
                else
                {
                    finish = true;
                }

                if(timer.ReadTime().Milliseconds > max_time_per_frame.Milliseconds)
                {
                    finish = true;
                }
            }
        }
    }
}
