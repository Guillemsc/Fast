using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Tweening
{
    public class Tween
    {
        private bool force_start_values = false;

        public Tween(bool force_start_values = false)
        {
            this.force_start_values = force_start_values;
        }

        protected bool ForceStartValues => force_start_values;

        public virtual void SetStartingValuesForward()
        {

        }

        public virtual void SetStartingValuesBackward()
        {

        }

        public virtual DG.Tweening.Sequence AnimateForward()
        {
            return null;
        }

        public virtual DG.Tweening.Sequence AnimateBackward()
        {
            return null;
        }
    }
}
