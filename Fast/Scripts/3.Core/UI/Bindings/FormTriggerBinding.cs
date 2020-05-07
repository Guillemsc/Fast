using System;
using UnityEngine;

namespace Fast.UI
{
    public abstract class FormTriggerBinding : BaseBinding
    {
        public override void OnValueRised(object value)
        {
            OnTriggerRised();
        }

        public virtual void OnTriggerRised()
        {

        }
    }
}
