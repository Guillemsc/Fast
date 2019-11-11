using System.Collections;
using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;

namespace Fast.FastFlowCanvas.Flow
{
    [Name("All Sub Forms Set Active")]
    [Category("Fast/Flow")]
    [Description("AllSubFormsSetActive")]
    public class AllSubFormsSetActive : EventNode
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
            FastInstance.Instance.MFlow.CurrFlowContainer.FlowAllSubFormsSetActive(active.value);

            exit.Call(flow);
        }
    }
}