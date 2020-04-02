using System;
using UnityEngine;

namespace Fast.References
{
    public class FormReference : Fast.References.Reference
    {
        [SerializeField] private Fast.UI.Form form = null;

        public Fast.UI.Form Data => form;
    }
}
