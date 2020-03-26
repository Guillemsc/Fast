#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Fast.Editor.Cinematics
{
    [CustomEditor(typeof(Fast.Cinematics.CinematicAsset))]
    [Sirenix.OdinInspector.HideMonoScript]
    class CinematicAssetCE : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Fast.Cinematics.CinematicAsset asset = (Fast.Cinematics.CinematicAsset)target;

            if (asset.NodeCanvasScript != null)
            {
                if (GUILayout.Button("OPEN CINEMATIC GRAPH"))
                {
                    NodeCanvas.Editor.GraphEditor.OpenWindow(asset.NodeCanvasScript);
                }
            }

            EditorGUILayout.Separator();

            base.OnInspectorGUI();
        }
    }
}

#endif
