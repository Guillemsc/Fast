using DG.Tweening;
using FlowCanvas;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;

namespace Fast.Cinematics
{
    [Name("Wait Time Action")]
    [Category("Fast/Cinematics")]
    [Description("Start timeline")]
    [Color("e6e6e6")]
    public class WaitTimeAction : UpdatableAction
    {
        private ValueInput<float> value = null;

        private Fast.Time.Timer timer = null;

        protected override void ActionRegisterPorts()
        {
            value = AddValueInput<float>("Time");
        }

        protected override void ActionStart(FlowCanvas.Flow flow)
        {
            timer = CinematicAsset.TimeContext.GetTimer();

            timer.Start();
        }

        protected override void ActionFinished(bool complete)
        {
            if(complete)
            {
                return;
            }
        }

        protected override void ActionUpdate()
        {
            CheckTime();
        }

        private void CheckTime()
        {
            if(timer == null)
            {
                return;
            }

            if(timer.ReadTime().TotalSeconds >= value.value)
            {
                UnityEngine.Debug.Log($"{timer.ReadTime().TotalSeconds}");

                Finish();
            }
        }
    }
}