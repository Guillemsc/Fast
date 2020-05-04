#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Fast.Editor.Scenes
{
    [CustomEditor(typeof(Fast.Scenes.ScenesConfigAsset))]
    [Sirenix.OdinInspector.HideMonoScript]
    class ScenesAssetCE : EditorHelper
    {
        private Fast.Scenes.ScenesConfigAsset target_script = null;

        private List<string> to_remove = new List<string>();

        private string[] available_scenes_to_add = null;

        private void OnEnable()
        {
            target_script = (Fast.Scenes.ScenesConfigAsset)target;

            UpdateAvaliableScenesToAdd();
            UpdateScenesOnBuild();
        }

        protected override void OnDrawInspectorGUI()
        {
            UpdateAvaliableScenesToAdd();

            DrawAddSceneButtonGUI();

            EditorGUILayout.Separator();
            EditorElements.HorizontalLine(Style);

            DrawSelectedScenesGUI();
        }

        private void DrawAddSceneButtonGUI()
        {
            if (GUILayout.Button("+", GUILayout.MaxWidth(40)))
            {
                if(available_scenes_to_add.Length <= 0)
                {
                    return;
                }

                target_script.AddScene(available_scenes_to_add[0]);

                UpdateAvaliableScenesToAdd();

                UpdateScenesOnBuild();
            }
        }

        private void DrawSelectedScenesGUI()
        {
            EditorGUILayout.LabelField("Project scenes:", Style.BoldTextStyle);

            IReadOnlyList<string> scenes = target_script.Scenes;

            for (int i = 0; i < scenes.Count; ++i)
            {
                string curr_scene = scenes[i];

                EditorGUILayout.BeginHorizontal();
                {
                    List<string> to_show = new List<string>();

                    to_show.Add(curr_scene);

                    if (available_scenes_to_add != null)
                    {
                        to_show.AddRange(available_scenes_to_add);
                    }

                    int selected_scene_dropdown = EditorGUILayout.Popup(0, to_show.ToArray());

                    if(selected_scene_dropdown != 0)
                    {
                        string new_scene = to_show[selected_scene_dropdown];

                        target_script.UpdateScene(curr_scene, new_scene);

                        UpdateAvaliableScenesToAdd();

                        UpdateScenesOnBuild();
                    }

                    if (GUILayout.Button("▲", GUILayout.MaxWidth(25)))
                    {
                        if(i - 1 >= 0)
                        {
                            string to_spaw = scenes[i - 1];
                            target_script.SwapScene(curr_scene, to_spaw);

                            UpdateAvaliableScenesToAdd();
                            UpdateScenesOnBuild();
                        }
                    }

                    if (GUILayout.Button("▼", GUILayout.MaxWidth(25)))
                    {
                        if (i + 1 < scenes.Count)
                        {
                            string to_spaw = scenes[i + 1];
                            target_script.SwapScene(curr_scene, to_spaw);

                            UpdateAvaliableScenesToAdd();
                            UpdateScenesOnBuild();
                        }
                    }

                    if (GUILayout.Button("Find", GUILayout.MaxWidth(40)))
                    {
                        string assets_path = Utils.GetSceneAssetPath(curr_scene);
                        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assets_path);

                        Selection.activeObject = obj;
                        EditorGUIUtility.PingObject(obj);
                    }

                    if (GUILayout.Button("X", GUILayout.MaxWidth(40)))
                    {
                        to_remove.Add(curr_scene);

                        EditorUtility.SetDirty(target_script);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            for(int i = 0; i < to_remove.Count; ++i)
            {
                target_script.RemoveScene(to_remove[i]);
            }

            if(to_remove.Count > 0)
            {
                UpdateScenesOnBuild();
            }

            to_remove.Clear();
        }

        private void UpdateAvaliableScenesToAdd()
        {
            List<string> ret = new List<string>();

            IReadOnlyList<string> scenes_paths = Utils.GetAllSceneAssetsPath();

            List<string> all_scenes = new List<string>();

            for (int i = 0; i < scenes_paths.Count; ++i)
            {
                FileInfo info = new FileInfo(scenes_paths[i]);

                string filename = System.IO.Path.GetFileNameWithoutExtension(info.Name);

                all_scenes.Add(filename);
            }

            for (int i = 0; i < all_scenes.Count; ++i)
            {
                string curr_scene = all_scenes[i];

                bool exists = target_script.SceneAdded(curr_scene);

                if (!exists)
                {
                    ret.Add(curr_scene);
                }
            }

            available_scenes_to_add = ret.ToArray();
        }

        private void UpdateScenesOnBuild()
        {
            Scenes.Utils.AddScenesToBuild(target_script.Scenes);
        }

        //private void DrawProjectScenesGUI()
        //{
        //    EditorGUILayout.LabelField("Project scenes:", Style.BoldTextStyle);

        //    string[] scenes = GetAllAvaliableScenesToAdd();

        //    for (int i = 0; i < scenes.Length; ++i)
        //    {
        //        string curr_scene = scenes[i];

        //        EditorGUILayout.BeginHorizontal();
        //        {
        //            EditorGUILayout.LabelField($"{curr_scene}");

        //            if (GUILayout.Button("Find", GUILayout.MaxWidth(40)))
        //            {
        //                string assets_path = Utils.GetSceneAssetPath(curr_scene);
        //                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assets_path);

        //                Selection.activeObject = obj;
        //                EditorGUIUtility.PingObject(obj);
        //            }

        //            if (GUILayout.Button("Add", GUILayout.MaxWidth(40)))
        //            {
        //                Fast.Scenes.Scene scene = new Fast.Scenes.Scene(curr_scene);
        //                target_script.AddScene(scene);

        //                EditorUtility.SetDirty(target_script);
        //            }
        //        }
        //        EditorGUILayout.EndHorizontal();
        //    }
        //}

        //private void DrawSelectedScenesGUI()
        //{
        //    EditorGUILayout.LabelField("Selected scenes:", Style.BoldTextStyle);

        //    for (int i = 0; i < target_script.Scenes.Count; ++i)
        //    {
        //        Fast.Scenes.Scene curr_scene = target_script.Scenes[i];

        //        EditorGUILayout.BeginHorizontal();
        //        {
        //            EditorGUILayout.LabelField($"{curr_scene.Name}");

        //            if (GUILayout.Button("Find", GUILayout.MaxWidth(40)))
        //            {
        //                string assets_path = Utils.GetSceneAssetPath(curr_scene.Name);
        //                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assets_path);

        //                Selection.activeObject = obj;
        //                EditorGUIUtility.PingObject(obj);
        //            }

        //            if (GUILayout.Button("X", GUILayout.MaxWidth(40)))
        //            {
        //                to_remove.Add(curr_scene);
        //            }
        //        }
        //        EditorGUILayout.EndHorizontal();
        //    }

        //    for (int i = 0; i < to_remove.Count; ++i)
        //    {
        //        target_script.RemoveScene(to_remove[i].Name);
        //    }

        //    if (to_remove.Count > 0)
        //    {
        //        EditorUtility.SetDirty(target_script);
        //    }

        //    to_remove.Clear();
        //}

        //private void DrawUtilsGUI()
        //{
        //    EditorGUILayout.LabelField("Utils:", Style.BoldTextStyle);

        //    if (GUILayout.Button("Remove deleted"))
        //    {
        //        CheckAddedScenes();

        //        EditorUtility.SetDirty(target_script);
        //    }

        //    if (GUILayout.Button("Add selected to build"))
        //    {
        //        Utils.AddScenesToBuild(target_script.Scenes);
        //    }
        //}

        //private void CheckAddedScenes()
        //{
        //    for (int i = 0; i < target_script.Scenes.Count; ++i)
        //    {
        //        Fast.Scenes.Scene curr_scene = target_script.Scenes[i];

        //        bool exists = Utils.SceneExists(curr_scene.Name);

        //        if (!exists)
        //        {
        //            to_remove.Add(curr_scene);
        //        }
        //    }

        //    for (int i = 0; i < to_remove.Count; ++i)
        //    {
        //        target_script.RemoveScene(to_remove[i].Name);
        //    }

        //    to_remove.Clear();
        //}
    }
}

#endif
