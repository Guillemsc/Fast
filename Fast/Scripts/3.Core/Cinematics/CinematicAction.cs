using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Cinematics
{
    public class CinematicAction
    {
        private bool started = false;

        private bool finished = false;

        public bool Finished => finished;

        public void Start()
        {
            if(!started)
            {
                finished = false;
                started = true;

                OnStartInternal();
            }
        }

        public void Update()
        {
            if(started && !finished)
            {
                OnUpdateInternal();
            }
        }

        public void Finish()
        {
            if(started && !finished)
            {
                OnFinishInternal();

                finished = true;
                started = false;
            }
        }

        protected virtual void OnStartInternal()
        {
            Finish();
        }

        protected virtual void OnUpdateInternal()
        {

        }

        protected virtual void OnFinishInternal()
        {

        }
    }
}
