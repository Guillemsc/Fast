using UnityEngine;

namespace Fast.Physics
{
    public class ColliderData
    {
        private GameObject owner = null;
        private Collider collider = null;

        public ColliderData(GameObject owner, Collider collider)
        {
            this.owner = owner;
            this.collider = collider;
        }

        public GameObject Owner
        {
            get { return owner; }
        }

        public Collider Collider
        {
            get { return collider; }
        }
    }
}
