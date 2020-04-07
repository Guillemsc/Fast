using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.Modules
{
    public class PrefabScenesModule : Fast.Modules.Module
    {
        private readonly Fast.PrefabScenes.PrefabSceneController controller = new PrefabScenes.PrefabSceneController();

        public async Task<Fast.PrefabScenes.PrefabScene<T>> LoadPrefabSceneAsync<T>
            (Fast.PrefabScenes.PrefabSceneReference<T> reference) where T : MonoBehaviour
        {
            return await controller.LoadPrefabSceneAsync<T>(reference);
        }

        public async Task UnloadPrefabSceneAsync<T>(Fast.PrefabScenes.PrefabSceneReference<T> reference) where T : MonoBehaviour
        {
            await controller.UnloadPrefabSceneAsync<T>(reference);
        }

        public async Task UnloadPrefabSceneAsync<T>(Fast.PrefabScenes.PrefabScene<T> prefab_scene) where T : MonoBehaviour
        {
            await controller.UnloadPrefabSceneAsync<T>(prefab_scene);
        }

        public Fast.PrefabScenes.PrefabScene<T> GetLoadedPrefabScene<T>(Fast.PrefabScenes.PrefabSceneReference<T> reference) 
            where T : MonoBehaviour
        {
            return controller.GetLoadedPrefabScene<T>(reference);
        }
    }
}
