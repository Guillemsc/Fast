using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Fast
{
    class FastInstance : MonoBehaviour
    {
        private static FastInstance instance = null;

        private Fast.Modules.ApplicationModule application_module = null;
        private Fast.Modules.PlatformModule platform_module = null;
        private Fast.Modules.LocalizationModule localization_module = null;
        private Fast.Modules.EventModule event_module = null;
        private Fast.Modules.FirebaseModule firebase_module = null;

        private Fast.Modules.FlowModule flow_module = null;
        private Fast.Modules.LogicModule logic_module = null;
        private Fast.Modules.TimeSlicedModule time_sliced_module = null;

        private List<Modules.Module> all_modules = new List<Modules.Module>();
        private List<Modules.UpdatableModule> updatable_modules = new List<Modules.UpdatableModule>();

        FastInstance()
        {
            if(instance == null)
            {
                instance = this;
            }

            application_module = (Modules.ApplicationModule)AddModule(new Modules.ApplicationModule());
            platform_module = (Modules.PlatformModule)AddModule(new Modules.PlatformModule());
            localization_module = (Modules.LocalizationModule)AddModule(new Modules.LocalizationModule());
            event_module = (Modules.EventModule)AddModule(new Modules.EventModule());
            firebase_module = (Modules.FirebaseModule)AddModule(new Modules.FirebaseModule());

            flow_module = (Modules.FlowModule)AddUpdatableModule(new Modules.FlowModule());
            logic_module = (Modules.LogicModule)AddUpdatableModule(new Modules.LogicModule());
            time_sliced_module = (Modules.TimeSlicedModule)AddUpdatableModule(new Modules.TimeSlicedModule());
        }

        private void Start()
        {
            StartModules();
        }

        private void Update()
        {
            UpdateModules();
        }

        private void OnApplicationQuit()
        {
            CleanUpModules();
        }

        public static FastInstance Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go_instance = new GameObject("FastInstance");
                    instance = go_instance.AddComponent<FastInstance>();

                    DontDestroyOnLoad(go_instance);
                }

                return instance;
            }
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

        public Fast.Modules.ApplicationModule MApplication
        {
            get { return application_module; }
        }

        public Fast.Modules.PlatformModule MPlatform
        {
            get { return platform_module; }
        }

        public Fast.Modules.LocalizationModule MLocalization
        {
            get { return localization_module; }
        }

        public Fast.Modules.EventModule MEvent
        {
            get { return event_module; }
        }

        public Fast.Modules.FirebaseModule MFirebase
        {
            get { return firebase_module; }
        }

        public Fast.Modules.FlowModule MFlow
        {
            get { return flow_module; }
        }

        public Fast.Modules.LogicModule MLogic
        {
            get { return logic_module; }
        }

        public Modules.TimeSlicedModule MTimeSliced
        {
            get { return time_sliced_module; }
        }
    }
}
