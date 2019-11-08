using System.Collections;
using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;

namespace Fast.FastFlowCanvas.Flow
{
    [Name("Curr Sub Form Set Active")]
    [Category("Fast/Flow")]
    [Description("CurrSubFormSetActive")]
    public class CurrSubFormSetActive : EventNode
    {
        private FlowInput enter;
        private FlowOutput exit;

        [RequiredField]
        [SerializeField] private ValueInput<bool> active = null;

        protected override void RegisterPorts()
        {
            enter = AddFlowInput("Enter", Enter);
            exit = AddFlowOutput("Exit");

            active = AddValueInput<bool>("Active");
        }

        private void Enter(FlowCanvas.Flow flow)
        {
            FastInstance.Instance.MFlow.CurrFlowContainer.FlowCurrSubFormSetActive(active.value);

            exit.Call(flow);
        }
    }
}