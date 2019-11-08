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
        private Fast.Flow.FlowContainer curr_flow_container = null;

        public override void Update()
        {
            flow_controller.Update();
        }

        public Fast.Flow.FlowController FlowController
        {
            get { return flow_controller; }
        }

        public Fast.Flow.FlowContainer CurrFlowContainer
        {
            get { return curr_flow_container; }
            set { curr_flow_container = value; }
        }
    }
}
