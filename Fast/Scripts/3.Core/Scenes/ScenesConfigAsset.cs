using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Scenes
{
    [CreateAssetMenu(fileName = "ScenesConfig", menuName = "Fast/ScenesConfig", order = 1)]
    public class ScenesConfigAsset : ScriptableObject
    {
        [SerializeField] private List<Scene> scenes = new List<Scene>();

        public IReadOnlyList<Scene> Scenes => scenes;

        public void AddScene(Scene scene)
        {
            if(scene == null)
            {
                return;
            }

            bool already_added = SceneAdded(scene.Name);

            if (already_added)
            {
                return;
            }

            scenes.Add(scene);
        }

        public Scene GetScene(string name)
        {
            for (int i = 0; i < scenes.Count; ++i)
            {
                Scene curr_scene = scenes[i];

                if (curr_scene.Name == name)
                {
                    return curr_scene;
                }
            }

            return null;
        }

        public bool SceneAdded(string name)
        {
            for (int i = 0; i < scenes.Count; ++i)
            {
                Scene curr_scene = scenes[i];

                if (curr_scene.Name == name)
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
                Scene curr_scene = scenes[i];

                if (curr_scene.Name == name)
                {
                    scenes.RemoveAt(i);

                    break;
                }
            }
        }
    }
}
