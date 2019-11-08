using System.Collections;
using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;

namespace Fast.FastFlowCanvas.Flow
{
    [Name("Add And Set Curr Sub Form")]
    [Category("Fast/Flow")]
    [Description("AddAndSetCurrSubForm")]
    public class AddAndSetCurrSubForm : EventNode
    {
        private FlowInput enter;
        private FlowOutput exit;

        [RequiredField]
        [SerializeField] private ValueInput<Fast.UI.Form> form = null;

        protected override void RegisterPorts()
        {
            enter = AddFlowInput("Enter", Enter);
            exit = AddFlowOutput("Exit");

            form = AddValueInput<Fast.UI.Form>("Form");
        }

        private void Enter(FlowCanvas.Flow flow)
        {
            FastInstance.Instance.MFlow.CurrFlowContainer.FlowAddAndSetCurrSubForm(form.value);

            exit.Call(flow);
        }
    }
}