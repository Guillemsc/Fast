using System;
using UnityEngine;

namespace Fast
{
    public class Bootstrap : Fast.MonoSingleton<Bootstrap>
    {
        [Sirenix.OdinInspector.Required]
        [Sirenix.OdinInspector.LabelText("Game configuration")]
        [SerializeField] private Fast.Game.GameConfigAsset game_config = null;

        [Sirenix.OdinInspector.LabelText("Application mode")]
        [SerializeField] private Fast.ApplicationMode mode = Fast.ApplicationMode.DEBUG;

        [Sirenix.OdinInspector.Required]
        [Sirenix.OdinInspector.LabelText("Services prefab")]
        [SerializeField] private GameObject services_prefab = null;

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

                InitGameConfig();

                SpawnBootstrapPrefab();
            }
        }

        private void InitFast()
        {
            FastService.Instance.Init(mode);
        }

        private void InitGameConfig()
        {
            if(game_config == null)
            {
                Fast.FastService.MLog.LogError(this, "Game config is null, quitting application");
                Fast.FastService.MApplication.Quit();
                return;
            }
        }

        private void SpawnBootstrapPrefab()
        {
            if(services_prefab == null)
            {
                Fast.FastService.MLog.LogError(this, "Services prefab is null, quitting application");
                Fast.FastService.MApplication.Quit();
                return;
            }

            Instantiate(services_prefab);
        }
    }
}
