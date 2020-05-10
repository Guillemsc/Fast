using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Scenes
{
    [CreateAssetMenu(fileName = "ScenesConfig", menuName = "Fast/ScenesConfig", order = 1)]
    public class ScenesConfigAsset : ScriptableObject
    {
        [SerializeField] [HideInInspector] private List<string> scenes = new List<string>();

        public IReadOnlyList<string> Scenes => scenes;

        public void AddScene(string scene)
        {
            if(scene == null)
            {
                return;
            }

            bool already_added = SceneAdded(scene);

            if (already_added)
            {
                return;
            }

            scenes.Add(scene);
        }

        public bool SceneAdded(string name)
        {
            for (int i = 0; i < scenes.Count; ++i)
            {
                string curr_scene = scenes[i];

                if (curr_scene == name)
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveScene(string name)
        {
            for (int i = 0; i < scenes.Count; ++i)
            {
                string curr_scene = scenes[i];

                if (curr_scene == name)
                {
                    scenes.RemoveAt(i);

                    break;
                }
            }
        }

        public void UpdateScene(string scene, string to_update)
        {
            for (int i = 0; i < scenes.Count; ++i)
            {
                string curr_scene = scenes[i];

                if (curr_scene == scene)
                {
                    scenes[i] = to_update;

                    break;
                }
            }
        }

        public void SwapScene(string scene, string to_swap)
        {
            for (int i = 0; i < scenes.Count; ++i)
            {
                string curr_scene = scenes[i];

                if (curr_scene == scene)
                {
                    for (int y = 0; y < scenes.Count; ++y)
                    {
                        string curr_scene2 = scenes[y];

                        if(curr_scene2 == to_swap)
                        {
                            scenes[i] = to_swap;
                            scenes[y] = scene;

                            return;
                        }
                    }
                }
            }
        }
    }
}
