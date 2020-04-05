using System;
using System.Collections.Generic;

namespace Fast.UI
{
    public class CurrFormPlayFormAnimInstruction : Fast.UI.UIInstruction
    {
        private readonly string animation_name = "";
        private readonly Fast.UI.FormAnimationDirection direction = FormAnimationDirection.FORWARD;

        public CurrFormPlayFormAnimInstruction(string animation_name, Fast.UI.FormAnimationDirection direction)
        {
            this.animation_name = animation_name;
            this.direction = direction;
        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            if (context.Controller.CurrForm == null)
            {
                Finish();
            }

            Fast.UI.Form form = context.Controller.CurrForm.Instance;

            FormAnimation form_animation = form.GetAnimation(animation_name);

            if (form_animation == null)
            {
                Finish();
            }

            form_animation.ForceStartingValues = true;

            form_animation.OnFinish.Subscribe(Finish);

            switch (direction)
            {
                case FormAnimationDirection.FORWARD:
                    {
                        form_animation.AnimateForward(Fast.FastService.MTime.GeneralTimeContext);
                        break;
                    }

                case FormAnimationDirection.BACKWARD:
                    {
                        form_animation.AnimateBackward(Fast.FastService.MTime.GeneralTimeContext);
                        break;
                    }
            }

            form.Parent.SetActive(true);
        }

        protected override void FinishInternal()
        {

        }
    }
}
