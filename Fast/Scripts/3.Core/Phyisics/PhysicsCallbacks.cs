using UnityEngine;

namespace Fast.Physics
{
    public class PhysicsCallbacks : MonoBehaviour
    {
        private Fast.Callback<CollisionData> on_collision_enter = new Fast.Callback<CollisionData>();
        private Fast.Callback<CollisionData> on_collision_stay = new Fast.Callback<CollisionData>();
        private Fast.Callback<CollisionData> on_collision_exit = new Fast.Callback<CollisionData>();
                
        private Fast.Callback<Collision2DData> on_collision_enter2d = new Fast.Callback<Collision2DData>();
        private Fast.Callback<Collision2DData> on_collision_stay2d = new Fast.Callback<Collision2DData>();
        private Fast.Callback<Collision2DData> on_collision_exit2d = new Fast.Callback<Collision2DData>();
                
        private Fast.Callback<ColliderData> on_trigger_enter = new Fast.Callback<ColliderData>();
        private Fast.Callback<ColliderData> on_trigger_stay = new Fast.Callback<ColliderData>();
        private Fast.Callback<ColliderData> on_trigger_exit = new Fast.Callback<ColliderData>();
                
        private Fast.Callback<Collider2DData> on_trigger_enter2d = new Fast.Callback<Collider2DData>();
        private Fast.Callback<Collider2DData> on_trigger_stay2d = new Fast.Callback<Collider2DData>();
        private Fast.Callback<Collider2DData> on_trigger_exit2d = new Fast.Callback<Collider2DData>();

        public Callback<CollisionData> OnCollEnter
        {
            get { return on_collision_enter; } 
        }

        public Callback<CollisionData> OnCollStay
        {
            get { return on_collision_stay; }
        }

        public Callback<CollisionData> OnCollExit
        {
            get { return on_collision_exit; }
        }

        public Callback<Collision2DData> OnCollEnter2D
        {
            get { return on_collision_enter2d; }
        }

        public Callback<Collision2DData> OnCollStay2D
        {
            get { return on_collision_stay2d; }
        }

        public Callback<Collision2DData> OnCollExit2D
        {
            get { return on_collision_exit2d; }
        }

        public Callback<ColliderData> OnTriggEnter
        {
            get { return on_trigger_enter; }
        }

        public Callback<ColliderData> OnTriggStay
        {
            get { return on_trigger_stay; }
        }

        public Callback<ColliderData> OnTriggExit
        {
            get { return on_trigger_exit; }
        }

        public Callback<Collider2DData> OnTriggEnter2D
        {
            get { return on_trigger_enter2d; }
        }

        public Callback<Collider2DData> OnTriggStay2D
        {
            get { return on_trigger_stay2d; }
        }

        public Callback<Collider2DData> OnTriggExit2D
        {
            get { return on_trigger_exit2d; }
        }

        private void OnCollisionEnter(Collision collision)
        {
            CollisionData ret = new CollisionData(this.gameObject, collision);

            on_collision_enter.Invoke(ret);
        }

        private void OnCollisionStay(Collision collision)
        {
            CollisionData ret = new CollisionData(this.gameObject, collision);

            on_collision_stay.Invoke(ret);
        }

        private void OnCollisionExit(Collision collision)
        {
            CollisionData ret = new CollisionData(this.gameObject, collision);

            on_collision_exit.Invoke(ret);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Collision2DData ret = new Collision2DData(this.gameObject, collision);

            on_collision_enter2d.Invoke(ret);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            Collision2DData ret = new Collision2DData(this.gameObject, collision);

            on_collision_stay2d.Invoke(ret);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Collision2DData ret = new Collision2DData(this.gameObject, collision);

            on_collision_exit2d.Invoke(ret);
        }

        private void OnTriggerEnter(Collider collision)
        {
            ColliderData ret = new ColliderData(this.gameObject, collision);

            on_trigger_enter.Invoke(ret);
        }

        private void OnTriggerStay(Collider collision)
        {
            ColliderData ret = new ColliderData(this.gameObject, collision);

            on_trigger_stay.Invoke(ret);
        }

        private void OnTriggerExit(Collider collision)
        {
            ColliderData ret = new ColliderData(this.gameObject, collision);

            on_trigger_exit.Invoke(ret);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Collider2DData ret = new Collider2DData(this.gameObject, collision);

            on_trigger_enter2d.Invoke(ret);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            Collider2DData ret = new Collider2DData(this.gameObject, collision);

            on_trigger_stay2d.Invoke(ret);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Collider2DData ret = new Collider2DData(this.gameObject, collision);

            on_trigger_exit2d.Invoke(ret);
        }
    }
}
