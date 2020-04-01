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

        private readonly List<SceneResolver> using_resolvers = new List<SceneResolver>();

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

        public async Task ResolveScenes(SceneResolver resolver)
        {
            if(resolver == null)
            {
                return;
            }

            lock (using_resolvers)
            {
                for (int i = 0; i < using_resolvers.Count; ++i)
                {
                    if (using_resolvers[i] == resolver)
                    {
                        return;
                    }
                }

                using_resolvers.Add(resolver);
            }

            for(int i = 0; i < resolver.ScenesToResolve.Count; ++i)
            {
                Scene curr_scene = resolver.ScenesToResolve[i];

                bool loaded = SceneIsLoaded(curr_scene);

                if(loaded)
                {
                    continue;
                }

                await LoadSceneAsync(curr_scene, LoadSceneMode.Additive);
            }
        }

        public async Task UnresolveScenes(SceneResolver resolver)
        {
            if (resolver == null)
            {
                return;
            }

            bool found = false;

            lock (using_resolvers)
            {
                for (int i = 0; i < using_resolvers.Count; ++i)
                {
                    if (using_resolvers[i] == resolver)
                    {
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                return;
            }

            for (int i = 0; i < resolver.ScenesToResolve.Count; ++i)
            {
                Scene curr_scene = resolver.ScenesToResolve[i];

                bool is_used = SceneIsUsed(curr_scene, resolver);

                if(is_used)
                {
                    continue;
                }

                await UnloadSceneAsync(curr_scene);
            }
        }

        private bool SceneIsUsed(Scene scene, SceneResolver to_ignore)
        {
            for (int i = 0; i < using_resolvers.Count; ++i)
            {
                SceneResolver curr_resolver = using_resolvers[i];

                if(curr_resolver == to_ignore)
                {
                    continue;
                }

                for(int y = 0; y < curr_resolver.ScenesToResolve.Count; ++y)
                {
                    if(curr_resolver.ScenesToResolve[y] == scene)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
