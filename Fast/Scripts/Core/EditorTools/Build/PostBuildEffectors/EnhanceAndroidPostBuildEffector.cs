using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Build
{
    class EnhanceAndroidPostBuildEffector : PostBuildEffector
    {
        private string session_id = "";

        public override string EffectorName()
        {
            return "Enhance Android";
        }

        public override bool CanUse(BuildSettings settings)
        {
            return settings.enable_builds.build_android;
        }

        public override bool NeedsToEffect(BuildSettings settings)
        {
            return settings.enable_post_builds.post_build_enhance_android;
        }

        public override bool CanEffect(BuildSettings settings, ref List<string> errors)
        {
            bool ret = true;

            return ret;
        }

        public override void StartEffect(BuildSettings settings, BuildStatus status, Action on_success, Action<List<string>> on_fail)
        {
            session_id = "";

            if (status.android_build_completed)
            {
                string android_custom_path = settings.AndroidSettingsFilePath();

                System.IO.FileInfo file = new System.IO.FileInfo(settings.EnhanceAndroidSettingsFilePath());
                file.Directory.Create();

                List<Enhance.SDK> sdks = GetSDKs(settings);

                Fast.Enhance.EnhanceApk enhance = new Enhance.EnhanceApk(settings.enhance_android_settings.enhance_key,
                       settings.enhance_android_settings.enhance_secret, android_custom_path, sdks);

                enhance.OnProgressUpdate.Subscribe(delegate (RequestProgress progress)
                {
                    Fast.Enhance.EnhanceApkProgress progress_enum = (Fast.Enhance.EnhanceApkProgress)progress.progress_index;

                    on_progress_update.Invoke(progress_enum.ToString());
                });

                enhance.RunRequest(
                delegate (Enhance.EnhanceApkSuccessObject enhance_success)
                {
                    session_id = enhance_success.session_id;

                    CheckCanDownload(settings,
                    delegate ()
                    {
                        on_progress_update.Invoke("Downloading App");

                        Fast.Enhance.DownloadEnhancedApk download = new Enhance.DownloadEnhancedApk(settings.enhance_android_settings.enhance_key,
                            settings.enhance_android_settings.enhance_secret, session_id, settings.EnhanceAndroidSettingsFilePath());

                        download.RunRequest(delegate (Enhance.DownloadEnhancedApkSuccessObject download_success)
                        {
                            if (on_success != null)
                                on_success.Invoke();
                        }
                        , delegate (Enhance.DownloadEnhancedApkErrorObject download_error)
                        {

                        });
                    });
                }
                , delegate (Enhance.EnhanceApkErrorObject enhance_error)
                {

                });
            }
            else
            {
                List<string> errors = new List<string>();

                if (on_fail != null)
                    on_fail.Invoke(errors);
            }
        }

        private void CheckCanDownload(BuildSettings settings, Action on_finish)
        {
            Fast.Enhance.GetSessionStatus status = new Enhance.GetSessionStatus(settings.enhance_android_settings.enhance_key,
                   settings.enhance_android_settings.enhance_secret, session_id);

            status.RunRequest(
            delegate (Enhance.GetSessionStatusSuccessObject get_session_success)
            {
                if (get_session_success.completed)
                {
                    if (on_finish != null)
                        on_finish.Invoke();
                }
                else
                {
                    CheckCanDownload(settings, on_finish);
                }
            }
            , delegate (Enhance.GetSessionStatusErrorObject get_session_error)
            {
                if (on_finish != null)
                    on_finish.Invoke();
            });
        }

        private List<Enhance.SDK> GetSDKs(BuildSettings settings)
        {
            List<Enhance.SDK> ret = new List<Enhance.SDK>();

            if(settings.enhance_android_settings.chartboost)
            {
                Enhance.ChartBoostSDK.InterstitialAds chartboost = new Enhance.ChartBoostSDK.InterstitialAds(
                    settings.enhance_android_settings.chartboost_app_id, settings.enhance_android_settings.chartboost_app_signature);

                ret.Add(chartboost);
            }

            return ret;
        }
    }
}
