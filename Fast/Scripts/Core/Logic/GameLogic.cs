using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Logic
{
    class GameLogic
    {
        private bool running = false;
        private bool finished = false;

        private Callback on_start = new Callback();
        private Callback on_finish = new Callback();

        public bool Running
        {
            get { return running; }
        }

        public bool Finished
        {
            get { return finished; }
        }

        public void Start()
        {
            if (!running)
            {
                finished = false;
                running = true;

                on_start.Invoke();

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
            if (running && !finished)
            {
                OnFinishInternal();

                on_finish.Invoke();

                running = false;
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

        protected virtual void OnUpdateInternal()
        {

        }

        protected virtual void OnFinishInternal()
        {

        }
    }
}
