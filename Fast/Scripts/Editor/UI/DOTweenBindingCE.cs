#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using DG.Tweening;

namespace Fast.Editor.UI
{
    [CustomEditor(typeof(Fast.UI.Bindings.DOTweenBinding), true)]
    [Sirenix.OdinInspector.HideMonoScript]
    class DOTweenBindingCE : FormBindingCE 
    {
        private Fast.UI.Bindings.DOTweenBinding target_script_inherited = null;

        private new void OnEnable()
        {
            base.OnEnable();

            target_script_inherited = (Fast.UI.Bindings.DOTweenBinding)target;
        }

        protected override void OnDrawInspectorGUI()
        {
            base.OnDrawInspectorGUI();

            DrawOtherPropertiesGUI();
        }

        private void DrawOtherPropertiesGUI()
        {
            target_script_inherited.Duration = EditorGUILayout.FloatField("Duration", target_script_inherited.Duration);

            target_script_inherited.UseCustomEasing = EditorGUILayout.Toggle("Custom easing", target_script_inherited.UseCustomEasing);

            if(!target_script_inherited.UseCustomEasing)
            {
                target_script_inherited.Easing = (Ease)EditorGUILayout.EnumPopup("Easing", target_script_inherited.Easing);
            }
            else
            {
                target_script_inherited.CustomEasing = EditorGUILayout.CurveField("Easing", target_script_inherited.CustomEasing);
            }

            target_script_inherited.UseStartingValue = EditorGUILayout.Toggle("Use starting value", target_script_inherited.UseStartingValue);
        }
    }
}


#endif