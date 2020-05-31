using System;
using UnityEngine;

namespace Fast.Logic.View
{
    public class LogicObjectView : MonoBehaviour
    {
        private int object_uid = 0;


        public int ObjectUID => object_uid;

        public void Init(int object_uid)
        {
            this.object_uid = object_uid;
        }
    }
}
