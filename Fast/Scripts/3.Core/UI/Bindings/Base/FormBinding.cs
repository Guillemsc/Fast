using System;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public class FormBinding : BaseBinding
    {
        [SerializeField] private BindableForm target = null;

        public override void OnValueRised(object value)
        {
            if(target == null)
            {
                return;
            }

            target.OnValueRised(value);
        }
    }
}
