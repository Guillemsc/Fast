using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Fast.Build
{
    class PostBuildEffector
    {
        protected Callback<string> on_progress_update = new Callback<string>();

        public Callback<string> OnProgressUpdate
        {
            get { return on_progress_update; }
        }

        public virtual string EffectorName()
        {
            return "Undefined";
        }

        public virtual bool CanUse(BuildSettings settings)
        {
            return true;
        }

        public virtual bool CanEffect(BuildSettings settings, ref List<string> errors)
        {
            return false;
        }

        public virtual bool NeedsToEffect(BuildSettings settings)
        {
            return false;
        }

        public virtual void StartEffect(BuildSettings settings, BuildStatus status, Action on_success, Action<List<string>> on_fail)
        {
            
        }
    }
}
