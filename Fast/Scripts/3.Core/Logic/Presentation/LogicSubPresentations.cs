using System;
using UnityEngine;

namespace Fast.Logic.Presentation
{
    public class LogicSubPresentation : MonoBehaviour
    {
        private LogicPresentation logic_presentation = null;

        public LogicPresentation LogicPresentation => logic_presentation;

        public void SetLogicParent(LogicPresentation logic_presentation)
        {
            this.logic_presentation = logic_presentation;
        }

        public virtual void ReceiveEffect(Commands.ILogicCommandEffect effect)
        {

        }
    }
}
