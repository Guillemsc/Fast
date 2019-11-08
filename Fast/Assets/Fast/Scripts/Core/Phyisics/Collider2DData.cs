using UnityEngine;

namespace Fast.Physics
{
    public class Collider2DData
    {
        private GameObject owner = null;
        private Collider2D collider2d = null;

        public Collider2DData(GameObject owner, Collider2D collider2d)
        {
            this.owner = owner;
            this.collider2d = collider2d;
        }

        public GameObject Owner
        {
            get { return owner; }
        }

        public Collider2D Collider2D
        {
            get { return collider2d; }
        }
    }
}
