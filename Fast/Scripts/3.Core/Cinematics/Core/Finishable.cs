using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Cinematics
{
    public class Finishable : CinematicsNode
    {
        private bool finished = false;

        private Fast.Callback on_finish = new Callback();

        protected override void RegisterPorts()
        {
            throw new NotImplementedException();
        }

        public bool Finished => finished;
        public Fast.Callback OnFinish => on_finish;

        protected void Reset()
        {
            finished = false;
        }

        public void Finish(bool complete = true)
        {
            if(finished)
            {
                return;
            }

            finished = true;

            FinishableFinished(complete);

            on_finish.Invoke();
        }

        protected virtual void FinishableFinished(bool complete = true)
        {

        }
    }
}
