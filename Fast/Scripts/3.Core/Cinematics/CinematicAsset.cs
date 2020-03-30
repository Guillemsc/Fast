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

        private Fast.Time.TimeContext time_context = null;
        private Fast.Bindings.BindingData binding_data = null;

        public Fast.Time.TimeContext TimeContext => time_context;
        public Fast.Bindings.BindingLink BindingLink => binding_link;
        public Fast.Bindings.BindingData BindingData => binding_data;

        public void Init(Fast.Time.TimeContext time_context, Fast.Bindings.BindingData binding_data)
        {
            this.time_context = time_context;
            this.binding_data = binding_data;
        }
    }
}
