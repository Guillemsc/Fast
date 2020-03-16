using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using System;

namespace Fast.Cinematics
{
    [Name("New Timeline")]
    [Category("Fast/Cinematics")]
    [Description("Start timeline")]
    [Color("0384fc")]
    public class NewTimeline : FlowControlNode
    {
        protected override void RegisterPorts()
        {
            AddFlowOutput("Output");
        }
    }
}
