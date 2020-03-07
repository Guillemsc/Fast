using System;

public static class CurrFormShowExtensions
{
    public static Fast.Flow.FlowContainer FlowCurrFormShow(this Fast.Flow.FlowContainer container)
    {
        Fast.Flow.CurrFormShowNode node = new Fast.Flow.CurrFormShowNode(container);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class CurrFormShowNode : FlowNode
    {
        public CurrFormShowNode(FlowContainer container) 
            : base(container)
        {
 
        }

        protected override void OnRunInternal()
        {
            if(Container.Controller.FlowState.CurrForm != null)
            {
                Container.Controller.FlowState.CurrForm.Show();
            }

            Finish();
        }
    }
}
