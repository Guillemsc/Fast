using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Modules
{
    class FlowModule : UpdatableModule
    {
        private List<Fast.Flow.FlowController> flow_controller = new List<Flow.FlowController>();

        public Fast.Flow.FlowController CreateController()
        {
            Fast.Flow.FlowController ret = new Flow.FlowController();

            flow_controller.Add(ret);

            return ret;
        }

        public override void Update()
        {
            for (int i = 0; i < flow_controller.Count; ++i)
            {
                Fast.Flow.FlowController curr_controller = flow_controller[i];

                curr_controller.Update();
            }
        }

        public void RunContainer(Fast.Flow.FlowController controller, int identifier_id, Action on_finish = null)
        {
            Flow.FlowContainer container = null;

            if (controller != null)
            {
                bool could_run = controller.RunContainer(identifier_id, out container);

                if (could_run)
                {
                    container.OnFinish.UnSubscribeAll();
                    container.OnFinish.Subscribe(on_finish);
                }
            }
        }

        public void PushRunContainer(Fast.Flow.FlowController controller, int identifier_id, Action on_start = null, Action on_finish = null)
        {
            Flow.FlowContainer container = null;

            if (controller != null)
            {
                bool could_run = controller.PushRunContainer(identifier_id, out container);

                if (could_run)
                {
                    container.OnStart.UnSubscribeAll();
                    container.OnStart.Subscribe(on_start);

                    container.OnFinish.UnSubscribeAll();
                    container.OnFinish.Subscribe(on_finish);
                }
            }
        }

        public Flow.FlowContainer CreateContainer(Fast.Flow.FlowController controller, int identifier_id)
        {
            return controller.CreateContainer(identifier_id);
        }
    }
}
