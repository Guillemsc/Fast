using DG.Tweening;
using System;
using UnityEngine;

namespace Fast.UI
{
    [ExecuteAlways]
    public abstract class FormBinding : MonoBehaviour
    {
        [SerializeField] [HideInInspector] private BindableForm binded_form = null;
        [SerializeField] [HideInInspector] private bool bind_automatically = true;
        [SerializeField] [HideInInspector] private bool use_custom_binding_properties = true;
        [SerializeField] [HideInInspector] private string binding_name = "";
        [SerializeField] [HideInInspector] private float delay = 0.0f;

        public BindableForm BindedForm
        {
            get { return binded_form; }
            set { binded_form = value; }
        }

        public bool BindAutomatically
        {
            get { return bind_automatically; }
            set { bind_automatically = value; }
        }

        public bool UseCustomBindingProperties
        {
            get { return use_custom_binding_properties; }
            set { use_custom_binding_properties = value; }
        }

        public string BindingName
        {
            get { return binding_name; } 
            set { binding_name = value; }
        }

        public float Delay
        {
            get { return delay; }
            set
            {
                delay = value;

                if(delay < 0)
                {
                    delay = 0.0f;
                }
            }
        }

        private void Awake()
        {
            CheckForBindableForm(true);
        }

        private void Update()
        {
            EditModeCheckForBindableForm();
        }

        private void OnDestroy()
        {
            Unbind();
        }

        private void EditModeCheckForBindableForm()
        {
            if(Application.isPlaying)
            {
                return;
            }

            CheckForBindableForm();
        }

        private void CheckForBindableForm(bool force_bind_to_form = false)
        {
            if (!force_bind_to_form)
            {
                if (binded_form != null)
                {
                    return;
                }

                if (!bind_automatically)
                {
                    return;
                }
            }

            if (binded_form == null)
            { 
                BindableForm bindable_form = gameObject.GetComponentInParent<BindableForm>();

                binded_form = bindable_form;

                if (bindable_form == null)
                {
                    return;
                }
            }

            Bind(binded_form);
        }

        private void Bind(BindableForm binded_form)
        {
            if(binded_form == null)
            {
                return;
            }

            this.binded_form = binded_form;

            binded_form.AddBinding(this);
        }

        private void Unbind()
        {
            if (binded_form == null)
            {
                return;
            }

            binded_form.RemoveBinding(this);

            binded_form = null;
        }

        public void RiseValue(object value)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.AppendInterval(Delay);

            sequence.AppendCallback(delegate ()
            {
                OnValueRised(value);
            });

            sequence.Play();
        }

        public virtual void OnValueRised(object value)
        {

        }
    }
}
