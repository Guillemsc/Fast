using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Logic
{
    class LogicAction
    {
        private bool started = false;
        private bool finished = false;

        private Callback on_start = new Callback();
        private Callback on_finish = new Callback();

        public bool Started
        {
            get { return started; }
        }

        public bool Finished
        {
            get { return finished; }
        }

        public void Start()
        {
            if(!started)
            {
                started = true;
                finished = false;

                on_start.Invoke();

                OnStartInternal();
            }
        }

        public void Finish()
        {
            if (started && !finished)
            {
                OnFinishInternal();

                on_finish.Invoke();

                started = false;
                finished = true;
            }
        }

        public Callback OnStart
        {
            get { return on_start; }
        }

        public Callback OnFinish
        {
            get { return on_finish; }
        }

        protected virtual void OnStartInternal()
        {

        }

        protected virtual void OnFinishInternal()
        {

        }
    }
}
