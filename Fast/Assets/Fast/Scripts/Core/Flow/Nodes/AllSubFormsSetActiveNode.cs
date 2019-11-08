using DG.Tweening;
using System;
using System.Collections.Generic;

public static class AllSubFormsSetActiveExtensions
{
    public static Fast.Flow.FlowContainer FlowAllSubFormsSetActive(this Fast.Flow.FlowContainer container, bool active)
    {
        Fast.Flow.AllSubFormsSetActiveNode node = new Fast.Flow.AllSubFormsSetActiveNode(container, active);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class AllSubFormsSetActiveNode : FlowNode
    {
        private bool active = false;

        private List<UI.Form> sub_forms = new List<UI.Form>();

        public AllSubFormsSetActiveNode(FlowContainer container, bool active)
            : base(container)
        {
            this.active = active;
        }

        protected override void OnRunInternal()
        {
            sub_forms = new List<UI.Form>(Container.Controller.FlowState.CurrSubForms);
           
            Sequence ret = DOTween.Sequence();
            
            ret.OnStart(delegate ()
            {
                for (int i = 0; i < sub_forms.Count; ++i)
                {
                    Fast.UI.Form curr_form = sub_forms[i];

                    curr_form.Parent.SetActive(active);
                }
            });
            
            ret.Play();
           
            Finish();
        }
    }
}