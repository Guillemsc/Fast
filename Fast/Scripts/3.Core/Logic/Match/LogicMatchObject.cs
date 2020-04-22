using System;

namespace Fast.Logic.Match
{
    public abstract class LogicMatchObject
    {
        private readonly int object_uid = 0;

        public LogicMatchObject(int object_uid)
        {
            this.object_uid = object_uid;
        }

        public int ObjectUID => object_uid;
    }
}
