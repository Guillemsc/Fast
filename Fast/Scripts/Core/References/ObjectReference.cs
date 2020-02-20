using System;
using UnityEngine;

namespace Fast.References
{
    public class ObjectReference<T>
    {
        private ObjectReferenceMaster<T> master = null;

        private Fast.Callback<ObjectReference<T>> on_invalidated = new Callback<References.ObjectReference<T>>();

        public ObjectReference(ObjectReferenceMaster<T> master)
        {
            this.master = master;
        }

        public void Remove()
        {
            master.RemoveReference(this);
        }

        public bool Valid
        {
            get { return master.Valid; }
        }

        public T Reference
        {
            get { return master.Reference; }
        }

        public Fast.Callback<ObjectReference<T>> OnInvalidated
        {
            get { return on_invalidated; }
        }
    }
}
