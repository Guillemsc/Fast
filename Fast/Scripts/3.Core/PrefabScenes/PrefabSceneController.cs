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

        public async Task<PrefabScene<T>> LoadPrefabSceneAsync<T>(Fast.Scenes.Scene to_load, Fast.Scenes.LoadedScene to_set, GameObject parent)
            where T : MonoBehaviour
        {
            if (to_load == null)
            {
                return null;
            }

            if (to_set == null)
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

            SceneManager.MoveGameObjectToScene(instance.gameObject, to_set.UnityScene);

            instance.gameObject.SetParent(parent);

            PrefabScene<T> prefab_scene = new PrefabScene<T>(to_load.Name, instance);

            Fast.FastService.MScenes.UnloadSceneAsync(to_load).ExecuteAsync();

            lock (prefab_scenes)
            {
                prefab_scenes[prefab_scene.Name] = prefab_scene;
            }

            return prefab_scene;
        }

        public BasePrefabScene GetLoadedPrefabScene(string name)
        {
            BasePrefabScene prefab_scene_to_get = null;

            prefab_scenes.TryGetValue(name, out prefab_scene_to_get);

            return prefab_scene_to_get;
        }

        public void UnloadPrefabScene(BasePrefabScene prefab_scene)
        {
            if(prefab_scene == null)
            {
                return;
            }

            prefab_scenes.Remove(prefab_scene.Name);

            if(prefab_scene.MonoBehaviourInstance == null)
            {
                return;
            }

            MonoBehaviour.Destroy(prefab_scene.MonoBehaviourInstance.gameObject);
        }
    }
}
