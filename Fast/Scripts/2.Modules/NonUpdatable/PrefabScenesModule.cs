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

        public async Task UnloadPrefabScene(Fast.PrefabScenes.BasePrefabScene prefab_scene)
        {
            await controller.UnloadPrefabSceneAsync(prefab_scene);
        }

        public Fast.PrefabScenes.BasePrefabScene GetLoadedPrefabScene(string name)
        {
            return controller.GetLoadedPrefabScene(name);
        }
    }
}
