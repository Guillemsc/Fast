using FlowCanvas;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Cinematics
{
    [Name("DebugTimeline")]
    [Category("Fast/Cinematics/Debug")]
    [Description("Start timeline")]
    [Color("0384fc")]
    public class DebugTimeline : Timeline
    { 
        private FlowInput main_flow_in = null;

        protected override void TimelineRegisterPorts()
        {
            main_flow_in = AddFlowInput("MainFlow", OnFlowInput);
        }

        private void OnFlowInput(FlowCanvas.Flow flow)
        {
            base.StartFlow();
        }
    }
}
