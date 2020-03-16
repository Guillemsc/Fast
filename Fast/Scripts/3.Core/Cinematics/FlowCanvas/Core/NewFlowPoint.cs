using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using System;

namespace Fast.Cinematics
{
    [Name("New Flow Point")]
    [Category("Fast/Cinematics")]
    [Description("Start timeline")]
    [Color("fc8c03")]
    public class NewFlowPoint : FlowControlNode
    {
        private FlowInput input_flow = null;

        protected override void RegisterPorts()
        {
            input_flow = AddFlowInput("Input", Input);

            AddFlowOutput("Output");

            AddValueOutput("FlowPoint", Get<NewFlowPoint>);
        }

        protected NewFlowPoint Get<T>()
        {
            return this;
        }

        private void Input(FlowCanvas.Flow fl)
        {

        }
    }
}
