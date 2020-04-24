using System;
using UnityEngine;

namespace Fast.Logic.Presentation
{
    public class LogicPresentationObject : MonoBehaviour
    {
        private int object_uid = 0;

        public void Init(int object_uid)
        {
            this.object_uid = object_uid;
        }

        public int ObjectUID => object_uid;
    }
}
