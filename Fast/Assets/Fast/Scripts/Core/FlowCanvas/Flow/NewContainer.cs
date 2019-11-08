using System.Collections;
using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;

namespace Fast.FastFlowCanvas.Flow
{
    [Name("New Container")]
    [Category("Fast/Flow")]
    [Description("NewContainer")]
    public class NewContainer : EventNode
    {
        private FlowInput enter;
        private FlowOutput exit;

        [RequiredField]
        [SerializeField] private ValueInput<int> identifier_id = null;

        protected override void RegisterPorts()
        {
            enter = AddFlowInput("Enter", Enter);
            exit = AddFlowOutput("Exit");

            identifier_id = AddValueInput<int>("Identifier id");
        }

        private void Enter(FlowCanvas.Flow flow)
        {
            int id = identifier_id.value;

            FastInstance.Instance.MFlow.CurrFlowContainer =
                FastInstance.Instance.MFlow.FlowController.CreateContainer(id);

            exit.Call(flow);
        }
    }
}