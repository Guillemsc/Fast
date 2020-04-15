using System;
using System.Collections.Generic;

namespace Fast.UI
{
    public class AllSubFormsSetActive : Fast.UI.UIInstruction
    {
        private readonly bool active = false;

        public AllSubFormsSetActive(bool active)
        {
            this.active = active;
        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            for (int i = 0; i < context.Controller.CurrSubForms.Count; ++i)
            {
                Fast.UI.Form curr_form = context.Controller.CurrSubForms[i].Instance;

                if(curr_form.Parent == null)
                {
                    continue;
                }

                curr_form.Parent.SetActive(active);
            }

            Finish();
        }
    }
}
