using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Fast.Scenes
{
    public class SceneController : Fast.IStartable
    {
        private readonly List<Scene> scenes = new List<Scene>();
        private readonly List<LoadedScene> loaded_scenes = new List<LoadedScene>();
        private LoadedScene root_scene = null;

        public void Start()
        {
            LoadRootScene();
        }

        public LoadedScene RootScene => root_scene;

        public void SetLoadableScenes(IReadOnlyList<Scene> scenes)
        {
            this.scenes.Clear();
            this.scenes.AddRange(scenes);
        }

        private void LoadRootScene()
        {
            UnityEngine.SceneManagement.Scene main_scene = SceneManager.GetActiveScene();

            Scene curr_scene = new Scene(main_scene.name);
            SceneRoot root = new SceneRoot(main_scene.GetRootGameObjects().ToList());

            this.root_scene = new LoadedScene(curr_scene, root, main_scene);
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

        public bool SceneIsLoaded(LoadedScene loaded_scene)
        {
            if (loaded_scene == null)
            {
                return false;
            }

            lock (loaded_scenes)
            {
                for (int i = 0; i < loaded_scenes.Count; ++i)
                {
                    LoadedScene curr_scene = loaded_scenes[i];

                    if (curr_scene == loaded_scene)
                    {
                        return true;
                    }
                }
            }

            return false;
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

        public async Task<LoadedScene> LoadSceneAsync(Scene scene, LoadSceneMode mode, bool allow_repeated = true)
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
                if (!allow_repeated)
                {
                    bool already_loaded = SceneIsLoaded(scene);

                    if (already_loaded)
                    {
                        tcs.SetResult(null);
                        can_continue = false;
                    }
                }
            }

            if (can_continue)
            {
                UnityEngine.AsyncOperation async_load = SceneManager.LoadSceneAsync(scene.Name, mode);

                async_load.completed += (delegate (UnityEngine.AsyncOperation operation)
                {
                    loaded_unity_scene = SceneManager.GetSceneByName(scene.Name);

                    if (!loaded_unity_scene.IsValid())
                    {
                        Fast.FastService.MLog.LogWarning(this, $"Scene: {scene.Name} is not valid");

                        tcs.SetResult(null);
                        can_continue = false;
                    }

                    if (can_continue)
                    {
                        GameObject[] root_gameobjects = loaded_unity_scene.GetRootGameObjects();

                        Fast.Scenes.SceneRoot root = new SceneRoot(root_gameobjects.ToList());

                        LoadedScene loaded_scene = new LoadedScene(scene, root, loaded_unity_scene);

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

        public async Task UnloadSceneAsync(LoadedScene loaded_scene, bool allow_repeated = true)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            bool can_continue = true;

            if (loaded_scene == null)
            {
                tcs.SetResult(null);

                can_continue = false;
            }

            if (can_continue)
            {
                bool already_loaded = SceneIsLoaded(loaded_scene);

                if (!already_loaded)
                {
                    tcs.SetResult(null);
                    can_continue = false;
                }
            }

            if (can_continue)
            {
                UnityEngine.AsyncOperation async_unload = SceneManager.UnloadSceneAsync(loaded_scene.UnityScene, UnloadSceneOptions.None);

                async_unload.completed += (delegate (UnityEngine.AsyncOperation operation)
                {
                    lock(loaded_scenes)
                    {
                        for(int i = 0; i < loaded_scenes.Count; ++i)
                        {
                            if(loaded_scenes[i].Scene.Name == loaded_scene.Scene.Name)
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
