﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Fast
{
    public class LogicObjectData<T>
    {
        private int object_type = 0;
        private int object_uid = 0;

        public LogicObjectData(int object_type, int object_uid)
        {
            this.object_type = object_type;
            this.object_uid = object_uid;
        }

        public int ObjectType
        {
            get { return object_type; }
        }

        public int ObjectUID
        {
            get { return object_uid; }
        }

        public virtual T Clone()
        {
            return default(T);
        }
    }
}
