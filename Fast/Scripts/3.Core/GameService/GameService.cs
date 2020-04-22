using System;
using System.Collections.Generic;

namespace Fast.Game
{
    public class GameService : Fast.MonoSingleton<GameService>
    {
        private Dictionary<Type, GameModule> modules = new Dictionary<Type, GameModule>();
        private List<UpdatableGameModule> updatable_modules = new List<UpdatableGameModule>();
        private Dictionary<Type, GameConfigAsset> configs = new Dictionary<Type, GameConfigAsset>();

        private bool initialized = false;

        GameService()
        {
            InitInstance(this);
        }

        private void Update()
        {
            UpdateGameServices();
        }

        private void OnApplicationQuit()
        {
            CleanUpGameServices();
        }

        public bool Initialized => initialized;

        public void Init()
        {
            if(initialized)
            {
                return;
            }

            StartGameServices();

            initialized = true;

            Fast.FastService.MLog.LogInfo(this, $"Game services inited");
        }

        public bool RegisterModule<T>(T game_module) where T : GameModule
        {
            if(initialized)
            {
                return false;
            }

            if(game_module == null)
            {
                return false;
            }

            Type module_type = typeof(T);

            bool already_added = modules.ContainsKey(module_type);

            if(already_added)
            {
                return false;
            }

            modules[module_type] = game_module;

            UpdatableGameModule updatable = game_module as UpdatableGameModule;

            if(updatable != null)
            {
                updatable_modules.Add(updatable);
            }

            return true;
        }

        public T GetModule<T>() where T : GameModule
        {
            Type module_type = typeof(T);

            GameModule module_to_get = null;
            modules.TryGetValue(module_type, out module_to_get);

            return module_to_get as T;
        }

        public void RegisterConfig<T>(T config) where T : GameConfigAsset
        {
            if (initialized)
            {
                return;
            }

            if (config == null)
            {
                return;
            }

            Type module_type = typeof(T);

            configs[module_type] = config;
        }

        public T GetConfig<T>() where T : GameConfigAsset
        {
            Type config_type = typeof(T);

            GameConfigAsset config_to_get = null;
            configs.TryGetValue(config_type, out config_to_get);

            return config_to_get as T;
        }

        private void StartGameServices()
        {
            if (initialized)
            {
                return;
            }

            foreach (KeyValuePair<Type, GameModule> entry in modules)
            {
                entry.Value.Awake();
            }

            foreach (KeyValuePair<Type, GameModule> entry in modules)
            {
                entry.Value.Start();
            }
        }

        private void UpdateGameServices()
        {
            if (!initialized)
            {
                return;
            }

            for(int i = 0; i < updatable_modules.Count; ++i)
            {
                updatable_modules[i].Update();
            }
        }

        private void CleanUpGameServices()
        {
            if(!initialized)
            {
                return;
            }

            foreach (KeyValuePair<Type, GameModule> entry in modules)
            {
                entry.Value.CleanUp();
            }

            modules.Clear();
        }
    }
}
