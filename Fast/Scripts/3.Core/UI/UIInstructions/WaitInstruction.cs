using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.UI
{
    public class WaitInstruction : Fast.UI.UIInstruction
    {
        private readonly float time_seconds = 0.0f;

        private Sequence seq = null;

        public WaitInstruction(float time_seconds)
        {
            this.time_seconds = time_seconds;
        }

        protected override void StartInternal(UIBehaviourContext context)
        {
            context.Controller.TimeContext.OnTimeScaleChanged.Subscribe(OnTimeScaleChanged);

            seq = DOTween.Sequence();

            seq.AppendInterval(time_seconds);

            seq.OnComplete(Finish);

            seq.Play();
        }

        protected override void FinishInternal(Fast.UI.UIBehaviourContext context)
        {
            context.Controller.TimeContext.OnTimeScaleChanged.UnSubscribe(OnTimeScaleChanged);
        }

        private void OnTimeScaleChanged(float time_scale)
        {
            if(seq == null)
            {
                return;
            }

            seq.timeScale = time_scale;
        }
    }
}
