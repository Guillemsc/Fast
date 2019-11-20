using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Fast
{
    public class LogicObjectData<T>
    {
        private int object_uid = 0;

        public LogicObjectData(int object_uid)
        {
            this.object_uid = object_uid;
        }

        public int ObjectUID
        {
            get { return object_uid; }
        }

        protected virtual T Clone()
        {
            return default(T);
        }
    }
}
