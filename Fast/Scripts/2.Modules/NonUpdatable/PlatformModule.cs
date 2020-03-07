using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Fast
{
    public enum Platform
    {
        IOS,
        ANDROID,
        WINDOWS,
        MACOS,
        LINUX,
        WEBGL,
    }
}

namespace Fast.Modules
{
    public class PlatformModule : Module
    {
        public PlatformModule(FastService fast) : base(fast)
        {

        }

        public Platform CurrentPlatform
        {
            get
            {

#if UNITY_IOS

                return Platform.IOS;

#endif

#if UNITY_ANDROID

                return Platform.ANDROID;

#endif

#if UNITY_STANDALONE_WIN	

                return Platform.WINDOWS;

#endif

#if UNITY_STANDALONE_OSX

                return Platform.MACOS;

#endif

#if UNITY_STANDALONE_LINUX

                return Platform.LINUX;

#endif

#if UNITY_WEBGL

                return Platform.WEBGL;

#endif

            }
        }

        public bool IsEditor
        {
            get { return Application.isEditor; }
        }
    }
}
