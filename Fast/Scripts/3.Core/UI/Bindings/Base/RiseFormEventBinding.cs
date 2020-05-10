using System;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public class RiseFormEventBinding : BaseBinding
    {
        [SerializeField] private string event_name = "";

        public string EventName
        {
            get { return event_name; }
            set { event_name = value; }
        }

        public override void OnValueRised(object value)
        {
            if(BindedForm == null)
            {
                return;
            }

            BindedForm.OnEventRised(event_name);
        }
    }
}
