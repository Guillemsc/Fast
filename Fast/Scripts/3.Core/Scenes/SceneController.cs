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

            bool can_continue = true;

            if (scene == null)
            {
                tcs.SetResult(null);
                can_continue = false;
            }

            UnityEngine.SceneManagement.Scene loaded_unity_scene = default;

            if (can_continue)
            {
                loaded_unity_scene = SceneManager.GetSceneByName(scene.Name);

                if (loaded_unity_scene == null)
                {
                    tcs.SetResult(null);
                    can_continue = false;
                }
            }

            if(can_continue)
            {
                bool already_loaded = SceneIsLoaded(scene);

                if(already_loaded)
                {
                    tcs.SetResult(null);
                    can_continue = false;
                }
            }

            if (can_continue)
            {
                UnityEngine.AsyncOperation async_load = SceneManager.LoadSceneAsync(scene.Name, mode);

                async_load.completed += (delegate (UnityEngine.AsyncOperation operation)
                {
                    loaded_unity_scene = SceneManager.GetSceneByName(scene.Name);

                    if(!loaded_unity_scene.IsValid())
                    {
                        Fast.FastService.MLog.LogWarning(this, $"Scene: {scene.Name} is not valid");

                        tcs.SetResult(null);
                        can_continue = false;
                    }

                    if (can_continue)
                    {
                        Fast.SceneServices.SceneService service = GetSceneService(loaded_unity_scene);

                        if (service == null)
                        {
                            Fast.FastService.MLog.LogWarning(this, $"Scene: {scene.Name} does not have scene services");
                        }

                        LoadedScene loaded_scene = new LoadedScene(scene, service, loaded_unity_scene);

                        lock (loaded_scenes)
                        {
                            loaded_scenes.Add(loaded_scene);
                        }

                        tcs.SetResult(loaded_scene);
                    }
                });
            }

            return await tcs.Task;
        }

        public async Task UnloadSceneAsync(Scene scene)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            bool can_continue = true;

            if (scene == null)
            {
                tcs.SetResult(null);

                can_continue = false;
            }

            if (can_continue)
            {
                bool already_loaded = SceneIsLoaded(scene);

                if (!already_loaded)
                {
                    tcs.SetResult(null);
                    can_continue = false;
                }
            }

            if (can_continue)
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

        private Fast.SceneServices.SceneService GetSceneService(UnityEngine.SceneManagement.Scene unity_scene)
        {
            if(unity_scene == null)
            {
                return null;
            }

            GameObject[] root_gameojects = unity_scene.GetRootGameObjects();

            for(int i = 0; i < root_gameojects.Length; ++i)
            {
                GameObject curr_go = root_gameojects[i];

                Fast.SceneServices.SceneService scene_service = curr_go.GetComponentInChildren<Fast.SceneServices.SceneService>();

                if(scene_service != null)
                {
                    return scene_service;
                }
            }

            return null;
        }
    }
}
