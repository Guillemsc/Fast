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
        private readonly List<string> to_remove = new List<string>();

        public override void OnInspectorGUI()
        {
            Fast.Cinematics.CinematicAsset asset = (Fast.Cinematics.CinematicAsset)target;

            if(asset == null)
            {
                return;
            }

            if (GUILayout.Button("OPEN CINEMATIC GRAPH"))
            {
                NodeCanvas.Editor.GraphEditor.OpenWindow(asset);
            }

            EditorGUILayout.Separator();

            DrawBindingLinksGUI(asset);
        }

        private void DrawBindingLinksGUI(Fast.Cinematics.CinematicAsset asset)
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

                        asset.BindingLink.AddBindingLink($"binding_{type}", type.ToString());
                    },
                    menu, "New", true);

                    menu.ShowAsContext();
                }
            }
            GUILayout.EndHorizontal();

            to_remove.Clear();

            for (int i = 0; i < asset.BindingLink.Links.Count; ++i)
            {
                Fast.Bindings.BindingLinkData curr_link = asset.BindingLink.Links[i];

                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Key", GUILayout.MaxWidth(30));

                    curr_link.Key = EditorGUILayout.TextField(curr_link.Key, GUILayout.MaxWidth(250));

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
                asset.BindingLink.RemoveBindingLink(to_remove[i]);
            }
        }
    }
}

#endif
