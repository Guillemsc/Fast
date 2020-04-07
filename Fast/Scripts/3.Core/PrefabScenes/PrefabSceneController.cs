using System;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Fast.PrefabScenes
{
    public class PrefabSceneController : Fast.IController
    {
        private readonly Dictionary<Type, Dictionary<string, Fast.PrefabScenes.BasePrefabScene>> prefab_scenes 
            = new Dictionary<Type, Dictionary<string, BasePrefabScene>>();

        private Dictionary<string, Fast.PrefabScenes.BasePrefabScene> GetPrefabScenesType(Type type)
        {
            Dictionary<string, Fast.PrefabScenes.BasePrefabScene> ret = null;

            prefab_scenes.TryGetValue(type, out ret);

            return ret;
        }

        public PrefabScene<T> GetLoadedPrefabScene<T>(PrefabSceneReference<T> reference) where T : MonoBehaviour
        {
            if(reference == null)
            {
                return null;
            }

            Dictionary<string, BasePrefabScene> types = GetPrefabScenesType(typeof(T));

            if(types == null)
            {
                return null;
            }

            BasePrefabScene prefab_scene = null;
            types.TryGetValue(reference.UID, out prefab_scene);

            if(prefab_scene == null)
            {
                return null;
            }

            PrefabScene<T> ret = prefab_scene as PrefabScene<T>;

            return ret;
        }

        public async Task<PrefabScene<T>> LoadPrefabSceneAsync<T>(PrefabSceneReference<T> reference)
            where T : MonoBehaviour
        {
            if (reference == null)
            {
                return null;
            }

            lock (prefab_scenes)
            {
                BasePrefabScene prefab_scene_to_get = GetLoadedPrefabScene<T>(reference);

                if (prefab_scene_to_get != null)
                {
                    PrefabScene<T> to_check_type = prefab_scene_to_get as PrefabScene<T>;

                    if (to_check_type != null)
                    {
                        return to_check_type;
                    }
                }
            }

            Fast.Scenes.Scene to_load = Fast.FastService.MScenes.GetLoadableScene(reference.SceneName);

            if(to_load == null)
            {
                return null;
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

            PrefabScene<T> prefab_scene = new PrefabScene<T>(reference.UID, loaded_scene, instance);

            lock (prefab_scenes)
            {
                Dictionary<string, BasePrefabScene> types = GetPrefabScenesType(typeof(T));

                if(types == null)
                {
                    types = new Dictionary<string, BasePrefabScene>();
                    prefab_scenes.Add(typeof(T), types);
                }

                types[reference.UID] = prefab_scene;
            }

            return prefab_scene;
        }

        public async Task UnloadPrefabSceneAsync<T>(PrefabSceneReference<T> reference) where T : MonoBehaviour
        {
            if(reference == null)
            {
                return;
            }

            PrefabScene<T> prefab_scene = GetLoadedPrefabScene(reference);

            await UnloadPrefabSceneAsync(prefab_scene);
        }

        public async Task UnloadPrefabSceneAsync<T>(PrefabScene<T> prefab_scene) where T : MonoBehaviour
        {
            if (prefab_scene == null)
            {
                return;
            }

            Dictionary<string, BasePrefabScene> types = GetPrefabScenesType(typeof(T));

            if (types != null)
            {
                types.Remove(prefab_scene.UID);
            }

            await Fast.FastService.MScenes.UnloadSceneAsync(prefab_scene.LoadedScene);
        }
    }
}
