using FlowCanvas;
using NodeCanvas.Framework;
using System;
using UnityEngine;

namespace Fast.Cinematics
{
    [CreateAssetMenu(fileName = "FastCinematic", menuName = "Fast/Cinematics/Cinematic", order = 1)]
    public class CinematicAsset : FlowCanvas.FlowGraph
    {
        [HideInInspector]
        [SerializeField] private Fast.Bindings.BindingLink binding_link = new Bindings.BindingLink();

        private Fast.Bindings.BindingData binding_data = null;

        public Fast.Bindings.BindingLink BindingLink => binding_link;
        public Fast.Bindings.BindingData BindingData => binding_data;

        public void SetBindingData(Fast.Bindings.BindingData binding_data)
        {
            this.binding_data = binding_data;
        }
    }
}
