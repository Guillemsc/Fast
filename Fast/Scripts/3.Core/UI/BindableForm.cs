using System;
using System.Collections.Generic;
using UnityEngine;
using Fast.UI.Bindings;

namespace Fast.UI
{
    /// <summary>
    /// Represents a piece of UI that is has some independent functionality
    /// </summary>
    [Sirenix.OdinInspector.HideMonoScript]
    public abstract class BindableForm : Form
    {
        private List<BaseBinding> bindings = new List<BaseBinding>();

        [BindingProperty] public static string OnFormAwake;
        [BindingProperty] public static string OnFormShow;
        [BindingProperty] public static string OnFormHide;

        private void Start()
        {
            RiseProperty(nameof(OnFormAwake), OnFormAwake);
        }

        private void OnDestroy()
        {
            RemoveFromAllBindings();
        }

        public override void Show()
        {
            RiseProperty(nameof(OnFormShow), OnFormShow);

            base.Show();
        }

        public override void Hide()
        {
            RiseProperty(nameof(OnFormHide), OnFormShow);

            base.Hide();
        }

        public void AddBinding(BaseBinding binding)
        {
            if(binding == null)
            {
                return;
            }

            RemoveBinding(binding);

            bindings.Add(binding);
        }

        public void RemoveBinding(BaseBinding binding)
        {
            if (binding == null)
            {
                return;
            }

            bindings.Remove(binding);
        }

        private void RemoveFromAllBindings()
        {
            for(int i = 0; i < bindings.Count; ++i)
            {
                bindings[i].BindedForm = null;
            }

            bindings.Clear();
        }

        public void RiseProperty(string parameter_name, object variable = null)
        {
            for(int i = 0; i < bindings.Count; ++i)
            {
                BaseBinding curr_binding = bindings[i];

                if(curr_binding.BindingName != parameter_name)
                {
                    continue;
                }

                curr_binding.RiseValue(variable);
            }
        }

        public virtual void OnEventRised(string event_name)
        {

        }
    }
}
