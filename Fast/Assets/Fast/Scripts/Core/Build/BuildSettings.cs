using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Build
{
    [System.Serializable]
    public class BuildSettingsSection
    {
        protected string extension = "";

        public BuildSettingsSection(string extension)
        {
            this.extension = extension;
        }

        public string Extension
        {
            get { return extension; }
        }

        public virtual void Serialize()
        {

        }

        public virtual void DeSerialize()
        {

        }
    }

    [System.Serializable]
    public class BuildSettings
    {
        [System.Serializable]
        public class BasicSettings : BuildSettingsSection
        {
            public BasicSettings() : base("basic_settings")
            {}

            public string game_name = "";
            public string company_name = "";
            public int version = 0;

            public List<string> scenes_to_build = new List<string>();

            public string base_build_folder = "";

            public int build_counter = 0;
        }
        public BasicSettings basic_settings = new BasicSettings();

        public string GetBasePath()
        {
            return basic_settings.base_build_folder + "/v" + basic_settings.version.ToString() + "/";
        }

        [System.Serializable]
        public class AdvancedSettings : BuildSettingsSection
        {
            public AdvancedSettings() : base("advanced_settings")
            { }

            public bool development_build = false;
        }
        public AdvancedSettings advanced_settings = new AdvancedSettings();

        [System.Serializable]
        public class WindowsSettings : BuildSettingsSection
        {
            public WindowsSettings() : base("windows_settings")
            { }
        }
        public WindowsSettings windows_settings = new WindowsSettings();

        public string WindowsSettingsFilePath()
        {
            return GetBasePath() + "Windows" + "/" + basic_settings.build_counter + "/" + basic_settings.game_name + ".exe";
        }

        [System.Serializable]
        public class AndroidSettings : BuildSettingsSection
        {
            public AndroidSettings() : base("android_settings")
            { }

            public string keystore_password = "";
            public string alias_password = "";

            public string package_name = "com.Company.ProductName";
            public bool google_play_ready = false;
        }
        public AndroidSettings android_settings = new AndroidSettings();

        public string AndroidSettingsFilePath()
        {
            return GetBasePath() + "Android" + "/" + basic_settings.build_counter + "_" + basic_settings.game_name + ".apk";
        }

        [System.Serializable]
        public class EnhanceAndroidSettings : BuildSettingsSection
        {
            public EnhanceAndroidSettings() : base("enhance_android_settings")
            { }

            public string enhance_key = "";
            public string enhance_secret = "";

            public bool chartboost = false;
            public string chartboost_app_id = "";
            public string chartboost_app_signature = "";
        }
        public EnhanceAndroidSettings enhance_android_settings = new EnhanceAndroidSettings();

        public string EnhanceAndroidSettingsFilePath()
        {
            return GetBasePath() + "EnhanceAndroid" + "/" + basic_settings.build_counter + "_" + basic_settings.game_name + ".apk";
        }

        [System.Serializable]
        public class EnableBuilds : BuildSettingsSection
        {
            public EnableBuilds() : base("enable_builds")
            {}

            public bool build_windows = false;
            public bool build_android = false;
        }
        public EnableBuilds enable_builds = new EnableBuilds();

        [System.Serializable]
        public class EnalbePostBuilds : BuildSettingsSection
        {
            public EnalbePostBuilds() : base("enable_post_builds")
            { }

            public bool post_build_enhance_android = false;
        }
        public EnalbePostBuilds enable_post_builds = new EnalbePostBuilds();
    }
}
