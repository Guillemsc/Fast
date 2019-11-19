using System;
using DG.Tweening;

public static class AddWaitTimeExtension
{
    public static Fast.Flow.FlowContainer FlowAddWaitTime(this Fast.Flow.FlowContainer container, float time)
    {
        Fast.Flow.AddWaitTimeNode node = new Fast.Flow.AddWaitTimeNode(container, time);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class AddWaitTimeNode : FlowNode
    {
        private float wait_time;

        public AddWaitTimeNode(FlowContainer container, float time) : base(container)
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