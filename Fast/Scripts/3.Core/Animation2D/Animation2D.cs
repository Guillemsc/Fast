using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Animation2D
{
    [System.Serializable]
    public class Animation2D 
    {
        [Sirenix.OdinInspector.Required]
        [Sirenix.OdinInspector.LabelText("Name")]
        [SerializeField] private string name = "";
        [SerializeField] private bool loop = false;
        [SerializeField] private float playback_speed = 0.0f;
        [SerializeField] private List<Sprite> sprites = null;

        public string Name => name;
        public bool Loop => loop;
        public float PlaybackSpeed => playback_speed;
        public IReadOnlyList<Sprite> Sprites => sprites;
    }
}
