using System.Collections;
using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;

namespace Fast.FastFlowCanvas.Flow
{
    [Name("Remove All Sub Forms")]
    [Category("Fast/Flow")]
    [Description("RemoveAllSubForms")]
    public class RemoveAllSubForms : EventNode
    {
        private FlowInput enter;
        private FlowOutput exit;

        protected override void RegisterPorts()
        {
            enter = AddFlowInput("Enter", Enter);
            exit = AddFlowOutput("Exit");
        }

        private void Enter(FlowCanvas.Flow flow)
        {
            FastInstance.Instance.MFlow.CurrFlowContainer.FlowRemoveAllSubForms();

            exit.Call(flow);
        }
    }
}