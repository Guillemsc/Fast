
#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;


namespace Fast.Build
{
    class PlatformBuilder
    {
        public virtual string PlatformName()
        {
            return "Undefined";
        }

        public virtual bool CanBuild(BuildSettings settings, ref List<string> errors)
        {
            return false;
        }

        public virtual bool NeedsToBuild(BuildSettings settings)
        {
            return false;
        }

        public virtual void BuildSettingsToPlayerSettings(BuildSettings settings)
        {

        }

        public virtual UnityEditor.Build.Reporting.BuildReport Build(BuildSettings settings, BuildStatus status, 
            string[] scenes, BuildOptions build_options)
        {
            return null;
        }
    }
}

#endif
