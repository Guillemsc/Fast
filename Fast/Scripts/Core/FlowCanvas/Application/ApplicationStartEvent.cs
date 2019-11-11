using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.FastFlowCanvas.Application
{
    [Name("On Application Start")]
    [Category("Fast/Application")]
    [Description("Called when the Application starts")]
    public class ApplicationStartEvent : EventNode
    {
        private FlowOutput start;

        protected override void RegisterPorts()
        {
            start = AddFlowOutput("Start");
        }

        public override void OnGraphStarted()
        {
            start.Call(new FlowCanvas.Flow());
        }
    }
}
