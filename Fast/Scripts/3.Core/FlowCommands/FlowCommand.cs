using System;
using System.Collections.Generic;

namespace Fast.FlowCommands
{
    public abstract class FlowCommand
    {
        private bool started = false;
        private bool finished = false;

        private readonly Fast.Callback on_finish = new Callback();

        public bool Started => started;
        public bool Finished => finished;

        public Fast.Callback OnFinish => on_finish;

        public void Execute()
        {
            if(!started)
            {
                started = true;
                finished = false;

                ExecuteInternal();
            }
        }

        protected void Finish()
        {
            if (started && !finished)
            {
                finished = true;

                on_finish.Invoke();
            }
        }

        protected virtual void ExecuteInternal()
        {

        }

        public virtual string LogCommand()
        {
            return "";
        }
    }
}
