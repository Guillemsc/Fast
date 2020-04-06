using System;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Fast.PrefabScenes
{
    public class PrefabSceneController : Fast.IController
    {
        private readonly Dictionary<string, Fast.PrefabScenes.BasePrefabScene> prefab_scenes
            = new Dictionary<string, PrefabScenes.BasePrefabScene>();

        public async Task<PrefabScene<T>> LoadPrefabSceneAsync<T>(Fast.Scenes.Scene to_load)
            where T : MonoBehaviour
        {
            if (to_load == null)
            {
                return null;
            }

            lock (prefab_scenes)
            {
                BasePrefabScene prefab_scene_to_get = GetLoadedPrefabScene(to_load.Name);

                if (prefab_scene_to_get != null)
                {
                    PrefabScene<T> to_check_type = prefab_scene_to_get as PrefabScene<T>;

                    if (to_check_type != null)
                    {
                        return to_check_type;
                    }
                }
            }

            Fast.Scenes.LoadedScene loaded_scene =
                await Fast.FastService.MScenes.LoadSceneAsync(to_load, UnityEngine.SceneManagement.LoadSceneMode.Additive);

            if (loaded_scene == null)
            {
                return null;
            }

            T instance = null;

            for (int i = 0; i < loaded_scene.SceneRoot.RootGameObjects.Count; ++i)
            {
                GameObject curr_root_go = loaded_scene.SceneRoot.RootGameObjects[i];

                instance = curr_root_go.GetComponent<T>();

                if (instance == null)
                {
                    continue;
                }

                break;
            }

            if (instance == null)
            {
                return null;
            }

            PrefabScene<T> prefab_scene = new PrefabScene<T>(loaded_scene, instance);

            lock (prefab_scenes)
            {
                prefab_scenes[prefab_scene.LoadedScene.Scene.Name] = prefab_scene;
            }

            return prefab_scene;
        }

        public BasePrefabScene GetLoadedPrefabScene(string name)
        {
            BasePrefabScene prefab_scene_to_get = null;

            prefab_scenes.TryGetValue(name, out prefab_scene_to_get);

            return prefab_scene_to_get;
        }

        public async Task UnloadPrefabSceneAsync(BasePrefabScene prefab_scene)
        {
            if(prefab_scene == null)
            {
                return;
            }

            prefab_scenes.Remove(prefab_scene.LoadedScene.Scene.Name);

            if (prefab_scene.MonoBehaviourInstance != null)
            {
                prefab_scene.MonoBehaviourInstance.gameObject.SetActive(false);
            }

            await Fast.FastService.MScenes.UnloadSceneAsync(prefab_scene.LoadedScene.Scene);
        }
    }
}
