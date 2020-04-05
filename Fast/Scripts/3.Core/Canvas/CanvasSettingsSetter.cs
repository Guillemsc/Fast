using System;
using UnityEngine;

namespace Fast.Canvas
{
    public class CanvasSettingsSetter : MonoBehaviour
    {
        [SerializeField] private CanvasConfigAsset config = null;
        [SerializeField] private UnityEngine.Canvas canvas = null;
        [SerializeField] private UnityEngine.UI.CanvasScaler canvas_scaler = null;

        private void Awake()
        {
            SetConfigData();
        }

        private void SetConfigData()
        {
            if(config == null)
            {
                return;
            }

            if(canvas == null)
            {
                return;
            }

            if(canvas_scaler == null)
            {
                return;
            }

            canvas.renderMode = config.RenderMode;
            canvas.pixelPerfect = config.PixelPerfect;
            canvas.sortingOrder = config.SortOrder;
            canvas.referencePixelsPerUnit = config.ReferencePixelsPerUnit;
            canvas_scaler.uiScaleMode = config.ScaleMode;
            canvas_scaler.scaleFactor = config.ScaleFactor;
            canvas_scaler.referenceResolution = config.ReferenceResolution;
            canvas_scaler.screenMatchMode = config.ScreenMatchMode;
            canvas_scaler.matchWidthOrHeight = config.MatchWidthHeight;
            canvas_scaler.fallbackScreenDPI = config.FallbackScreenDpi;
            canvas_scaler.defaultSpriteDPI = config.DefaultSpriteDpi;
        }
    }
}
