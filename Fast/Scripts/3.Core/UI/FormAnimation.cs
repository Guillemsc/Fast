using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI
{
    /// <summary>
    /// [For inheritance] Animation used in conjunction with a UI form, to automate UI animations
    /// </summary>
    public abstract class FormAnimation : MonoBehaviour
    {
        [Sirenix.OdinInspector.ShowInInspector]
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.Title("Animation name", "(This name is set on the constructor of " +
            "the animation and can't be set here)")]
        [Sirenix.OdinInspector.ReadOnly]
        private string animation_name = "";

        private bool started = false;
        private bool finished = false;

        private bool force_starting_values = false;

        private Fast.Time.TimeContext time_context = null;

        private Callback on_finish = new Callback();

        /// <param name="animation_name"> A unique name for the animation.</param>
        public FormAnimation(string animation_name)
        {
            this.animation_name = animation_name;
        }

        /// <summary>
        /// [Internal, don't use] The unique name set to the animation.
        /// </summary>
        public string AnimationName => animation_name;

        /// <summary>
        /// [Internal, don't use] Defines if the animation must use it's starting values once the animation is started.
        /// </summary>
        public bool ForceStartingValues
        {
            get { return force_starting_values; }
            set { force_starting_values = value; }
        }

        public Fast.Time.TimeContext TimeContext => time_context;

        /// <summary>
        /// [Internal, don't use] Starts the animation on the forward direction, and marks the animation 
        /// as started. Calls OnAnimateForwardInternal().
        /// </summary>
        public void AnimateForward(Fast.Time.TimeContext time_context)
        {
            if (time_context == null)
            {
                return;
            }

            if (!started)
            {
                started = true;

                this.time_context = time_context;
                this.time_context.OnTimeScaleChanged.Subscribe(TimeScaleChangedInternal);

                AnimateForwardInternal(time_context.TimeScale);
            }
        }

        /// <summary>
        /// Starts the animation on the backwards direction, and marks the animation 
        /// as started. Calls OnAnimateBackwardInternal().
        /// </summary>
        public void AnimateBackward(Fast.Time.TimeContext time_context)
        {
            if(time_context == null)
            {
                return;
            }

            if (!started)
            {
                started = true;

                this.time_context = time_context;
                this.time_context.OnTimeScaleChanged.Subscribe(TimeScaleChangedInternal);

                AnimateBackwardInternal(this.time_context.TimeScale);
            }
        }

        /// <summary>
        /// Function that needs to be called to mark the end of the animation. Calls OnFinishInternal().
        /// </summary>
        public void Finish()
        {
            if (started)
            {
                OnFinishInternal();

                on_finish.Invoke();
                on_finish.UnSubscribeAll();

                time_context.OnTimeScaleChanged.UnSubscribe(TimeScaleChangedInternal);

                started = false;
            }
        }

        /// <summary>
        /// Invoked at the end of the animation.
        /// </summary>
        public Callback OnFinish
        {
            get { return on_finish; }
        }

        protected virtual void TimeScaleChangedInternal(float time_scale)
        {

        }

        /// <summary>
        /// Called once the animation is started in the forward direction.
        /// </summary>
        protected virtual void AnimateForwardInternal(float time_scale)
        {

        }

        /// <summary>
        /// Called once the animation is started in the backwards direction.
        /// </summary>
        protected virtual void AnimateBackwardInternal(float time_scale)
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
