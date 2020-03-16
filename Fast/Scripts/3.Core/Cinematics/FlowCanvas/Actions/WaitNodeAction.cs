using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using System;
using UnityEngine;

namespace Fast.Cinematics
{
    [Name("Wait")]
    [Category("Fast/Cinematics/Actions")]
    [Description("Start timeline")]
    [Color("e6e6e6")]
    public class WaitNodeAction : NewAction
    {
        private ValueInput<float> value = null;

        protected override void RegisterCustomPorts()
        {
            value = AddValueInput<float>("Time");
        }

        public override CinematicAction GetCinematicAction()
        {
            return new WaitAction(value.value);
        }
    }
}
