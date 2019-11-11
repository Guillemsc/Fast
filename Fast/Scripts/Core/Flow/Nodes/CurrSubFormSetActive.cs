using DG.Tweening;
using System;

public static class CurrSubFormSetActiveExtensions
{
    public static Fast.Flow.FlowContainer FlowCurrSubFormSetActive(this Fast.Flow.FlowContainer container, bool active)
    {
        Fast.Flow.CurrSubFormSetActiveNode node = new Fast.Flow.CurrSubFormSetActiveNode(container, active);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class CurrSubFormSetActiveNode : FlowNode
    {
        private bool active = false;

        private UI.Form sub_form = null;

        public CurrSubFormSetActiveNode(FlowContainer container, bool active)
            : base(container)
        {
            this.active = active;
        }

        protected override void OnRunInternal()
        {
            sub_form = Container.Controller.FlowState.CurrSubForm;

            if (sub_form != null)
            {
                Sequence ret = DOTween.Sequence();

                ret.OnStart(delegate ()
                {
                    sub_form.Parent.SetActive(active);
                });

                ret.Play();
            }

            Finish();
        }
    }
}