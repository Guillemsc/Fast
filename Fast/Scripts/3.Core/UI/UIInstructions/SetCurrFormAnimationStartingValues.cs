using System;
using System.Collections.Generic;

namespace Fast.UI
{
    public class SetCurrFormAnimationStartingValues : Fast.UI.UIInstruction
    {
        private readonly string animation_name = "";
        private readonly Fast.UI.FormAnimationDirection direction = FormAnimationDirection.FORWARD;

        public SetCurrFormAnimationStartingValues(string animation_name, Fast.UI.FormAnimationDirection direction)
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
