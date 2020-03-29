﻿#if UNITY_EDITOR


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Fast.Editor.Scenes
{
    class Utils
    {
        public static IReadOnlyList<string> GetAllSceneAssetsPath()
        {
            List<string> scenes_assets_path = new List<string>();

            string[] assets_path = AssetDatabase.FindAssets($"t:SceneAsset");

            for(int i = 0; i < assets_path.Length; ++i)
            {
                string scene_path = AssetDatabase.GUIDToAssetPath(assets_path[i]);

                scenes_assets_path.Add(scene_path);
            }

            return scenes_assets_path;
        }

        public static bool SceneExists(string scene_name)
        {
            string[] assets_path = AssetDatabase.FindAssets($"t:SceneAsset {scene_name}");

            if(assets_path.Length == 0)
            {
                return false;
            }

            return true;
        }

        public static string GetSceneAssetPath(string scene_name)
        {
            string[] assets_path = AssetDatabase.FindAssets($"t:SceneAsset {scene_name}");

            if (assets_path.Length > 0)
            {
                return AssetDatabase.GUIDToAssetPath(assets_path[0]);
            }

            return "";
        }

        public static void AddScenesToBuild(IReadOnlyList<Fast.Scenes.Scene> scenes)
        {
            EditorBuildSettingsScene[] original = EditorBuildSettings.scenes;
            EditorBuildSettingsScene[] new_settings = new EditorBuildSettingsScene[scenes.Count];

            for (int i = 0; i < scenes.Count; ++i)
            {
                Fast.Scenes.Scene curr_scene = scenes[i];

                string assets_path = GetSceneAssetPath(curr_scene.Name);

                EditorBuildSettingsScene new_build_scene = new EditorBuildSettingsScene(assets_path, true);
                new_settings[i] = new_build_scene;
            }

            EditorBuildSettings.scenes = new_settings;
        }
    }
}

#endif
