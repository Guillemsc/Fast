using System;
using DG.Tweening;

public static class StopGameLogicExtension
{
    public static Fast.Flow.FlowContainer FlowStopGameLogic(this Fast.Flow.FlowContainer container, Fast.Architecture.GameLogic logic)
    {
        Fast.Flow.StopGameLogic node = new Fast.Flow.StopGameLogic(container, logic);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class StopGameLogic : FlowNode
    {
        private Fast.Architecture.GameLogic logic = null;

        public StopGameLogic(FlowContainer container, Fast.Architecture.GameLogic logic) : base(container)
        {
            this.logic = logic;
        }

        protected override void OnRunInternal()
        {
            if (logic != null)
            {
                Fast.FastService.MLogic.StopLogic(logic);

                Finish();
            }
        }
    }
}