using System;
using System.Collections.Generic;

namespace Fast.UI
{
    public class CurrSubFormSetActive : Fast.UI.UIInstruction
    {
        private readonly bool active = false;

        public CurrSubFormSetActive(bool active)
        {
            this.active = active;
        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            if (context.Controller.CurrSubForm == null)
            {
                Finish();
            }

            Fast.UI.Form form = context.Controller.CurrSubForm.Instance;

            if (form.Parent == null)
            {
                Finish();

                return;
            }

            form.Parent.SetActive(active);

            Finish();
        }
    }
}
