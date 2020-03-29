using FlowCanvas;
using FlowCanvas.Nodes;
using System;

namespace Fast.Cinematics
{
    public class CinematicsNode : FlowControlNode
    {
        private Fast.Bindings.BindingData binding_data = null;

        protected Fast.Bindings.BindingData BindingData => binding_data;

        protected override void RegisterPorts()
        {
            throw new NotImplementedException();
        }

        public void CinematicsCall(CinematicsNode calling, FlowOutput output, FlowCanvas.Flow flow)
        {
            CinematicsCall(calling, output, this.binding_data, flow);
        }

        public static void CinematicsCall(CinematicsNode calling, FlowOutput output, Fast.Bindings.BindingData binding_data, FlowCanvas.Flow flow)
        {
            if (calling == null)
            {
                return;
            }

            if (output == null)
            {
                return;
            }

            calling.binding_data = binding_data;

            output.Call(flow);
        }
    }
}
