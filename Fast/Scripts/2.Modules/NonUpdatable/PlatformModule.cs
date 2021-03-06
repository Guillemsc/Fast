﻿using System;
using UnityEngine;

namespace Fast.Modules
{
    public class PlatformModule : Module
    {
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

        public bool IsPlaying
        {
            get { return Application.isPlaying; }
        }

        public RuntimePlatform UnityPlatform
        {
            get { return Application.platform; }
        }
    }
}
