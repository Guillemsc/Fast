using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.UI
{
    public abstract class UIInstruction
    {
        protected readonly UIController controller = null;

        private bool started = false;
        private bool finished = false;

        private Fast.Callback on_finish = new Callback();

        public UIInstruction(UIController controller)
        {
            this.controller = controller;
        }

        public bool Started => started;
        public bool Finished => finished;

        public Fast.Callback OnFinish => on_finish;

        public void Start()
        {
            if(!started)
            {
                started = true;

                StartInternal();
            }
        }

        public void Finish()
        {
            if(started && finished)
            {
                FinishInternal();

                on_finish.Invoke();
                on_finish.UnSubscribeAll();

                finished = true;
                started = false;
            }
        }

        protected virtual void StartInternal()
        {

        }

        protected virtual void FinishInternal()
        {

        }
    }
}
