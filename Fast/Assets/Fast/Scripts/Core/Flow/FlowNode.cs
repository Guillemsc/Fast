using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Flow
{
    public enum FastUIEngineNodeDirection
    {
        FORWARD,
        BACKWARD,
    }

    public class FlowNode
    {
        private FlowContainer container = null;

        private bool start_with_last = false;
        private bool start_at_end_of_last = false;

        private List<FlowNode> pushed_at_end = new List<FlowNode>();

        private bool running = false;
        private bool finished = false;

        private Callback on_finish = new Callback();

        public FlowNode(FlowContainer container)
        {
            this.container = container;
        }

        public FlowContainer Container
        {
            get { return container; }
        }

        public bool StartWithLast
        {
            get { return start_with_last; }
            set { start_with_last = value; }
        }

        public bool StartAtEndOfLast
        {
            get { return start_at_end_of_last; }
            set { start_at_end_of_last = value; }
        }

        public bool Running
        {
            get { return running; }
        }

        public bool Finished
        {
            get { return finished; }
        }

        public void AddToPushedAtEnd(FlowNode node)
        {
            pushed_at_end.Add(node);
        }

        public List<FlowNode> PushedAtEnd
        {
            get { return pushed_at_end; }
        }

        public void Run()
        {
            if (!running)
            {
                running = true;

                OnRunInternal();
                OnPostRunInternal();
            }
        }

        public void Finish()
        {
            if (running)
            {
                OnFinishInternal();

                on_finish.Invoke();

                running = false;
                finished = true;
            }
        }

        public Callback OnFinish
        {
            get { return on_finish; }
        }

        protected virtual void OnRunInternal()
        {

        }

        protected virtual void OnPostRunInternal()
        {

        }

        protected virtual void OnFinishInternal()
        {

        }
    }
}
