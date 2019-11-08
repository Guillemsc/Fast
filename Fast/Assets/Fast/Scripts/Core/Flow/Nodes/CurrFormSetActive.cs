using DG.Tweening;
using System;

public static class CurrFormSetActiveExtensions
{
    public static Fast.Flow.FlowContainer FlowCurrFormSetActive(this Fast.Flow.FlowContainer container, bool active)
    {
        Fast.Flow.CurrFormSetActiveNode node = new Fast.Flow.CurrFormSetActiveNode(container, active);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class CurrFormSetActiveNode : FlowNode
    {
        private bool active = false;

        private Fast.UI.Form form = null;

        public CurrFormSetActiveNode(FlowContainer container, bool active)
            : base(container)
        {
            this.active = active;
        }

        protected override void OnRunInternal()
        {
            form = Container.Controller.FlowState.CurrForm;

            if (form != null)
            {
                Sequence ret = DOTween.Sequence();

                ret.OnStart(delegate ()
                {
                    form.Parent.SetActive(active);
                });

                ret.Play();
            }

            Finish();
        }
    }
}