using System;

public static class CurrSubFormShowExtensions
{
    public static Fast.Flow.FlowContainer FlowCurrSubFormShow(this Fast.Flow.FlowContainer container)
    {
        Fast.Flow.CurrSubFormShowNode node = new Fast.Flow.CurrSubFormShowNode(container);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class CurrSubFormShowNode : FlowNode
    {
        public CurrSubFormShowNode(FlowContainer container)
            : base(container)
        {

        }

        protected override void OnRunInternal()
        {
            if (Container.Controller.FlowState.CurrSubForm != null)
            {
                Container.Controller.FlowState.CurrSubForm.Show();
            }

            Finish();
        }
    }
}