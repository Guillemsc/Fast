using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Fast.Scenes
{
    public class SceneController
    {
        private ScenesConfigAsset config = null;

        private readonly List<SceneReference> loaded_scenes = new List<SceneReference>();

        public void SetConfig(ScenesConfigAsset config)
        {
            this.config = config;
        }

        public async Task<bool> LoadSceneAsync(SceneReference reference)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            bool can_continue = true;

            if (reference == null)
            {
                tcs.SetResult(false);
                can_continue = false;
            }

            if (can_continue)
            {
                if (reference.HasInstance())
                {
                    tcs.SetResult(false);
                    can_continue = false;
                }
            }

            if(can_continue)
            {
                bool has_scene = config.SceneAdded(reference.Name);

                if(!has_scene)
                {
                    tcs.SetResult(false);
                    can_continue = false;
                }
            }

            UnityEngine.SceneManagement.Scene loaded_unity_scene = default;

            if (can_continue)
            {
                loaded_unity_scene = SceneManager.GetSceneByName(reference.Name);

                if (loaded_unity_scene == null)
                {
                    tcs.SetResult(false);
                    can_continue = false;
                }
            }

            if (can_continue)
            {
                UnityEngine.AsyncOperation async_load = SceneManager.LoadSceneAsync(reference.Name, LoadSceneMode.Additive);

                async_load.completed += (delegate (UnityEngine.AsyncOperation operation)
                {
                    loaded_unity_scene = SceneManager.GetSceneByName(reference.Name);

                    if (!loaded_unity_scene.IsValid())
                    {
                        Fast.FastService.MLog.LogWarning(this, $"Scene: {reference.Name} is not valid");

                        tcs.SetResult(false);
                        can_continue = false;
                    }

                    SceneInstance instance = null;

                    if (can_continue)
                    {
                        GameObject[] root_gameobjects = loaded_unity_scene.GetRootGameObjects();

                        for(int i = 0; i < root_gameobjects.Length; ++i)
                        {
                            GameObject curr_gameobject = root_gameobjects[i];

                            instance = curr_gameobject.GetComponent<SceneInstance>();

                            if(instance != null)
                            {
                                break;
                            }
                        }

                        if(instance == null)
                        {
                            tcs.SetResult(false);
                            can_continue = false;
                        }
                    }

                    if (can_continue)
                    {
                        reference.SetInstanceData(instance);

                        loaded_scenes.Add(reference);

                        instance.OnSceneLoaded();

                        tcs.SetResult(true);
                    }
                });
            }

            return await tcs.Task;
        }

        public async Task UnloadSceneAsync(SceneReference reference)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            bool can_continue = true;

            if (reference == null)
            {
                tcs.SetResult(false);
                can_continue = false;
            }

            if (can_continue)
            {
                bool found = false;

                lock (loaded_scenes)
                {
                    for (int i = 0; i < loaded_scenes.Count; ++i)
                    {
                        if (loaded_scenes[i] == reference)
                        {
                            loaded_scenes.RemoveAt(i);

                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    tcs.SetResult(false);
                    can_continue = false;
                }
            }

            if (can_continue)
            {
                if (!reference.HasInstance())
                {
                    tcs.SetResult(false);
                    can_continue = false;
                }
            }

            if (can_continue)
            {
                reference.SetInstanceData(null);

                UnityEngine.AsyncOperation async_unload = SceneManager.UnloadSceneAsync(
                    reference.GetInstance().UnityScene, UnloadSceneOptions.None);

                async_unload.completed += (delegate (UnityEngine.AsyncOperation operation)
                {
                    tcs.SetResult(true);
                });
            }

            await tcs.Task;
        }
    }
}
