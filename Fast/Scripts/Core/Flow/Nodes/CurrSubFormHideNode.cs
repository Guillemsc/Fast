using System;

public static class CurrSubFormHideExtensions
{
    public static Fast.Flow.FlowContainer FlowCurrSubFormHide(this Fast.Flow.FlowContainer container)
    {
        Fast.Flow.CurrSubFormHideNode node = new Fast.Flow.CurrSubFormHideNode(container);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class CurrSubFormHideNode : FlowNode
    {
        public CurrSubFormHideNode(FlowContainer container)
            : base(container)
        {

        }

        protected override void OnRunInternal()
        {
            if (Container.Controller.FlowState.CurrSubForm != null)
            {
                Container.Controller.FlowState.CurrSubForm.Hide();
            }

            Finish();
        }
    }
}