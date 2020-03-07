using System;
using DG.Tweening;

public static class AddWaitTimeExtension
{
    public static Fast.Flow.FlowContainer FlowWaitTime(this Fast.Flow.FlowContainer container, float time)
    {
        Fast.Flow.WaitTime node = new Fast.Flow.WaitTime(container, time);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class WaitTime : FlowNode
    {
        private float wait_time;

        public WaitTime(FlowContainer container, float time) : base(container)
        {
            wait_time = time;
        }

        protected override void OnRunInternal()
        {
            Sequence seq = DOTween.Sequence();
            seq.SetDelay(wait_time);
            seq.OnComplete(Finish);
            seq.Play();
        }
    }
}