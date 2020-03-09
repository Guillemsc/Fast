using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.Modules
{
    public class ApplicationModule : Module
    {
        private int sleep_timeout_time = 30;

        public ApplicationModule(FastService fast) : base(fast)
        {
            
        }

        public bool VSync
        {
            get { return QualitySettings.vSyncCount == 1; }
            set { QualitySettings.vSyncCount = (value == true ? 1 : 0); }
        }

        public int MaxFps
        {
            get { return Application.targetFrameRate; }
            set { Application.targetFrameRate = value; }
        }

        public bool RunInBackground
        {
            get { return Application.runInBackground; }
            set { Application.runInBackground = value; }
        }

        public bool NeverSleep
        {
            get { return Screen.sleepTimeout == SleepTimeout.NeverSleep; }
            set { Screen.sleepTimeout = (value == true ? SleepTimeout.NeverSleep : sleep_timeout_time); }
        }

        public int SleepTimeoutTime
        {
            get { return sleep_timeout_time; }
            set
            {
                sleep_timeout_time = value;

                if(Screen.sleepTimeout != SleepTimeout.NeverSleep)
                {
                    Screen.sleepTimeout = sleep_timeout_time;
                }
            }
        }

        public bool FullScreen
        {
            get { return Screen.fullScreen; }
            set { Screen.fullScreen = value; }
        }

        public FullScreenMode FullScreenMode
        {
            get { return Screen.fullScreenMode; }
            set { Screen.fullScreenMode = value; }
        }

        public SystemLanguage SystemLanguage
        {
            get { return Application.systemLanguage; }
        }

        public void Quit()
        {

#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;
#else

            Application.Quit(1);

#endif

        }
    }
}
