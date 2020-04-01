using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ShowFormInstructionExtension
{
    public static void InstructionShowForm(this Fast.UI.UIBehaviour behaviour,
        string form_to_hide_animation, Fast.UI.Form form_to_show, string form_to_show_animation)
    {
        Fast.UI.Instructions.ShowFormInstruction instruction = new Fast.UI.Instructions.ShowFormInstruction(
            behaviour.Controller, form_to_hide_animation, form_to_show, form_to_show_animation);

        behaviour.AddInstruction(instruction);
    }

    public static void InstructionShowForm(this Fast.UI.UIBehaviour behaviour,
       Fast.UI.Form form_to_show, string form_to_show_animation)
    {
        Fast.UI.Instructions.ShowFormInstruction instruction = new Fast.UI.Instructions.ShowFormInstruction(
            behaviour.Controller, "", form_to_show, form_to_show_animation);

        behaviour.AddInstruction(instruction);
    }
}

namespace Fast.UI.Instructions
{
    public class ShowFormInstruction : Fast.UI.UIInstruction
    {
        private readonly string form_to_hide_animation = "";
        private readonly Form form_to_show = null;
        private readonly string form_to_show_animation = "";

        public ShowFormInstruction(UIController controller, string form_to_hide_animation, Form form_to_show, 
            string form_to_show_animation)
            : base(controller)
        {
            this.form_to_hide_animation = form_to_hide_animation;
            this.form_to_show = form_to_show;
            this.form_to_show_animation = form_to_show_animation;
        }

        protected override void StartInternal()
        {
            StartHideForm();
        }

        private void StartHideForm()
        {
            Form curr_form = controller.CurrForm;

            if (curr_form != null)
            {
                PlayFormAnimationInstruction instruction =
                    new PlayFormAnimationInstruction(controller, curr_form, form_to_hide_animation, false, FormAnimationDirection.BACKWARD);

                instruction.OnFinish.Subscribe(delegate()
                {
                    curr_form.Parent.SetActive(false);

                    StartShowForm();
                });

                instruction.Start();
            }
            else
            {
                StartShowForm();
            }
        }

        private void StartShowForm()
        {
            controller.CurrForm = form_to_show;

            if (form_to_show != null)
            {
                PlayFormAnimationInstruction instruction =
                    new PlayFormAnimationInstruction(controller, form_to_show, form_to_show_animation, true, FormAnimationDirection.FORWARD);

                instruction.OnFinish.Subscribe(Finish);

                instruction.Start();

                form_to_show.Parent.SetActive(true);
            }
            else
            {
                Finish();
            }
        }
    }
}
