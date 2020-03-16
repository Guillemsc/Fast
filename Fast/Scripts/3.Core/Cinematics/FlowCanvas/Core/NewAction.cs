using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using System;

namespace Fast.Cinematics
{
    public class NewAction : FlowControlNode
    {
        protected ValueInput<NewFlowPoint> input_flow_point = null;

        protected override void RegisterPorts()
        {
            input_flow_point = AddValueInput<NewFlowPoint>("FlowPoint");

            RegisterCustomPorts();
        }

        protected virtual void RegisterCustomPorts()
        {

        }

        public virtual CinematicAction GetCinematicAction()
        {
            return null;
        }
    }
}
