using System;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Build
{
    class PlatformBuilderController
    {
        private List<PlatformBuilder> platform_builders = new List<PlatformBuilder>();
        private List<PostBuildEffector> post_build_effectors = new List<PostBuildEffector>();

        private List<PostBuildEffector> effectors_to_run = new List<PostBuildEffector>();

        private List<string> build_errors = new List<string>();

        private Callback<string> on_progress_update = new Callback<string>();

        public PlatformBuilderController()
        {
            AddPlatformBuilder(new WindowsPlatformBuilder());
            AddPlatformBuilder(new AndroidPlatformBuilder());

            AddPostBuildEffector(new EnhanceAndroidPostBuildEffector());
        }

        private void AddPlatformBuilder(PlatformBuilder builder)
        {
            platform_builders.Add(builder);
        }

        private void AddPostBuildEffector(PostBuildEffector effector)
        {
            post_build_effectors.Add(effector);
        }

        public Callback<string> OnProgressUpdate
        {
            get { return on_progress_update; }
        }

        private bool CanBuildBasic(BuildSettings settings, ref List<string> errors)
        {
            bool ret = true;

            if (string.IsNullOrEmpty(settings.basic_settings.base_build_folder))
            {
                errors.Add("There needs to be a valid base build folder");

                ret = false;
            }

            if (string.IsNullOrEmpty(settings.basic_settings.game_name))
            {
                errors.Add("The game name needs to be set");

                ret = false;
            }

            if (string.IsNullOrEmpty(settings.basic_settings.company_name))
            {
                ret = false;
            }

            return ret;
        }

        private bool CanBuildAll(BuildSettings settings, out List<string> errors_list)
        {
            bool ret = true;

            errors_list = new List<string>();

            ret = CanBuildBasic(settings, ref errors_list);

            if (ret)
            {
                for (int i = 0; i < platform_builders.Count; ++i)
                {
                    PlatformBuilder curr_builder = platform_builders[i];

                    if (curr_builder.NeedsToBuild(settings))
                    {
                        string progress = "Building " + curr_builder.PlatformName();
                        on_progress_update.Invoke(progress);

                        ret = curr_builder.CanBuild(settings, ref errors_list);

                        if (ret == false)
                        {
                            break;
                        }
                    }
                }

                for (int i = 0; i < post_build_effectors.Count; ++i)
                {
                    PostBuildEffector effector = post_build_effectors[i];

                    if (effector.CanUse(settings))
                    {
                        if (effector.NeedsToEffect(settings))
                        {
                            effector.OnProgressUpdate.UnSubscribeAll();
                            effector.OnProgressUpdate.Subscribe(delegate (string progress)
                            {
                                string progress_msg = "Building " + effector.EffectorName() + ": " + progress;
                                on_progress_update.Invoke(progress_msg);
                            });

                            ret = effector.CanEffect(settings, ref errors_list);

                            if (ret == false)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return ret;
        }

        private void BuildSettingsToPlayerSettingsBasic(BuildSettings settings)
        {
            PlayerSettings.productName = settings.basic_settings.game_name;
            PlayerSettings.companyName = settings.basic_settings.company_name;
            PlayerSettings.bundleVersion = settings.basic_settings.version.ToString();
        }

        public void BuildAll(BuildSettings settings, Action on_finish)
        {
            BuildStatus status = new BuildStatus();

            build_errors.Clear();

            bool can_build = CanBuildAll(settings, out build_errors);

            if (can_build)
            {
                ++settings.basic_settings.build_counter;

                BuildSettingsToPlayerSettingsBasic(settings);

                string[] scenes = settings.basic_settings.scenes_to_build.ToArray();

                BuildOptions build_options = new BuildOptions();

                if (settings.advanced_settings.development_build)
                {
                    build_options |= BuildOptions.Development;
                }

                for (int i = 0; i < platform_builders.Count; ++i)
                {
                    PlatformBuilder curr_builder = platform_builders[i];

                    if (curr_builder.NeedsToBuild(settings))
                    {
                        curr_builder.BuildSettingsToPlayerSettings(settings);

                        UnityEditor.Build.Reporting.BuildReport report = curr_builder.Build(settings, status, scenes, build_options);
                    }
                }

                effectors_to_run.AddRange(post_build_effectors);

                RunNextEffector(settings, status, on_finish);
            }
        }

        private void RunNextEffector(BuildSettings settings, BuildStatus status, Action on_finish)
        {
            if (effectors_to_run.Count > 0)
            {
                PostBuildEffector curr_effector = effectors_to_run[0];

                if (curr_effector.NeedsToEffect(settings))
                {
                    curr_effector.StartEffect(settings, status,
                    delegate ()
                    {
                        effectors_to_run.RemoveAt(0);

                        RunNextEffector(settings, status, on_finish);
                    }
                    , delegate(List<string> errors)
                    {
                        effectors_to_run.RemoveAt(0);

                        RunNextEffector(settings, status, on_finish);
                    });
                }
            }
            else
            {
                if (on_finish != null)
                    on_finish.Invoke();
            }
        }
    }
}

#endif
