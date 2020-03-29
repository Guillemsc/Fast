using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using FlowCanvas;
using System;
using UnityEngine;

namespace Fast.Cinematics
{
    [Name("Binding Data")]
    [Category("Fast/Cinematics")]
    [Description("Start timeline")]
    [Color("eb4284")]
    public class BindingData : CinematicsNode
    {
        private ValueInput<string> binding_key_input = null;
        private ValueOutput<object> value_output = null;

        protected override void RegisterPorts()
        {
            binding_key_input = AddValueInput<string>("Binding key");

            value_output = AddValueOutput<object>("Data", OnGetValueOutput);
        }

        private object OnGetValueOutput()
        {
            CinematicAsset graph = base.flowGraph as CinematicAsset;

            if(graph == null)
            {
                return null;
            }

            return graph.BindingData.GetBindingObject<object>(binding_key_input.value);
        }
    }
}
