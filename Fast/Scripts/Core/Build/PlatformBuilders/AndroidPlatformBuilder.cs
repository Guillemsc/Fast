using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Build
{
    class AndroidPlatformBuilder : PlatformBuilder
    {
        public override string PlatformName()
        {
            return "Android";
        }

        public override bool CanBuild(BuildSettings settings, ref List<string> errors)
        {
            bool ret = true;

            return ret;
        }

        public override bool NeedsToBuild(BuildSettings settings)
        {
            return settings.enable_builds.build_android;
        }

        public override void BuildSettingsToPlayerSettings(BuildSettings settings)
        {
            if(settings.android_settings.google_play_ready)
            {
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
            }
            else
            {
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
            }

            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, settings.android_settings.package_name);

            PlayerSettings.Android.bundleVersionCode = settings.basic_settings.version;

            PlayerSettings.Android.keystorePass = settings.android_settings.keystore_password;
            PlayerSettings.keyaliasPass = settings.android_settings.alias_password;
        }

        public override UnityEditor.Build.Reporting.BuildReport Build(BuildSettings settings, BuildStatus status, string[] scenes, BuildOptions build_options)
        {
            string custom_path = settings.AndroidSettingsFilePath();

            UnityEditor.Build.Reporting.BuildReport ret = BuildPipeline.BuildPlayer(scenes, custom_path,
                BuildTarget.Android, build_options);

            if(ret.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                status.android_build_completed = true;
            }

            return ret;
        }
    }
}

#endif
