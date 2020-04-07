using System;

namespace Fast.UI
{
    public class SetLastLoadedFormAsCurrentSubFormInstruction : Fast.UI.UIInstruction
    {
        protected override void StartInternal(UIBehaviourContext context)
        {
            if(context.LastLoadedForm == null)
            {
                Finish();

                return;
            }

            context.Controller.CurrSubForm = context.LastLoadedForm;

            context.Controller.CurrSubForms.Remove(context.LastLoadedForm);
            context.Controller.CurrSubForms.Add(context.LastLoadedForm);

            Finish();
        }
    }
}
