using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public class MultipleGameObjectTargetsBinding : FormBinding
    {
        [SerializeField] private List<GameObject> targets = new List<GameObject>();

        public IReadOnlyList<GameObject> Targets => targets;
    }
}
