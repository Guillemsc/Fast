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

        private UIBehaviourContext behaviour_context = null;

        public bool Started => started;
        public bool Finished => finished;
        public Fast.Callback OnFinish => on_finish;

        public void Start(Fast.UI.UIBehaviourContext context)
        {
            if(!started)
            {
                started = true;

                this.behaviour_context = context;

                StartInternal(context);
            }
        }

        public void Finish()
        {
            if(started)
            {
                finished = true;
                started = false;

                FinishInternal(behaviour_context);

                on_finish.Invoke();
            }
        }

        protected virtual void StartInternal(Fast.UI.UIBehaviourContext context)
        {

        }

        protected virtual void FinishInternal(Fast.UI.UIBehaviourContext context)
        {
            
        }
    }
}
