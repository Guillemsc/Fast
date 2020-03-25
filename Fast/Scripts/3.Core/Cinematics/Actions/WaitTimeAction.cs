using DG.Tweening;
using FlowCanvas;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;

namespace Fast.Cinematics
{
    [Name("Wait Time Action")]
    [Category("Fast/NewCinematics")]
    [Description("Start timeline")]
    [Color("e6e6e6")]
    public class WaitTimeAction : Action, IUpdatable
    {
        private ValueInput<float> value = null;

        private Sequence seq = null;

        protected override void ActionRegisterPorts()
        {
            value = AddValueInput<float>("Time");
        }

        protected override void ActionStart()
        {
            seq = DOTween.Sequence();

            seq.AppendInterval(value.value);

            seq.Play();

            seq.onComplete += (delegate ()
            {
                UnityEngine.Debug.Log($"Finished: {value.value}");

                Finish();
            });
        }

        protected override void ActionFinished(bool complete)
        {
            if(complete)
            {
                return;
            }

            seq.Kill();
        }

        public void Update()
        {

        }
    }
}