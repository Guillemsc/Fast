using System;

namespace Fast.Logic
{
    public class BaseMatchLogicObject
    {
        private int object_uid = 0;

        private int object_category = 0;
        private int object_category_variation = 0;

        public BaseMatchLogicObject(int object_uid, int object_category)
        {
            this.object_category = object_category;
        }

        protected int ObjectUID
        {
            get { return object_uid; }
        }

        protected int ObjectCategory
        {
            get { return object_category; }
        }

        protected int ObjectCategoryVariation
        {
            get { return object_category_variation; }
            set { object_category_variation = value; }
        }
    }
}
