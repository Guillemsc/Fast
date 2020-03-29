using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fast.Scenes
{
    public class Utils
    {
        public static async Task<bool> LoadSceneAsync(Scene scene, LoadSceneMode mode)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            if (scene == null)
            {
                tcs.SetResult(false);
            }
            else
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.Name, mode);

                asyncLoad.completed += (delegate (AsyncOperation operation)
                {
                    tcs.SetResult(true);
                });
            }

            return await tcs.Task;
        }
    }
}
