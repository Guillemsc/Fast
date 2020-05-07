using System;
using UnityEngine;
using UnityEngine.UI;

namespace Fast.UI.Bindings
{
    class ImageBinding : MultipleTargetBinding<Image>
    {
        protected override void SetupTarget(Image target, object value)
        {
            Sprite sprite = value as Sprite;

            if (sprite == null)
            {
                return;
            }

            target.sprite = sprite;
        }
    }
}
