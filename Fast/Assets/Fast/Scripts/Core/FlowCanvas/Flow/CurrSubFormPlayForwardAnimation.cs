using System.Collections;
using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;

namespace Fast.FastFlowCanvas.Flow
{
    [Name("Curr Sub Form Play Forward Animation")]
    [Category("Fast/Flow")]
    [Description("CurrSubFormPlayForwardAnimation")]
    public class CurrSubFormPlayForwardAnimation : EventNode
    {
        private FlowInput enter;
        private FlowOutput exit;

        [RequiredField]
        [SerializeField] private ValueInput<string> animation_name = null;

        [RequiredField]
        [SerializeField] private ValueInput<bool> force_starting_values = null;

        protected override void RegisterPorts()
        {
            enter = AddFlowInput("Enter", Enter);
            exit = AddFlowOutput("Exit");

            animation_name = AddValueInput<string>("Animation name");
            force_starting_values = AddValueInput<bool>("Force starting values");
        }

        private void Enter(FlowCanvas.Flow flow)
        {
            FastInstance.Instance.MFlow.CurrFlowContainer.
                FlowCurrSubFormPlayForwardAnimation(animation_name.value, force_starting_values.value);

            exit.Call(flow);
        }
    }
}