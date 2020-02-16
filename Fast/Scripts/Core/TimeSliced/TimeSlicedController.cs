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

        public void Update()
        {
            UpdateTasks();
        }

        public TimeSlicedTask PushTask(TimeSlicedTask task, int weight = 0)
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
                tasks_queue.Add(task, weight);
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
            if(tasks_queue.Count > 0)
            {
                TimeSlicedTask curr_task = tasks_queue.At(0);

                if(!curr_task.Running)
                {
                    curr_task.Start();
                }

                for (int i = 0; i < curr_task.MaxIterationsPerFrame; ++i)
                {
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

                        break;
                    }
                }
            }
        }
    }
}
