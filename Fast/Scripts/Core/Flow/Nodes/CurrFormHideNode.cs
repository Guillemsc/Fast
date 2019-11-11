using System;

public static class CurrFormHideExtensions
{
    public static Fast.Flow.FlowContainer FlowCurrFormHide(this Fast.Flow.FlowContainer container)
    {
        Fast.Flow.CurrFormHideNode node = new Fast.Flow.CurrFormHideNode(container);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class CurrFormHideNode : FlowNode
    {
        public CurrFormHideNode(FlowContainer container)
            : base(container)
        {

        }

        protected override void OnRunInternal()
        {
            if (Container.Controller.FlowState.CurrForm != null)
            {
                Container.Controller.FlowState.CurrForm.Hide();
            }

            Finish();
        }
    }
}