using System;

public static class SetCurrFormExtensions
{
    public static Fast.Flow.FlowContainer FlowSetCurrForm(this Fast.Flow.FlowContainer container, Fast.UI.Form form)
    {
        Fast.Flow.SetCurrFormNode node = new Fast.Flow.SetCurrFormNode(container, form);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class SetCurrFormNode : FlowNode
    {
        private UI.Form form = null;

        public SetCurrFormNode(FlowContainer container, UI.Form form)
            : base(container)
        {
            this.form = form;
        }

        protected override void OnRunInternal()
        {
            if (Container.Controller.FlowState.CurrForm != form)
            {
                Container.Controller.FlowState.LastCurrForm = Container.Controller.FlowState.CurrForm;

                Container.Controller.FlowState.CurrForm = form;
            }

            Finish();
        }
    }
}
