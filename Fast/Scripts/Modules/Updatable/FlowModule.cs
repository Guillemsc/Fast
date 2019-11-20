using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Modules
{
    class FlowModule : UpdatableModule
    {
        private Fast.Flow.FlowController flow_controller = new Flow.FlowController();

        public override void Update()
        {
            flow_controller.Update();
        }

        public void RunContainer(int identifier_id, Action on_finish = null)
        {
            Flow.FlowContainer container = null;

            bool could_run = flow_controller.RunContainer(identifier_id, out container);

            if(could_run)
            {
                container.OnFinish.UnSubscribeAll();
                container.OnFinish.Subscribe(on_finish);
            }
        }

        public void PushRunContainer(int identifier_id, Action on_start = null, Action on_finish = null)
        {
            Flow.FlowContainer container = null;

            bool could_run = flow_controller.PushRunContainer(identifier_id, out container);

            if (could_run)
            {
                container.OnStart.UnSubscribeAll();
                container.OnStart.Subscribe(on_start);

                container.OnFinish.UnSubscribeAll();
                container.OnFinish.Subscribe(on_finish);
            }
        }

        public Flow.FlowContainer CreateContainer(int identifier_id)
        {
            return flow_controller.CreateContainer(identifier_id);
        }
    }
}
