using System;
using System.Collections.Generic;

namespace Fast.TimeSliced
{
    /// <summary>
    /// [For inheritance] Used for ocasions when a taks must be splited into different frames.
    /// </summary>
    public class TimeSlicedTask
    {
        private int weight = 0;

        private bool running = false;
        private bool finished = false;

        private Callback on_finish = new Callback();

        /// <summary>
        /// Returns if the task has started and it's running.
        /// </summary>
        public bool Running
        {
            get { return running; }
        }

        /// <summary>
        /// Returns if the task has started and has finished.
        /// </summary>
        public bool Finished
        {
            get { return finished; }
        }

        /// <summary>
        /// Invoked when the task finishes.
        /// </summary>
        public Callback OnFinish
        {
            get { return on_finish; }
        }

        /// <summary>
        /// [Internal, don't use] Sets the task as started and calls OnStartInternal().
        /// </summary>
        public void Start()
        {
            if(!running)
            {
                running = true;

                OnStartInternal();
            }
        }

        /// <summary>
        /// [Internal, don't use] Updates the task by calling OnUpdateInternal().
        /// </summary>
        public void Update()
        {
            if(running && !finished)
            {
                OnUpdateInternal();
            }
        }

        /// <summary>
        /// Function that needs to be called to mark the end of the task. Calls OnFinishInternal().
        /// </summary>
        public void Finish()
        {
            if(running && !finished)
            {
                OnFinishInternal();

                on_finish.Invoke();
                on_finish.UnSubscribeAll();

                running = false;
                finished = true;
            }
        }

        /// <summary>
        /// Called when the task is started.
        /// </summary>
        public virtual void OnStartInternal()
        {

        }

        /// <summary>
        /// Called once the task is starded and needs to update the progress.
        /// </summary>
        public virtual void OnUpdateInternal()
        {

        }

        /// <summary>
        /// Called when the task is finished.
        /// </summary>
        public virtual void OnFinishInternal()
        {

        }
    }
}
