using System;
using System.Collections.Generic;

namespace Fast.TimeSliced
{
    class TimeSlicedTask
    {
        private int weight = 0;

        private bool running = false;
        private bool finished = false;

        private Callback on_finish = new Callback();

        public bool Running
        {
            get { return running; }
        }

        public bool Finished
        {
            get { return finished; }
        }

        public Callback OnFinish
        {
            get { return on_finish; }
        }

        public void Start()
        {
            if(!running)
            {
                running = true;

                OnStartInternal();
            }
        }

        public void Update()
        {
            if(running && !finished)
            {
                OnUpdateInternal();
            }
        }

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

        public virtual void OnStartInternal()
        {

        }

        public virtual void OnUpdateInternal()
        {

        }

        public virtual void OnFinishInternal()
        {

        }
    }
}
