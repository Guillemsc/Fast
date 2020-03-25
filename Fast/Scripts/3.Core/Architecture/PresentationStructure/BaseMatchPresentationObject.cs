using System;
using UnityEngine;

namespace Fast.Architecture
{
    public class BaseMatchPresentationObject : MonoBehaviour
    {
        private bool inited = false;
        private BaseMatchLogicObject logic_object = null;

        public void Init(BaseMatchLogicObject logic_object)
        {
            if(inited)
            {
                return;
            }

            inited = true;

            this.logic_object = logic_object;
        }

        public bool Inited => inited;

        public BaseMatchLogicObject BaseMatchLogicObject => logic_object;
    }
}
