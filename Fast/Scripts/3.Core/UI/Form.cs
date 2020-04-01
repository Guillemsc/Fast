using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI
{
    /// <summary>
    /// Represents a piece of UI that is has some independent functionality
    /// </summary>
    [Sirenix.OdinInspector.HideMonoScript]
    public abstract class Form : MonoBehaviour
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

        private List<FormAnimation> animations = new List<FormAnimation>();

        private bool showing = false;

        public void Awake()
        {
            parent.SetActive(false);

            FindAnimations();

            AwakeInternal();
        }

        /// <summary>
        /// Parent object for all the form UI.
        /// </summary>
        public GameObject Parent => parent;

        /// <summary>
        /// Animation used in default ocasions.
        /// </summary>
        public FormAnimation DefaultAnimation => default_animation;

        private void FindAnimations()
        {
            FormAnimation[] animations_array = gameObject.GetComponents<FormAnimation>();

            animations.AddRange(animations_array);
        }

        /// <summary>
        /// [Internal, don't use] Gets an animation added to this form.
        /// </summary>
        /// <param name="animation_name"> The unique name of the animation.</param>
        public FormAnimation GetAnimation(string animation_name)
        {
            FormAnimation ret = null;

            for (int i = 0; i < animations.Count; ++i)
            {
                FormAnimation curr_animation = animations[i];

                if (curr_animation != null)
                {
                    if (curr_animation.AnimationName == animation_name)
                    {
                        ret = curr_animation;
                    }
                }
                else
                {
                    Debug.LogError("[Fast.Form.GetAnimation] There is a null animation on the form animations list");
                }
            }

            return ret;
        }

        /// <summary>
        /// [Internal, don't use] Calls the virtual method OnShowInternal(), and marks the form as being used.
        /// </summary>
        public void Show()
        {
            if (!showing)
            {
                showing = true;

                ShowInternal();
            }
        }

        /// <summary>
        /// [Internal, don't use] Calls the virtual method OnShowInternal(), and marks the form as not being used.
        /// </summary>
        public void Hide()
        {
            if (showing)
            {
                HideInternal();

                showing = false;
            }
        }

        /// <summary>
        /// Called on the same time as the MonoBehaveour's Awake().
        /// </summary>
        virtual protected void AwakeInternal()
        {

        }

        /// <summary>
        /// Called when the the form is marked as being used.
        /// </summary>
        virtual protected void ShowInternal()
        {

        }

        /// <summary>
        /// Called when the the form stops to be marked as being used.
        /// </summary>
        virtual protected void HideInternal()
        {

        }
    }
}
