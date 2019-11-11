using System.Collections;
using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;

namespace Fast.FastFlowCanvas.Flow
{
    [Name("Curr Form Play Backward Animation")]
    [Category("Fast/Flow")]
    [Description("CurrFormPlayBackwardAnimation")]
    public class CurrFormPlayBackwardAnimation : EventNode
    {
        private FlowInput enter;
        private FlowOutput exit;

        private FlowOutput exit_animation_finished;

        [RequiredField]
        [SerializeField] private ValueInput<string> animation_name = null;

        [RequiredField]
        [SerializeField] private ValueInput<bool> force_starting_values = null;

        private Fast.Callback callback = null;

        protected override void RegisterPorts()
        {
            enter = AddFlowInput("Enter", Enter);
            exit = AddFlowOutput("Exit");

            animation_name = AddValueInput<string>("Animation name");
            force_starting_values = AddValueInput<bool>("Force starting values");

            exit_animation_finished = AddFlowOutput("Exit On Animation Finished");
        }

        private void Enter(FlowCanvas.Flow flow)
        {
            FastInstance.Instance.MFlow.CurrFlowContainer.
                FlowCurrFormPlayBackwardAnimation(animation_name.value, force_starting_values.value);

            callback = FastInstance.Instance.MFlow.CurrFlowContainer.LastNode.OnFinish;

            callback.Subscribe(delegate ()
            {
                exit_animation_finished.Call(flow);
            });

            exit.Call(flow);
        }
    }
}