using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PlayFormAnimationInstructionExtension
{
    public static void InstructionPlayFormAnimation(this Fast.UI.UIBehaviour behaviour, 
        Fast.UI.Form form, string animation_name, bool force_starting_values, Fast.UI.FormAnimationDirection dir)
    {
        Fast.UI.Instructions.PlayFormAnimationInstruction instruction = new Fast.UI.Instructions.PlayFormAnimationInstruction(
            behaviour.Controller, form, animation_name, force_starting_values, dir);

        behaviour.AddInstruction(instruction);
    }
}

namespace Fast.UI.Instructions
{
    public class PlayFormAnimationInstruction : UIInstruction
    {
        private readonly Form form = null;
        private readonly string animation_name = "";
        private readonly bool force_starting_values = false;
        private readonly FormAnimationDirection dir = FormAnimationDirection.FORWARD;

        public PlayFormAnimationInstruction(UIController controller, Form form, string animation_name, 
            bool force_starting_values, FormAnimationDirection dir) 
            : base(controller)
        {
            this.form = form;
            this.animation_name = animation_name;
            this.force_starting_values = force_starting_values;
            this.dir = dir;
        }

        protected override void StartInternal()
        {
            if(form == null)
            {
                // Log error
                Finish();
            }

            FormAnimation animation = form.GetAnimation(animation_name);

            if(animation == null)
            {
                // Log error
                Finish();

                return;
            }

            animation.ForceStartingValues = force_starting_values;

            animation.OnFinish.UnSubscribeAll();
            animation.OnFinish.Subscribe(Finish);

            switch (dir)
            {
                case FormAnimationDirection.FORWARD:
                    {
                        animation.AnimateForward(controller.timeContext);

                        break;
                    }

                case FormAnimationDirection.BACKWARD:
                    {
                        animation.AnimateBackward(controller.timeContext);

                        break;
                    }
            }
        }
    }
}
