using System;
using System.Collections.Generic;

namespace Fast.UI
{
    public class CurrFormSetActive : Fast.UI.UIInstruction
    {
        private readonly bool active = false;

        public CurrFormSetActive(bool active)
        {
            this.active = active;
        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            if (context.Controller.CurrForm == null)
            {
                Finish();
            }

            Fast.UI.Form form = context.Controller.CurrForm.Instance;

            if(form.Parent == null)
            {
                Finish();

                return;
            }

            form.Parent.SetActive(active);

            Finish();
        }
    }
}
