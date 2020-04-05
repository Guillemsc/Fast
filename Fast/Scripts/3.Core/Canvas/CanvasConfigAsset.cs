using System;
using UnityEngine;
using UnityEngine.UI;

namespace Fast.Canvas
{
    [CreateAssetMenu(fileName = "CanvasConfig", menuName = "Fast/Canvas/CanvasConfigAsset", order = 1)]
    public class CanvasConfigAsset : ScriptableObject
    {
        [Sirenix.OdinInspector.Title("General")]
        [SerializeField] private RenderMode render_mode = RenderMode.ScreenSpaceCamera;
        [SerializeField] private bool pixel_perfect = false;
        [SerializeField] private int sort_order = 0;
        [SerializeField] private float reference_pixels_per_unit = 100.0f;
        [SerializeField] private CanvasScaler.ScaleMode scale_mode = CanvasScaler.ScaleMode.ConstantPhysicalSize;

        [Sirenix.OdinInspector.Title("Constant pixel size")]
        [SerializeField] private float scale_factor = 1.0f;

        [Sirenix.OdinInspector.Title("Scale with screen size")]
        [SerializeField] private Vector2Int reference_resolution = Vector2Int.zero;
        [SerializeField] private CanvasScaler.ScreenMatchMode screen_match_mode = CanvasScaler.ScreenMatchMode.Expand;
        [Range(0, 1)] [SerializeField] private float match_width_height = 0.0f;

        [Sirenix.OdinInspector.Title("Constant physical size")]
        [SerializeField] private float fallback_screen_dpi = 96;
        [SerializeField] private float default_sprite_dpi = 96;

        public RenderMode RenderMode => render_mode;
        public bool PixelPerfect => pixel_perfect;
        public int SortOrder => sort_order;
        public float ReferencePixelsPerUnit => reference_pixels_per_unit;
        public CanvasScaler.ScaleMode ScaleMode => scale_mode;
        public float ScaleFactor => scale_factor;
        public Vector2Int ReferenceResolution => reference_resolution;
        public CanvasScaler.ScreenMatchMode ScreenMatchMode => screen_match_mode;
        public float MatchWidthHeight => match_width_height;
        public float FallbackScreenDpi => fallback_screen_dpi;
        public float DefaultSpriteDpi => default_sprite_dpi;
    }
}
