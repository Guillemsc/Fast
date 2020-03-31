using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast
{
    [Sirenix.OdinInspector.HideMonoScript]
    public class FastService : Fast.MonoSingleton<FastService>
    {
        [Sirenix.OdinInspector.Required]
        [Sirenix.OdinInspector.LabelText("Game configuration")]
        [SerializeField] private Fast.Game.GameConfigAsset game_config = null;

        [Sirenix.OdinInspector.Required]
        [Sirenix.OdinInspector.LabelText("Scenes configuration")]
        [SerializeField] private Fast.Scenes.ScenesConfigAsset scenes_config = null;

        [Sirenix.OdinInspector.Required]
        [Sirenix.OdinInspector.LabelText("Services prefab")]
        [SerializeField] private GameObject services_prefab = null;

        [Sirenix.OdinInspector.LabelText("Application mode")]
        [SerializeField] private Fast.ApplicationMode mode = Fast.ApplicationMode.DEBUG;

        private bool first_update = true;

        private bool initialized = false;

        private Fast.Modules.LogModule log_module = null;
        private Fast.Modules.ApplicationModule application_module = null;
        private Fast.Modules.PlatformModule platform_module = null;
        private Fast.Modules.GameDataSaveModule game_data_save_module = null;
        private Fast.Modules.LocalizationModule localization_module = null;
        private Fast.Modules.EventModule event_module = null;
        private Fast.Modules.FirebaseModule firebase_module = null;
        private Fast.Modules.AmazonModule amazon_module = null;
        private Fast.Modules.GameModule game_module = null;
        private Fast.Modules.ScenesModule scenes_module = null;

        private Fast.Modules.SettingsDataSaveModule settings_data_save_module = null;
        private Fast.Modules.TimeModule time_module = null;
        private Fast.Modules.FlowModule flow_module = null;
        private Fast.Modules.LogicModule logic_module = null;
        private Fast.Modules.TimeSlicedModule time_sliced_module = null;
        private Fast.Modules.ParticlesModule particles_module = null; 

        private List<Modules.IModule> all_modules = new List<Modules.IModule>();
        private List<Modules.UpdatableModule> updatable_modules = new List<Modules.UpdatableModule>();

        FastService()
        {
            InitInstance(this);
        }

        public static bool Initialized
        {
            get { return Instance.initialized; }
        }

        public void Init()
        {
            if (!initialized)
            {
                log_module = (Modules.LogModule)AddModule(new Modules.LogModule());
                application_module = (Modules.ApplicationModule)AddModule(new Modules.ApplicationModule());
                platform_module = (Modules.PlatformModule)AddModule(new Modules.PlatformModule());
                game_data_save_module = (Modules.GameDataSaveModule)AddModule(new Modules.GameDataSaveModule());
                localization_module = (Modules.LocalizationModule)AddModule(new Modules.LocalizationModule());
                event_module = (Modules.EventModule)AddModule(new Modules.EventModule());
                firebase_module = (Modules.FirebaseModule)AddModule(new Modules.FirebaseModule());
                amazon_module = (Modules.AmazonModule)AddModule(new Modules.AmazonModule());
                game_module = (Modules.GameModule)AddModule(new Modules.GameModule());
                scenes_module = (Modules.ScenesModule)AddModule(new Modules.ScenesModule());

                settings_data_save_module = (Modules.SettingsDataSaveModule)AddUpdatableModule(new Modules.SettingsDataSaveModule());
                time_module = (Modules.TimeModule)AddUpdatableModule(new Modules.TimeModule());
                flow_module = (Modules.FlowModule)AddUpdatableModule(new Modules.FlowModule());
                logic_module = (Modules.LogicModule)AddUpdatableModule(new Modules.LogicModule());
                time_sliced_module = (Modules.TimeSlicedModule)AddUpdatableModule(new Modules.TimeSlicedModule());
                particles_module = (Modules.ParticlesModule)AddUpdatableModule(new Modules.ParticlesModule());

                StartModules();

                InitGameConfig();
                InitScenesConfig();
                SpawnServicesPrefab();

                initialized = true;

                MLog.LogInfo(this, "Init success");
            }
        }

        private void InitGameConfig()
        {
            if (game_config == null)
            {
                MLog.LogError(this, "Game config is null, quitting application");
                MApplication.Quit();
                return;
            }

            MGame.SetGameConfig(game_config);
        }

        private void InitScenesConfig()
        {
            if (scenes_config == null)
            {
                MLog.LogError(this, "Scenes config is null, quitting application");
                MApplication.Quit();
                return;
            }

            MScenes.SetScenesConfig(scenes_config);
        }

        private void SpawnServicesPrefab()
        {
            if (services_prefab == null)
            {
                MLog.LogError(this, "Services prefab is null, quitting application");
                MApplication.Quit();
                return;
            }

            GameObject services_instance = Instantiate(services_prefab);
            services_instance.name = "Services";
        }

        private void Update()
        {
            if(first_update)
            {
                first_update = false;

                Init();
            }

            UpdateModules();
        }

        private void OnApplicationQuit()
        {
            CleanUpModules();
        }

        private Modules.Module AddModule(Modules.Module module)
        {
            all_modules.Add(module);

            return module;
        }

        private Modules.UpdatableModule AddUpdatableModule(Modules.UpdatableModule module)
        {
            all_modules.Add(module);
            updatable_modules.Add(module);

            return module;
        }

        private void StartModules()
        {
            for (int i = 0; i < all_modules.Count; ++i)
            {
                all_modules[i].Awake();
            }

            for (int i = 0; i < all_modules.Count; ++i)
            {
                all_modules[i].Start();
            }
        }

        private void UpdateModules()
        {
            for (int i = 0; i < updatable_modules.Count; ++i)
            {
                updatable_modules[i].PreUpdate();
            }

            for (int i = 0; i < updatable_modules.Count; ++i)
            {
                updatable_modules[i].Update();
            }

            for (int i = 0; i < updatable_modules.Count; ++i)
            {
                updatable_modules[i].PostUpdate();
            }
        }

        private void CleanUpModules()
        {
            for (int i = 0; i < all_modules.Count; ++i)
            {
                all_modules[i].CleanUp();
            }

            all_modules.Clear();
        }

        public ApplicationMode ApplicationMode
        {
            get { return mode; }
        }

        public static Fast.Modules.LogModule MLog
        {
            get { return Instance.log_module; }
        }

        public static Fast.Modules.ApplicationModule MApplication
        {
            get { return Instance.application_module; }
        }

        public static Fast.Modules.PlatformModule MPlatform
        {
            get { return Instance.platform_module; }
        }

        public static Fast.Modules.GameDataSaveModule MGameDataSave
        {
            get { return Instance.game_data_save_module; }
        }

        public static Fast.Modules.LocalizationModule MLocalization
        {
            get { return Instance.localization_module; }
        }

        public static Fast.Modules.EventModule MEvent
        {
            get { return Instance.event_module; }
        }

        public static Fast.Modules.FirebaseModule MFirebase
        {
            get { return Instance.firebase_module; }
        }

        public static Fast.Modules.AmazonModule MAmazon
        {
            get { return Instance.amazon_module; }
        }

        public static Fast.Modules.GameModule MGame
        {
            get { return Instance.game_module; }
        }

        public static Fast.Modules.ScenesModule MScenes
        {
            get { return Instance.scenes_module; }
        }

        public static Fast.Modules.SettingsDataSaveModule MSettingsDataSave
        {
            get { return Instance.settings_data_save_module; }
        }

        public static Fast.Modules.TimeModule MTime
        {
            get { return Instance.time_module; }
        }

        public static Fast.Modules.FlowModule MFlow
        {
            get { return Instance.flow_module; }
        }

        public static Fast.Modules.LogicModule MLogic
        {
            get { return Instance.logic_module; }
        }

        public static Modules.TimeSlicedModule MTimeSliced
        {
            get { return Instance.time_sliced_module; }
        }

        public static Modules.ParticlesModule MParticles
        {
            get { return Instance.particles_module; }
        }
    }
}
