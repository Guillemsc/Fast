using UnityEngine;

namespace Fast.Physics
{
    public class Collision2DData
    {
        private GameObject owner = null;
        private Collision2D collision2d = null;

        public Collision2DData(GameObject owner, Collision2D collision2d)
        {
            this.owner = owner;
            this.collision2d = collision2d;
        }

        public GameObject Owner
        {
            get { return owner; }
        }

        public Collision2D Collision2D
        {
            get { return collision2d; }
        }
    }
}
