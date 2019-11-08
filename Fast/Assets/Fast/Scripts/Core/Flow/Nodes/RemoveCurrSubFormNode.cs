using System;

public static class RemoveCurrSubFormExtensions
{
    public static Fast.Flow.FlowContainer FlowRemoveCurrSubForm(this Fast.Flow.FlowContainer container)
    {
        Fast.Flow.RemoveCurrSubFormNode node = new Fast.Flow.RemoveCurrSubFormNode(container);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class RemoveCurrSubFormNode : FlowNode
    {
        public RemoveCurrSubFormNode(FlowContainer container)
            : base(container)
        {
        }

        protected override void OnRunInternal()
        {
            Container.Controller.FlowState.RemoveCurrSubForm(Container.Controller.FlowState.CurrSubForm);

            Finish();
        }
    }
}
