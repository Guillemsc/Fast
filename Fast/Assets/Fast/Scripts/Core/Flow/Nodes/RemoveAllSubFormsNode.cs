using System;

public static class RemoveAllSubFormsExtensions
{
    public static Fast.Flow.FlowContainer FlowRemoveAllSubForms(this Fast.Flow.FlowContainer container)
    {
        Fast.Flow.RemoveAllSubFormsNode node = new Fast.Flow.RemoveAllSubFormsNode(container);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class RemoveAllSubFormsNode : FlowNode
    {
        public RemoveAllSubFormsNode(FlowContainer container)
            : base(container)
        {

        }

        protected override void OnRunInternal()
        {
            Container.Controller.FlowState.ClearCurrSubForms();

            Finish();
        }
    }
}