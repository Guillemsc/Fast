using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.SceneServices
{
    public class SceneService : MonoBehaviour
    {
        private readonly List<Fast.Modules.Module> all_modules = new List<Modules.Module>();
        private readonly List<Fast.Modules.UpdatableModule> updatable_modules = new List<Modules.UpdatableModule>();

        private Fast.Modules.Module AddModule(Fast.Modules.Module module)
        {
            all_modules.Add(module);

            return module;
        }

        private Fast.Modules.UpdatableModule AddUpdatableModule(Fast.Modules.UpdatableModule module)
        {
            all_modules.Add(module);
            updatable_modules.Add(module);

            return module;
        }
    }
}
