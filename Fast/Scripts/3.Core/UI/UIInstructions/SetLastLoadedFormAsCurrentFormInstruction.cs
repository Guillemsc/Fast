using System;

namespace Fast.UI
{
    public class SetLastLoadedFormAsCurrentFormInstruction : Fast.UI.UIInstruction
    {
        protected override void StartInternal(UIBehaviourContext context)
        {
            context.Controller.CurrForm = context.LastLoadedForm;

            Finish();
        }
    }
}
