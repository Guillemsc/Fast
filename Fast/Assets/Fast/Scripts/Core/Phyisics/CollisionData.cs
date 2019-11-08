using UnityEngine;

namespace Fast.Physics
{
    public class CollisionData
    {
        private GameObject owner = null;
        private Collision collision = null;

        public CollisionData(GameObject owner, Collision collision)
        {
            this.owner = owner;
            this.collision = collision;
        }

        public GameObject Owner
        {
            get { return owner; }
        }

        public Collision Collision
        {
            get { return collision; }
        }
    }
}
