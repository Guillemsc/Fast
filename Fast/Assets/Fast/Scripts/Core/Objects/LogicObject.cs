using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fast
{
    public class LogicObject : MonoBehaviour
    {
        private int object_uid = 0;

        public LogicObject(int object_uid)
        {
            this.object_uid = object_uid;
        }

        public int ObjectUID
        {
            get { return object_uid; }
        }
    }
}
