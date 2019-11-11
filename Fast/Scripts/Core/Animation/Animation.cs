using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fast
{
    public class Animation 
    {
        private bool force_start_values = false;

        public Animation(bool force_start_values = false)
        {
            this.force_start_values = force_start_values;
        }

        public virtual DG.Tweening.Sequence AnimateForward()
        {
            return null;
        }

        public virtual DG.Tweening.Sequence AnimateBackward()
        {
            return null;
        }

        protected bool ForceStartValues
        {
            get { return force_start_values; }
        }
    }
}
