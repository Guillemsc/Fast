using System;
using UnityEngine;

namespace Fast.Logic.View
{
    public class LogicSubView : MonoBehaviour
    {
        private LogicView logic_view = null;

        public LogicView LogicView => logic_view;

        public void SetLogicParent(LogicView logic_view)
        {
            this.logic_view = logic_view;
        }

        public virtual void ReceiveEffect(Commands.ILogicCommandEffect effect)
        {

        }
    }
}
