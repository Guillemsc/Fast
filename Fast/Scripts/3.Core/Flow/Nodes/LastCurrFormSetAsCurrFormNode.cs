using System;

public static class LastCurrFormSetAsCurrFormExtensions
{
    public static Fast.Flow.FlowContainer FlowLastCurrFormSetAsCurrForm(this Fast.Flow.FlowContainer container)
    {
        Fast.Flow.LastCurrFormSetAsCurrFormNode node = new Fast.Flow.LastCurrFormSetAsCurrFormNode(container);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class LastCurrFormSetAsCurrFormNode : FlowNode
    {
        public LastCurrFormSetAsCurrFormNode(FlowContainer container)
            : base(container)
        {
   
        }

        protected override void OnRunInternal()
        {
            UI.Form curr_form = Container.Controller.FlowState.CurrForm;

            Container.Controller.FlowState.CurrForm = Container.Controller.FlowState.LastCurrForm;

            Container.Controller.FlowState.LastCurrForm = curr_form;

            Finish();
        }
    }
}