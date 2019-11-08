using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI
{
    public class FormAnimation : MonoBehaviour
    {
        [Sirenix.OdinInspector.ShowInInspector]
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.Title("Animation name")]
        [Sirenix.OdinInspector.ReadOnly]
        private string animation_name = "";

        private bool running = false;
        private bool force_starting_values = false;

        private Callback on_finish = new Callback();

        public FormAnimation(string animation_name)
        {
            this.animation_name = animation_name;
        }

        public string AnimationName
        {
            get { return animation_name; }
        }

        public bool ForceStartingValues
        {
            get { return force_starting_values; }
            set { force_starting_values = value; }
        }

        public void AnimateForward()
        {
            if (!running)
            {
                running = true;

                OnAnimateForwardInternal();
            }
        }

        public void AnimateBackward()
        {
            if (!running)
            {
                running = true;

                OnAnimateBackwardInternal();
            }
        }

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

        public Callback OnFinish
        {
            get { return on_finish; }
        }

        protected virtual void OnAnimateForwardInternal()
        {

        }

        protected virtual void OnAnimateBackwardInternal()
        {

        }

        protected virtual void OnFinishInternal()
        {

        }
    }
}
