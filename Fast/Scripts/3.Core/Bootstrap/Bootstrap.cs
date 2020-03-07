using System;
using UnityEngine;

namespace Fast
{
    public class Bootstrap : Fast.MonoSingleton<Bootstrap>
    {
        [Sirenix.OdinInspector.LabelText("Services prefab")]
        [SerializeField] private GameObject services_prefab = null;

        [Sirenix.OdinInspector.LabelText("Mode")]
        [SerializeField] private ApplicationMode mode = ApplicationMode.DEBUG;

        private bool first_update = true;

        Bootstrap()
        {
            InitInstance(this);
        }

        private void Update()
        {
            if (first_update)
            {
                first_update = false;

                InitFast();

                SpawnBootstrapPrefab();
            }
        }

        private void SpawnBootstrapPrefab()
        {
            Instantiate(services_prefab);
        }

        private void InitFast()
        {
            FastService.Instance.Init(mode);
        }
    }
}
