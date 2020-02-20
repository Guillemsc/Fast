using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.References
{
    public class ObjectReferenceMaster<T>
    {
        private T referenced_object = default(T);

        private bool valid = true;

        private List<ObjectReference<T>> references = new List<ObjectReference<T>>();

        public ObjectReferenceMaster(T referenced_object)
        {
            this.referenced_object = referenced_object;
        }

        public ObjectReference<T> GenerateReference()
        {
            ObjectReference<T> ret = new ObjectReference<T>(this);

            references.Add(ret);

            return ret;
        }

        public void RemoveReference(ObjectReference<T> reference)
        {
            for (int i = 0; i < references.Count; ++i)
            {
                ObjectReference<T> curr_reference = references[i];

                if(curr_reference == reference)
                {
                    references.RemoveAt(i);

                    break;
                }
            }
        }

        public void InvalidateReferences()
        {
            List<ObjectReference<T>> to_invoke = new List<ObjectReference<T>>(references);

            for (int i = 0; i < to_invoke.Count; ++i)
            {
                ObjectReference<T> curr_reference = to_invoke[i];

                curr_reference.OnInvalidated.Invoke(curr_reference);
            }

            valid = false;
        }

        public T Reference
        {
            get { return referenced_object; }
        }

        public bool Valid
        {
            get { return valid; }
        }
    }
}
