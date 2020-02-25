using System;

namespace Fast.Logic
{
    public class BaseMatchLogicObject
    {
        protected int object_uid = 0;

        protected int object_category = 0;
        protected int object_category_variation = 0;

        protected void Init(int object_uid, int object_category, int object_category_variation = 0)
        {
            this.object_uid = object_uid;
            this.object_category = object_category;
            this.object_category_variation = object_category_variation;
        }

        public int ObjectUID
        {
            get { return object_uid; }
        }
    }
}
