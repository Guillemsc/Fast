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
    public abstract class Form : Fast.Scenes.SceneInstance
    {
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.Title("Parent", "All form UI objects should be placed as childs of the parent")]
        [Sirenix.OdinInspector.Required]
        [Sirenix.OdinInspector.SceneObjectsOnly]
        [SerializeField] private GameObject parent = null;

        [SerializeField] private float start_show_delay = 0.0f;
        [SerializeField] private float end_hide_delay = 0.0f;

        private bool showing = false;

        public float StartShowDelay => start_show_delay;
        public float EndHideDelay => end_hide_delay;

        protected void Awake()
        {
            if(parent == null)
            {
                Fast.FastService.MLog.LogError(this, "UI Form does not have parent reference");

                return;
            }

            AwakeInternal();
        }

        /// <summary>
        /// Parent object for all the form UI.
        /// </summary>
        public GameObject Parent => parent;

        /// <summary>
        /// [Internal, don't use] Calls the virtual method OnShowInternal(), and marks the form as being used.
        /// </summary>
        public virtual void Show()
        {
            if (parent == null)
            {
                return;
            }

            if (!showing)
            {
                showing = true;

                ShowInternal();
            }
        }

        /// <summary>
        /// [Internal, don't use] Calls the virtual method OnShowInternal(), and marks the form as not being used.
        /// </summary>
        public virtual void Hide()
        {
            if (parent == null)
            {
                return;
            }

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
