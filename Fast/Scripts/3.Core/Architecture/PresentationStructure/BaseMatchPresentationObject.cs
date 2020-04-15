using System;
using UnityEngine;

namespace Fast.Architecture
{
    public class MatchPresentationObject : MonoBehaviour
    {
        private bool inited = false;
        private MatchLogicObject logic_object = null;

        public void Init(MatchLogicObject logic_object)
        {
            if(inited)
            {
                return;
            }

            inited = true;

            this.logic_object = logic_object;
        }

        public bool Inited => inited;

        public MatchLogicObject MatchLogicObject => logic_object;
    }
}
