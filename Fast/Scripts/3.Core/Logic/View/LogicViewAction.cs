using System;

namespace Fast.Logic.View
{
    public abstract class LogicViewAction
    {
        private bool started = false;
        private bool finished = false;

        public bool Started => started;
        public bool Finished => finished;

        public void Start()
        {
            if(!started)
            {
                started = true;

                StartInternal();
            }
        }

        public void Update()
        {
            if(started && !finished)
            {
                UpdateInternal();
            }
        }

        public void Finish()
        {
            if(started && !finished)
            {
                finished = true;

                FinishInternal();
            }
        }

        protected virtual void StartInternal()
        {

        }

        protected virtual void UpdateInternal()
        {

        }

        protected virtual void FinishInternal()
        {

        }
    }
}
