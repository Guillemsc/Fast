using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Build
{
    class WindowsPlatformBuilder : PlatformBuilder
    {
        public override string PlatformName()
        {
            return "Windows";
        }

        public override bool CanBuild(BuildSettings settings, ref List<string> errors)
        {
            bool ret = true;

            return ret;
        }

        public override bool NeedsToBuild(BuildSettings settings)
        {
            return settings.enable_builds.build_windows;
        }

        public override void BuildSettingsToPlayerSettings(BuildSettings settings)
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x);
        }

        public override UnityEditor.Build.Reporting.BuildReport Build(BuildSettings settings, BuildStatus status, string[] scenes, BuildOptions build_options)
        {
            string custom_path = settings.WindowsSettingsFilePath();

            UnityEditor.Build.Reporting.BuildReport ret = BuildPipeline.BuildPlayer(scenes, custom_path, 
                BuildTarget.StandaloneWindows, build_options);

            if (ret.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                status.windows_build_completed = true;
            }

            return ret;
        }
    }
}

#endif
