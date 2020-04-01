using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class WaitInstructionExtension
{
    public static void InstructionWait(this Fast.UI.UIBehaviour behaviour, float seconds)
    {
        Fast.UI.Instructions.WaitInstruction instruction = new Fast.UI.Instructions.WaitInstruction(
            behaviour.Controller, seconds);

        behaviour.AddInstruction(instruction);
    }
}

namespace Fast.UI.Instructions
{
    public class WaitInstruction : UIInstruction
    {
        private readonly float seconds = 0.0f;

        private Sequence sequence = null;

        public WaitInstruction(UIController controller, float seconds)
            : base(controller)
        {
            this.seconds = seconds;
        }

        protected override void StartInternal()
        {
            controller.timeContext.OnTimeScaleChanged.Subscribe(OnTimeScaleChanged);

            sequence = DOTween.Sequence();

            sequence.AppendInterval(seconds);

            sequence.timeScale = controller.timeContext.TimeScale;

            sequence.OnComplete(Finish);

            sequence.Play();
        }

        protected override void FinishInternal()
        {
            controller.timeContext.OnTimeScaleChanged.UnSubscribe(OnTimeScaleChanged);
        }

        private void OnTimeScaleChanged(float time_scale)
        {
            if(sequence != null)
            {
                sequence.timeScale = time_scale;
            }
        }
    }
}
