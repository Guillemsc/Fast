using System;
using UnityEngine;

namespace Fast.Layers
{ 
    public class LayerSelector<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] private T layer = default;
        [SerializeField] private Renderer target_renderer = null;

        private void Awake()
        {
            SetLayer();
        }

        private int LayerToInt()
        {
            return Convert.ToInt32(layer);
        }

        private void SetLayer()
        {
            if(target_renderer == null)
            {
                return;
            }

            target_renderer.sortingOrder = LayerToInt();
        }
    }
}
