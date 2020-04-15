using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Animation2D
{
    [CreateAssetMenu(fileName = "Animation2DPack", menuName = "Fast/Animation2D/Animation2DPack", order = 1)]
    public class Animation2DPackAsset : ScriptableObject
    {
        [SerializeField] private List<Animation2D> animations = new List<Animation2D>();

        public IReadOnlyList<Animation2D> Animations => animations;
    }
}
