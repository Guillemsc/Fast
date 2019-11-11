using System.Collections;
using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;

namespace Fast.FastFlowCanvas.Flow
{
    [Name("Curr Form Set Active")]
    [Category("Fast/Flow")]
    [Description("CurrFormSetActive")]
    public class CurrFormSetActive : EventNode
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
            FastInstance.Instance.MFlow.CurrFlowContainer.FlowCurrFormSetActive(active.value);

            exit.Call(flow);
        }
    }
}