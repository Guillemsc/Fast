using System;
using DG.Tweening;

public static class StartGameLogicExtension
{
    public static Fast.Flow.FlowContainer FlowStartGameLogic(this Fast.Flow.FlowContainer container, Fast.Architecture.GameLogic logic)
    {
        Fast.Flow.StartGameLogic node = new Fast.Flow.StartGameLogic(container, logic);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class StartGameLogic : FlowNode
    {
        private Fast.Architecture.GameLogic logic = null;

        public StartGameLogic(FlowContainer container, Fast.Architecture.GameLogic logic) : base(container)
        {
            this.logic = logic;
        }

        protected override void OnRunInternal()
        {
            if(logic != null)
            {
                Fast.FastService.MLogic.StartLogic(logic);

                Finish();
            }
        }
    }
}