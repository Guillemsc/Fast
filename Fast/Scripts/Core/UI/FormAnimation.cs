using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI
{
    /// <summary>
    /// [For inheritance] Animation used in conjunction with a UI form, to automate UI animations
    /// </summary>
    public class FormAnimation : MonoBehaviour
    {
        [Sirenix.OdinInspector.ShowInInspector]
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.Title("Animation name", "(This name is set on the constructor of " +
            "the animation and can't be set here)")]
        [Sirenix.OdinInspector.ReadOnly]
        private string animation_name = "";

        private bool running = false;
        private bool force_starting_values = false;

        private Callback on_finish = new Callback();

        /// <param name="animation_name"> A unique name for the animation.</param>
        public FormAnimation(string animation_name)
        {
            this.animation_name = animation_name;
        }

        /// <summary>
        /// [Internal, don't use] The unique name set to the animation.
        /// </summary>
        public string AnimationName
        {
            get { return animation_name; }
        }

        /// <summary>
        /// [Internal, don't use] Defines if the animation must use it's starting values once the animation is started.
        /// </summary>
        public bool ForceStartingValues
        {
            get { return force_starting_values; }
            set { force_starting_values = value; }
        }

        /// <summary>
        /// [Internal, don't use] Starts the animation on the forward direction, and marks the animation 
        /// as started. Calls OnAnimateForwardInternal().
        /// </summary>
        public void AnimateForward()
        {
            if (!running)
            {
                running = true;

                OnAnimateForwardInternal();
            }
        }

        /// <summary>
        /// [Internal, don't use] Starts the animation on the backwards direction, and marks the animation 
        /// as started. Calls OnAnimateBackwardInternal().
        /// </summary>
        public void AnimateBackward()
        {
            if (!running)
            {
                running = true;

                OnAnimateBackwardInternal();
            }
        }

        /// <summary>
        /// Function that needs to be called to mark the end of the animation. Calls OnFinishInternal().
        /// </summary>
        public void Finish()
        {
            if (running)
            {
                OnFinishInternal();

                on_finish.Invoke();
                on_finish.UnSubscribeAll();

                running = false;
            }
        }

        /// <summary>
        /// Invoked at the end of the animation.
        /// </summary>
        public Callback OnFinish
        {
            get { return on_finish; }
        }

        /// <summary>
        /// Called once the animation is started in the forward direction.
        /// </summary>
        protected virtual void OnAnimateForwardInternal()
        {

        }

        /// <summary>
        /// Called once the animation is started in the backwards direction.
        /// </summary>
        protected virtual void OnAnimateBackwardInternal()
        {

        }

        /// <summary>
        /// Called once the animation is finished.
        /// </summary>
        protected virtual void OnFinishInternal()
        {

        }
    }
}
