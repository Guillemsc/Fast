using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fast
{
    [System.Serializable]
    public class LogicObject : MonoBehaviour
    {
        protected int object_type = 0;
        protected int object_type_variation = 0;
        protected int object_uid = 0;
        
        public LogicObject(int object_type)
        {
            this.object_type = object_type;
        }

        public void SetObjectData(int object_uid, int object_type_variation)
        {
            this.object_uid = object_uid;
            this.object_type_variation = object_type_variation;
        }

        public int ObjectType
        {
            get { return object_type; }
        }

        public int ObjectTypeVariation
        {
            get { return object_type_variation; }
        }

        public int ObjectUID
        {
            get { return object_uid; }
        }
    }
}
