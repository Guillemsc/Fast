using System;
using System.Collections.Generic;

namespace Fast.UI
{
    public class SetCurrSubFormAnimationStartingValues : Fast.UI.UIInstruction
    {
        private readonly string animation_name = "";
        private readonly Fast.UI.FormAnimationDirection direction = FormAnimationDirection.FORWARD;

        public SetCurrSubFormAnimationStartingValues(string animation_name, Fast.UI.FormAnimationDirection direction)
        {
            this.animation_name = animation_name;
            this.direction = direction;
        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            if (context.Controller.CurrSubForm == null)
            {
                Finish();
            }

            Fast.UI.Form form = context.Controller.CurrSubForm.Instance;

            FormAnimation form_animation = form.GetAnimation(animation_name);

            if (form_animation == null)
            {
                Finish();

                return;
            }

            switch (direction)
            {
                case FormAnimationDirection.FORWARD:
                    {
                        form_animation.SetStartingValuesForward();

                        break;
                    }

                case FormAnimationDirection.BACKWARD:
                    {
                        form_animation.SetStartingValuesBackward();
                        break;
                    }
            }

            Finish();
        }
    }
}
