using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;

namespace Fast.Cinematics
{
    public class Action : Finishable
    {
        private FlowInput main_flow_in = null;

        protected CinematicAsset CinematicAsset => (CinematicAsset)base.flowGraph;

        protected override void RegisterPorts()
        {
            main_flow_in = AddFlowInput("Action flow", OnFlowInput);

            ActionRegisterPorts();
        }

        private void OnFlowInput(FlowCanvas.Flow flow)
        {
            Reset();

            ActionStart(flow);
        }

        protected override void FinishableFinished(bool complete)
        {
            ActionFinished(complete);
        }

        protected virtual void ActionRegisterPorts()
        {

        }

        protected virtual void ActionStart(FlowCanvas.Flow flow)
        {

        }

        protected virtual void ActionFinished(bool complete)
        {

        }
    }
}