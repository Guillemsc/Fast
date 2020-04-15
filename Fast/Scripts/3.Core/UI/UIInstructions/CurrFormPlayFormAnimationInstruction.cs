using System;
using System.Collections.Generic;

namespace Fast.UI
{
    public class CurrFormPlayFormAnimationInstruction : Fast.UI.UIInstruction
    {
        private readonly string animation_name = "";
        private readonly Fast.UI.FormAnimationDirection direction = FormAnimationDirection.FORWARD;
        private readonly bool force_starting_values = false;

        public CurrFormPlayFormAnimationInstruction(string animation_name, Fast.UI.FormAnimationDirection direction,
            bool force_starting_values)
        {
            this.animation_name = animation_name;
            this.direction = direction;
            this.force_starting_values = force_starting_values;
        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            if (context.Controller.CurrForm == null)
            {
                Finish();

                return;
            }

            Fast.UI.Form form = context.Controller.CurrForm.Instance;

            FormAnimation form_animation = form.GetAnimation(animation_name);

            if (form_animation == null)
            {
                Finish();
            }

            form_animation.ForceStartingValues = force_starting_values;

            form_animation.OnFinish.Subscribe(Finish);

            switch (direction)
            {
                case FormAnimationDirection.FORWARD:
                    {
                        form_animation.AnimateForward(context.Controller.TimeContext);
                        break;
                    }

                case FormAnimationDirection.BACKWARD:
                    {
                        form_animation.AnimateBackward(context.Controller.TimeContext);
                        break;
                    }
            }
        }
    }
}
