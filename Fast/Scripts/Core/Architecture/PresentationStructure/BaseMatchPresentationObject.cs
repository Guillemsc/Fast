using System;
using UnityEngine;

namespace Fast.Presentation
{
    public class BaseMatchPresentationObject : MonoBehaviour
    {
        private Logic.BaseMatchLogicObject logic_object = null;

        protected Logic.BaseMatchLogicObject LogicObject
        {
            get { return logic_object; }
            set { logic_object = value; }
        }
    }
}
