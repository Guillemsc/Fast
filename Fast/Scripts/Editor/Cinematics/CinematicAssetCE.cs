#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParadoxNotion.Design;

namespace Fast.Editor.Cinematics
{
    [CustomEditor(typeof(Fast.Cinematics.CinematicAsset))]
    [Sirenix.OdinInspector.HideMonoScript]
    class CinematicAssetCE : UnityEditor.Editor
    {
        private Fast.Cinematics.CinematicAsset target_script = null;

        private readonly List<string> to_remove = new List<string>();


        private void OnEnable()
        {
            target_script = (Fast.Cinematics.CinematicAsset)target;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("OPEN CINEMATIC GRAPH"))
            {
                NodeCanvas.Editor.GraphEditor.OpenWindow(target_script);
            }

            EditorGUILayout.Separator();

            DrawBindingLinksGUI();
        }

        private void DrawBindingLinksGUI()
        {
            GUILayout.BeginHorizontal();
            {                
                if (GUILayout.Button("Add binding"))
                {
                    GenericMenu menu = new GenericMenu();

                    GenericMenu prefered_menu = EditorUtils.GetPreferedTypesSelectionMenu(typeof(object), delegate (Type type)
                    {
                        if(type == null)
                        {
                            return;
                        }

                        target_script.BindingLink.AddBindingLink($"binding_{type}", type.ToString());

                        EditorUtility.SetDirty(target_script);
                    },
                    menu, "New", true);

                    menu.ShowAsContext();
                }
            }
            GUILayout.EndHorizontal();

            to_remove.Clear();

            for (int i = 0; i < target_script.BindingLink.Links.Count; ++i)
            {
                Fast.Bindings.BindingLinkData curr_link = target_script.BindingLink.Links[i];

                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Key", GUILayout.MaxWidth(30));

                    EditorGUI.BeginChangeCheck();
                    {
                        curr_link.Key = EditorGUILayout.TextField(curr_link.Key, GUILayout.MaxWidth(250));
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        EditorUtility.SetDirty(target_script);
                    }

                    EditorGUILayout.LabelField($"Type: {curr_link.Type}", GUILayout.MaxWidth(150));

                    if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
                    {
                        to_remove.Add(curr_link.Key);
                    }
                }
                GUILayout.EndHorizontal();
            }

            for(int i = 0; i< to_remove.Count; ++i)
            {
                target_script.BindingLink.RemoveBindingLink(to_remove[i]);
            }

            if(to_remove.Count > 0)
            {
                EditorUtility.SetDirty(target_script);
            }
        }
    }
}

#endif
