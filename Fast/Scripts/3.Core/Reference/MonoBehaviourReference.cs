using System;
using UnityEngine;

namespace Fast
{
    public class MonoBehaviourReference : MonoBehaviour
    {
        [Sirenix.OdinInspector.DisableInPlayMode]
        [Sirenix.OdinInspector.Title("Reference name", "Needs to be unique to the whole project")]
        [Sirenix.OdinInspector.HideLabel]
        [SerializeField] private string reference_name = "";

        private void Awake()
        {
            if(!Fast.FastService.Initialized)
            {
                return;
            }

            Fast.FastService.MReferences.AddReference(reference_name, this);
        }

        private void OnDestroy()
        {
            if (!Fast.FastService.Initialized)
            {
                return;
            }

            Fast.FastService.MReferences.RemoveReference(reference_name);
        }
    }
}
