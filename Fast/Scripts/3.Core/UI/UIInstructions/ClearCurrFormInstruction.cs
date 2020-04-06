using System;
using System.Collections.Generic;

namespace Fast.UI
{
    public class ClearCurrFormInstruction : Fast.UI.UIInstruction
    {
        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            context.Controller.CurrForm = null;

            Finish();
        }
    }
}
