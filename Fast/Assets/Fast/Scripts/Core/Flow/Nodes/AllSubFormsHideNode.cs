using System;

public static class AllSubFormsHideExtensions
{
    public static Fast.Flow.FlowContainer FlowAllSubFormsHide(this Fast.Flow.FlowContainer container)
    {
        Fast.Flow.AllSubFormsHideNode node = new Fast.Flow.AllSubFormsHideNode(container);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class AllSubFormsHideNode : FlowNode
    {
        public AllSubFormsHideNode(FlowContainer container)
            : base(container)
        {

        }

        protected override void OnRunInternal()
        {
            for (int i = 0; i < Container.Controller.FlowState.CurrSubForms.Count; ++i)
            {
                Fast.UI.Form curr_form = Container.Controller.FlowState.CurrSubForms[i];

                curr_form.Hide();
            }

            Finish();
        }
    }
}
