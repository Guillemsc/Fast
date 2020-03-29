#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Fast.Editor.Scenes
{
    [CustomEditor(typeof(Fast.Scenes.ScenesAsset))]
    [Sirenix.OdinInspector.HideMonoScript]
    class ScenesAssetCE : EditorHelper
    {
        private int to_add_selected = 0;

        private List<Fast.Scenes.Scene> to_remove = new List<Fast.Scenes.Scene>();

        protected override void OnDrawInspectorGUI()
        {
            Fast.Scenes.ScenesAsset asset = (Fast.Scenes.ScenesAsset)target;

            EditorGUILayout.BeginHorizontal();
            {
                string[] options = GetAllAvaliableScenesToAdd(asset);

                if (GUILayout.Button("Add"))
                {
                    if (to_add_selected < options.Length && to_add_selected >= 0)
                    {
                        Fast.Scenes.Scene scene = new Fast.Scenes.Scene(options[to_add_selected]);

                        asset.AddScene(scene);
                    }
                }

                to_add_selected = EditorGUILayout.Popup(to_add_selected, options);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorElements.HorizontalLine(Style);

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Selected scenes:", Style.BoldTextStyle);

            for (int i = 0; i < asset.Scenes.Count; ++i)
            {
                Fast.Scenes.Scene curr_scene = asset.Scenes[i];

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField($"{curr_scene.Name}");

                    if (GUILayout.Button("Find", GUILayout.MaxWidth(40)))
                    {
                        string assets_path = Utils.GetSceneAssetPath(curr_scene.Name);
                        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assets_path);

                        Selection.activeObject = obj;
                        EditorGUIUtility.PingObject(obj);
                    }

                    if (GUILayout.Button("X", GUILayout.MaxWidth(40)))
                    {
                        to_remove.Add(curr_scene);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            for (int i = 0; i < to_remove.Count; ++i)
            {
                asset.RemoveScene(to_remove[i].Name);
            }

            to_remove.Clear();

            EditorGUILayout.Separator();

            EditorElements.HorizontalLine(Style);

            if (GUILayout.Button("Remove deleted"))
            {
                CheckAddedScenes(asset);
            }

            if (GUILayout.Button("Add all to build"))
            {
                Utils.AddScenesToBuild(asset.Scenes);
            }
        }

        private string[] GetAllAvaliableScenesToAdd(Fast.Scenes.ScenesAsset asset)
        {
            List<string> ret = new List<string>();

            List<Fast.Scenes.Scene> all_scenes = new List<Fast.Scenes.Scene>();

            IReadOnlyList<string> scenes_paths = Utils.GetAllSceneAssetsPath();

            for (int i = 0; i < scenes_paths.Count; ++i)
            {
                FileInfo info = new FileInfo(scenes_paths[i]);

                string filename = System.IO.Path.GetFileNameWithoutExtension(info.Name);

                Fast.Scenes.Scene scene = new Fast.Scenes.Scene(filename);

                all_scenes.Add(scene);
            }

            for(int i = 0; i < all_scenes.Count; ++i)
            {
                Fast.Scenes.Scene curr_scene = all_scenes[i];

                bool exists = asset.SceneAdded(curr_scene.Name);

                if(!exists)
                {
                    ret.Add(curr_scene.Name);
                }
            }

            return ret.ToArray();
        }

        private void CheckAddedScenes(Fast.Scenes.ScenesAsset asset)
        {
            for(int i = 0; i < asset.Scenes.Count; ++i)
            {
                Fast.Scenes.Scene curr_scene = asset.Scenes[i];

                bool exists = Utils.SceneExists(curr_scene.Name);

                if(!exists)
                {
                    to_remove.Add(curr_scene);
                }
            }

            for (int i = 0; i < to_remove.Count; ++i)
            {
                asset.RemoveScene(to_remove[i].Name);
            }

            to_remove.Clear();
        }
    }
}

#endif
