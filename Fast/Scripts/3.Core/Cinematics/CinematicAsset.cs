using FlowCanvas;
using System;
using UnityEngine;

namespace Fast.Cinematics
{
    [CreateAssetMenu(fileName = "FastCinematic", menuName = "Fast/FastCinematic", order = 1)]
    public class CinematicAsset : ScriptableObject
    {
        [SerializeField] private FlowScript node_canvas_script = null;

        public FlowScript NodeCanvasScript => node_canvas_script;
    }
}
