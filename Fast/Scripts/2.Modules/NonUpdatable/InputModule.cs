using System;
using UnityEngine;

namespace Fast.Modules
{
    public class InputModule : Fast.Modules.Module
    {
        public Vector3 MousePosition => Input.mousePosition;
    }
}
