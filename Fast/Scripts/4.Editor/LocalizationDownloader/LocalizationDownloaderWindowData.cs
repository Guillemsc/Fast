using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fast.Editor.LocalizationDownloader
{
    [System.Serializable]
    public class LocalizationDownloaderWindowData : Windows.WindowHelperData
    {
        public bool ShowSettings = false;

        public string ClientId = "";
        public string ClientSecret = "";
        public string PathToSave = "";
        public List<LocalizationFile> FilesToDownload = new List<LocalizationFile>();
    }
}
