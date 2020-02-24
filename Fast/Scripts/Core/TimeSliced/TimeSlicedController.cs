using System;
using System.Collections.Generic;

namespace Fast.TimeSliced
{
    /// <summary>
    /// Controller of the time sliced tasks.
    /// </summary>
    class TimeSlicedController
    {
        private Fast.Containers.PriorityQueue<TimeSlicedTask> tasks_queue
            = new Fast.Containers.PriorityQueue<TimeSlicedTask>();

        private float max_time_ms_per_frame = 2.0f;
        private Fast.Timer frame_timer = new Timer();

        public void Update()
        {
            UpdateTasks();
        }

        public float MaxTimeMsPerFrame
        {
            get { return max_time_ms_per_frame; }
            set { max_time_ms_per_frame = value; }
        }

        public TimeSlicedTask PushTask(TimeSlicedTask task, int priority = 0)
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

            return task;
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
            frame_timer.Start();

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

                if(frame_timer.ReadTimeMs() > max_time_ms_per_frame)
                {
                    finish = true;
                }
            }
        }
    }
}
