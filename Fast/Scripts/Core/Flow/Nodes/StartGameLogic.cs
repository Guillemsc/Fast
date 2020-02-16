using System;
using DG.Tweening;

public static class StartGameLogicExtension
{
    public static Fast.Flow.FlowContainer FlowStartGameLogic(this Fast.Flow.FlowContainer container, Fast.Logic.GameLogic logic)
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
        private Fast.Logic.GameLogic logic = null;

        public StartGameLogic(FlowContainer container, Fast.Logic.GameLogic logic) : base(container)
        {
            this.logic = logic;
        }

        protected override void OnRunInternal()
        {
            if(logic != null)
            {
                Fast.FastInstance.Instance.MLogic.StartLogic(logic);

                Finish();
            }
        }
    }
}