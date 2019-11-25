using System;

public static class InvokeCallbackExtensions
{
    public static Fast.Flow.FlowContainer FlowInvokeCallback(this Fast.Flow.FlowContainer container, Action callback)
    {
        Fast.Flow.InvokeCallbackNode node = new Fast.Flow.InvokeCallbackNode(container, callback);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class InvokeCallbackNode : FlowNode
    {
        private Action callback = null;

        public InvokeCallbackNode(FlowContainer container, Action callback)
            : base(container)
        {
            this.callback = callback;
        }

        protected override void OnRunInternal()
        {
            if (callback != null)
                callback.Invoke();

            Finish();
        }
    }
}
