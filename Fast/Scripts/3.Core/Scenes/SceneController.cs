using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Fast.Scenes
{
    public class SceneController
    {
        private readonly List<Scene> scenes = new List<Scene>();
        private readonly List<LoadedScene> loaded_scenes = new List<LoadedScene>();

        public void SetLoadableScenes(IReadOnlyList<Scene> scenes)
        {
            this.scenes.Clear();
            this.scenes.AddRange(scenes);
        }

        public Scene GetLoadableScene(string name)
        {
            for(int i = 0; i < scenes.Count; ++i)
            {
                Scene curr_scene = scenes[i];

                if(curr_scene.Name == name)
                {
                    return curr_scene;
                }
            }

            return null;
        }

        public bool SceneIsLoaded(Scene scene)
        {
            if(scene == null)
            {
                return false;
            }

            lock (loaded_scenes)
            {
                for (int i = 0; i < loaded_scenes.Count; ++i)
                {
                    LoadedScene curr_scene = loaded_scenes[i];

                    if (curr_scene.Scene.Name == scene.Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<LoadedScene> LoadSceneAsync(Scene scene, LoadSceneMode mode)
        {
            TaskCompletionSource<LoadedScene> tcs = new TaskCompletionSource<LoadedScene>();

            bool can_load = true;

            if (scene == null)
            {
                tcs.SetResult(null);
                can_load = false;
            }

            UnityEngine.SceneManagement.Scene loaded_unity_scene = default;

            if (can_load)
            {
                loaded_unity_scene = SceneManager.GetSceneByName(scene.Name);

                if (loaded_unity_scene == null)
                {
                    tcs.SetResult(null);
                    can_load = false;
                }
            }

            if(can_load)
            {
                bool already_loaded = SceneIsLoaded(scene);

                if(already_loaded)
                {
                    tcs.SetResult(null);
                    can_load = false;
                }
            }

            if (can_load)
            {
                UnityEngine.AsyncOperation async_load = SceneManager.LoadSceneAsync(scene.Name, mode);

                async_load.completed += (delegate (UnityEngine.AsyncOperation operation)
                {
                    if (loaded_unity_scene == null)
                    {

                    }

                    LoadedScene loaded_scene = new LoadedScene(scene, loaded_unity_scene);

                    lock(loaded_scenes)
                    {
                        loaded_scenes.Add(loaded_scene);
                    }

                    tcs.SetResult(loaded_scene);
                });
            }

            return await tcs.Task;
        }

        public async Task UnloadSceneAsync(Scene scene)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            bool can_unload = true;

            if (scene == null)
            {
                tcs.SetResult(null);

                can_unload = false;
            }

            if (can_unload)
            {
                bool already_loaded = SceneIsLoaded(scene);

                if (!already_loaded)
                {
                    tcs.SetResult(null);
                    can_unload = false;
                }
            }

            if (can_unload)
            {
                UnityEngine.AsyncOperation async_unload = SceneManager.UnloadSceneAsync(scene.Name);

                async_unload.completed += (delegate (UnityEngine.AsyncOperation operation)
                {
                    lock(loaded_scenes)
                    {
                        for(int i = 0; i < loaded_scenes.Count; ++i)
                        {
                            if(loaded_scenes[i].Scene.Name == scene.Name)
                            {
                                loaded_scenes.RemoveAt(i);

                                break;
                            }
                        }
                    }

                    tcs.SetResult(null);
                });
            }

            await tcs.Task;
        }
    }
}
