using DG.Tweening;
using FlowCanvas;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;

namespace Fast.Cinematics
{
    [Name("Log Action")]
    [Category("Fast/Cinematics")]
    [Description("Start timeline")]
    [Color("e6e6e6")]
    public class LogAction : Action
    {
        private ValueInput<Fast.LogType> type = null;
        private ValueInput<string> value = null;

        private Sequence seq = null;

        protected override void ActionRegisterPorts()
        {
            type = AddValueInput<Fast.LogType>("Log type");

            value = AddValueInput<string>("To log");
        }

        protected override void ActionStart(FlowCanvas.Flow flow)
        {
            Fast.FastService.MLog.Log(type.value, null, value.value);

            Finish();
        }
    }
}