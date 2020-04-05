using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.Modules
{
    public class PrefabScenesModule : Fast.Modules.Module
    {
        private readonly Fast.PrefabScenes.PrefabSceneController controller = new PrefabScenes.PrefabSceneController();

        public async Task<Fast.PrefabScenes.PrefabScene<T>> LoadPrefabSceneAsync<T>(Fast.Scenes.Scene to_load, 
            Fast.Scenes.LoadedScene to_set, GameObject parent) where T : MonoBehaviour
        {
            return await controller.LoadPrefabSceneAsync<T>(to_load, to_set, parent);
        }

        public void UnloadPrefabScene(Fast.PrefabScenes.BasePrefabScene prefab_scene)
        {
            controller.UnloadPrefabScene(prefab_scene);
        }
    }
}
