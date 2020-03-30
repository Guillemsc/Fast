using System;

namespace Fast.Cinematics
{
    public class UpdatableAction : Action, NodeCanvas.Framework.IUpdatable
    {
        public void Update()
        {
            ActionUpdate();
        }

        protected virtual void ActionUpdate()
        {

        }
    }
}
