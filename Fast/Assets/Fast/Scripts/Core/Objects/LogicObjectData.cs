using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Fast
{
    public class LogicObjectData 
    {
        private Type object_type = null;
        private int object_uid = 0;

        public LogicObjectData(Type object_type)
        {
            this.object_type = object_type;
            this.object_uid = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }

        public Type ObjectType
        {
            get { return object_type; }
        }

        public int ObjectUID
        {
            get { return object_uid; }
        }

        protected virtual void Serialize()
        {

        }

        protected virtual void DeSerialize()
        {

        }
    }
}
