using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI
{
    [Sirenix.OdinInspector.HideMonoScript]
    public class Form : MonoBehaviour
    {
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.Title("Parent", "All form UI objects should be placed as childs of the parent")]
        [Sirenix.OdinInspector.Required]
        [Sirenix.OdinInspector.SceneObjectsOnly]
        [SerializeField] private GameObject parent = null;

        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.Title("Default animation", "Animation used in default ocasions")]
        [Sirenix.OdinInspector.Required]
        [Sirenix.OdinInspector.SceneObjectsOnly]
        [SerializeField] private FormAnimation default_animation = null;

        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.Title("Animations", "List of animations that can be used with this form")]
        [Sirenix.OdinInspector.SceneObjectsOnly]
        [SerializeField] private List<FormAnimation> animations = new List<FormAnimation>();

        private bool showing = false;

        public void Awake()
        {
            parent.SetActive(false);

            OnAwakeInternal();
        }

        public GameObject Parent
        {
            get { return parent; }
        }

        public FormAnimation DefaultAnimation
        {
            get { return default_animation; }
        }

        public FormAnimation GetAnimation(string animation_name)
        {
            FormAnimation ret = null;

            for (int i = 0; i < animations.Count; ++i)
            {
                FormAnimation curr_animation = animations[i];

                if (curr_animation.AnimationName == animation_name)
                {
                    ret = curr_animation;
                }
            }

            return ret;
        }

        public void Show()
        {
            if (!showing)
            {
                showing = true;

                OnShowInternal();
            }
        }

        public void Hide()
        {
            if (showing)
            {
                OnShowInternal();

                showing = false;
            }
        }

        virtual protected void OnAwakeInternal()
        {

        }

        virtual protected void OnShowInternal()
        {

        }

        virtual protected void OnHideInternal()
        {

        }
    }
}
