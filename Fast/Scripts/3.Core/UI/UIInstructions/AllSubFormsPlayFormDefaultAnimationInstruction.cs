using System;
using System.Collections.Generic;

namespace Fast.UI
{
    public class AllSubFormsPlayFormDefaultAnimationInstruction : Fast.UI.UIInstruction
    {
        private readonly Fast.UI.FormAnimationDirection direction = FormAnimationDirection.FORWARD;
        private readonly bool force_starting_values = false;

        private int waiting_animations_count = 0;
        private int waiting_animations_finished = 0;

        public AllSubFormsPlayFormDefaultAnimationInstruction(Fast.UI.FormAnimationDirection direction,
            bool force_starting_values)
        {
            this.direction = direction;
            this.force_starting_values = force_starting_values;
        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            for(int i = 0; i < context.Controller.CurrSubForms.Count; ++i)
            {
                Fast.UI.Form curr_form = context.Controller.CurrSubForms[i].Instance;

                FormAnimation default_anim = curr_form.DefaultAnimation;

                if(default_anim == null)
                {
                    continue;
                }

                ++waiting_animations_count;

                default_anim.ForceStartingValues = force_starting_values;

                default_anim.OnFinish.Subscribe(OnAnimationFinished);

                switch (direction)
                {
                    case FormAnimationDirection.FORWARD:
                        {
                            default_anim.AnimateForward(context.Controller.TimeContext);
                            break;
                        }

                    case FormAnimationDirection.BACKWARD:
                        {
                            default_anim.AnimateBackward(context.Controller.TimeContext);
                            break;
                        }
                }
            }

            if(waiting_animations_count == 0)
            {
                Finish();
            }
        }

        private void OnAnimationFinished()
        {
            ++waiting_animations_finished;

            if(waiting_animations_finished == waiting_animations_count)
            {
                Finish();
            }
        }
    }
}
