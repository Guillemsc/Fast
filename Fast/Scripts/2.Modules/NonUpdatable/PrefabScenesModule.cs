using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.Modules
{
    public class PrefabScenesModule : Fast.Modules.Module
    {
        private readonly Fast.PrefabScenes.PrefabSceneController controller = new PrefabScenes.PrefabSceneController();

        public async Task<Fast.PrefabScenes.PrefabScene<T>> LoadPrefabSceneAsync<T>(Fast.Scenes.Scene to_load) where T : MonoBehaviour
        {
            return await controller.LoadPrefabSceneAsync<T>(to_load);
        }

        public async Task UnloadPrefabScene<T>(Fast.PrefabScenes.PrefabScene<T> prefab_scene) where T : MonoBehaviour
        {
            await controller.UnloadPrefabSceneAsync<T>(prefab_scene);
        }

        public Fast.PrefabScenes.PrefabScene<T> GetLoadedPrefabScene<T>(string name) where T : MonoBehaviour
        {
            return controller.GetLoadedPrefabScene<T>(name);
        }
    }
}
