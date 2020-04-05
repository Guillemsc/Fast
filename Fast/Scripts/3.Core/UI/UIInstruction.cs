using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.UI
{
    public class UIInstruction
    {
        private bool started = false;
        private bool finished = false;

        private Fast.Callback on_finish = new Callback();

        public bool Started => started;
        public bool Finished => finished;
        public Fast.Callback OnFinish => on_finish;

        public void Start(Fast.UI.UIBehaviourContext context)
        {
            if(!started)
            {
                started = true;

                StartInternal(context);
            }
        }

        public void Finish()
        {
            if(started)
            {
                finished = true;
                started = false;

                FinishInternal();

                on_finish.Invoke();
            }
        }

        protected virtual void StartInternal(Fast.UI.UIBehaviourContext context)
        {

        }

        protected virtual void FinishInternal()
        {

        }
    }
}
