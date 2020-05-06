#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using DG.Tweening;

namespace Fast.Editor.UI
{
    [CustomEditor(typeof(Fast.UI.Bindings.DOTweenBinding), true)]
    [Sirenix.OdinInspector.HideMonoScript]
    class DOTweenBindingCE : FormBindingCE
    {
        private Fast.UI.Bindings.DOTweenBinding target_script = null;

        private new void OnEnable()
        {
            base.OnEnable();

            target_script = (Fast.UI.Bindings.DOTweenBinding)target;
        }

        protected override void OnDrawInspectorGUI()
        {
            base.OnDrawInspectorGUI();

            DrawOtherPropertiesGUI();
        }

        private void DrawOtherPropertiesGUI()
        {
            target_script.Duration = EditorGUILayout.FloatField("Duration", target_script.Duration);

            target_script.UseCustomEasing = EditorGUILayout.Toggle("Custom easing", target_script.UseCustomEasing);

            if(!target_script.UseCustomEasing)
            {
                target_script.Easing = (Ease)EditorGUILayout.EnumPopup("Easing", target_script.Easing);
            }
            else
            {
                target_script.CustomEasing = EditorGUILayout.CurveField("Easing", target_script.CustomEasing);
            }

            target_script.UseStartingValue = EditorGUILayout.Toggle("Use starting value", target_script.UseStartingValue);
        }
    }
}


#endif