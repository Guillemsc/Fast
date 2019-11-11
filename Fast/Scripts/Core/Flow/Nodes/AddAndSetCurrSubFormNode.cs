using System;

public static class AddAndSetCurrSubFormExtensions
{
    public static Fast.Flow.FlowContainer FlowAddAndSetCurrSubForm(this Fast.Flow.FlowContainer container, Fast.UI.Form sub_form)
    {
        Fast.Flow.AddAndSetCurrSubFormNode node = new Fast.Flow.AddAndSetCurrSubFormNode(container, sub_form);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class AddAndSetCurrSubFormNode : FlowNode
    {
        private UI.Form sub_form = null;

        public AddAndSetCurrSubFormNode(FlowContainer container, UI.Form sub_form)
            : base(container)
        {
            this.sub_form = sub_form;
        }

        protected override void OnRunInternal()
        {
            Container.Controller.FlowState.AddAndSetCurrSubForm(sub_form);

            Finish();
        }
    }
}
