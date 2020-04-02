using System;
using System.Collections.Generic;

namespace Fast.FlowCommands
{
    public abstract class FlowCommand
    {
        private readonly string category = "";
        private readonly string name = "";

        private bool started = false;
        private bool finished = false;

        private readonly Fast.Callback on_finish = new Callback();

        public FlowCommand(string category, string name)
        {
            this.category = category;
            this.name = name;
        }

        public string Category => category;
        public string Name => name;

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

        public virtual IReadOnlyList<FlowCommand> PreAddCommands()
        {
            return new List<FlowCommand>();
        }

        protected virtual void ExecuteInternal()
        {

        }

        public virtual IReadOnlyList<FlowCommand> PostAddCommands()
        {
            return new List<FlowCommand>();
        }

        public virtual string LogCommand()
        {
            return "";
        }
    }
}
