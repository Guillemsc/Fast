using System;
using UnityEngine;

namespace Fast.References
{
    public class Reference : MonoBehaviour
    {
        [Tooltip("Reference name")]
        [SerializeField] private string reference_name = "";

        private readonly Fast.Callback on_destroyed = new Fast.Callback();

        public void OnEnable()
        {
            if (!Fast.FastService.Initialized)
            {
                return;
            }

            Fast.FastService.MReferences.AddReference(reference_name, this);
        }

        public void OnDisable()
        {
            if (!Fast.FastService.Initialized)
            {
                return;
            }

            on_destroyed.Invoke();

            Fast.FastService.MReferences.RemoveReference(reference_name);
        }

        public Fast.Callback OnDestroyed => on_destroyed;
    }
}
