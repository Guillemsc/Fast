using System;
using System.Collections.Generic;

namespace Fast.UI
{
    public class ClearAllSubForms : Fast.UI.UIInstruction
    {
        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            context.Controller.CurrSubForm = null;
            context.Controller.CurrSubForms.Clear();

            Finish();
        }
    }
}
