using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Autofac;

namespace Fast
{
    public class FastService : Fast.MonoSingleton<FastService>
    {
        private bool initialized = false;

        private ApplicationMode mode = ApplicationMode.DEBUG;

        private Fast.Modules.LogModule log_module = null;
        private Fast.Modules.ApplicationModule application_module = null;
        private Fast.Modules.PlatformModule platform_module = null;
        private Fast.Modules.LocalizationModule localization_module = null;
        private Fast.Modules.EventModule event_module = null;
        private Fast.Modules.FirebaseModule firebase_module = null;
        private Fast.Modules.AmazonModule amazon_module = null;

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

        public void Init(ApplicationMode mode)
        {
            if (!initialized)
            {
                initialized = true;

                this.mode = mode;

                log_module = (Modules.LogModule)AddModule(new Modules.LogModule(this));
                application_module = (Modules.ApplicationModule)AddModule(new Modules.ApplicationModule(this));
                platform_module = (Modules.PlatformModule)AddModule(new Modules.PlatformModule(this));
                localization_module = (Modules.LocalizationModule)AddModule(new Modules.LocalizationModule(this));
                event_module = (Modules.EventModule)AddModule(new Modules.EventModule(this));
                firebase_module = (Modules.FirebaseModule)AddModule(new Modules.FirebaseModule(this));
                amazon_module = (Modules.AmazonModule)AddModule(new Modules.AmazonModule(this));

                flow_module = (Modules.FlowModule)AddUpdatableModule(new Modules.FlowModule(this));
                logic_module = (Modules.LogicModule)AddUpdatableModule(new Modules.LogicModule(this));
                time_sliced_module = (Modules.TimeSlicedModule)AddUpdatableModule(new Modules.TimeSlicedModule(this));
                particles_module = (Modules.ParticlesModule)AddUpdatableModule(new Modules.ParticlesModule(this));

                StartModules();

                MLog.LogInfo(this, "Init success");
            }
        }

        private void Update()
        {
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
