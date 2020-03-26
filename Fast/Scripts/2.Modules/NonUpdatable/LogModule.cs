using System;
using System.Diagnostics;
using UnityEngine;

namespace Fast
{
    public enum LogType
    {
        ERROR,
        WARNING,
        INFO,
    }
}

namespace Fast.Modules
{
    public class LogModule : Module
    {
        private bool terminated = false;

        public override void Start()
        {
            Application.logMessageReceived += LogMessageReceived;
        }

        private LogType UnityLogTypeToFastLogType(UnityEngine.LogType type)
        {
            LogType ret = default;

            switch(type)
            {
                case UnityEngine.LogType.Error:
                case UnityEngine.LogType.Exception:
                case UnityEngine.LogType.Assert:
                    {
                        ret = LogType.ERROR;

                        break;
                    }

                case UnityEngine.LogType.Warning:
                    {
                        ret = LogType.WARNING;

                        break;
                    }

                case UnityEngine.LogType.Log:
                    {
                        ret = LogType.INFO;

                        break;
                    }
            }

            return ret;
        }

        public string GetLog(LogType type, object context, string error)
        {
            string ret = "";

            if (context != null)
            {
                ret = $"[{type}]: {context.GetType().Name} -> {error}";
            }
            else
            {
                ret = $"[{type}]: {error}";
            }

            return ret;
        }

        public void LogMessageReceived(string condition, string stackTrace, UnityEngine.LogType type)
        {
            LogType log_type = UnityLogTypeToFastLogType(type);

            if (log_type == LogType.ERROR)
            {
                MessageWindowError($"{condition}\n\nStackTrace: {stackTrace}");
            }
        }

        public void Log(LogType type, object context, string error)
        {
            if (FastService.Instance.ApplicationMode == ApplicationMode.DEBUG)
            {
                string full_log = GetLog(type, context, error);

                switch (type)
                {
                    case LogType.ERROR:
                        {
                            UnityEngine.Debug.LogError(full_log);
                            break;
                        }

                    case LogType.WARNING:
                        {
                            UnityEngine.Debug.LogWarning(full_log);
                            break;
                        }

                    case LogType.INFO:
                        {
                            UnityEngine.Debug.Log(full_log);
                            break;
                        }
                }
            }
        }

        public void LogError(string error)
        {
            Log(LogType.ERROR, null, error);
        }

        public void LogError(object context, string error)
        {
            Log(LogType.ERROR, context, error);
        }

        public void LogWarning(string warning)
        {
            Log(LogType.WARNING, null, warning);
        }

        public void LogWarning(object context, string warning)
        {
            Log(LogType.WARNING, context, warning);
        }

        public void LogInfo(string info)
        {
            Log(LogType.INFO, null, info);
        }

        public void LogInfo(object context, string info)
        {
            Log(LogType.INFO, context, info);
        }

        public void MessageWindowLog(LogType type, object context, string error)
        {

#if UNITY_EDITOR

            if(terminated)
            {
                return;
            }

            if (!FastService.MPlatform.IsPlaying)
            {
                return;
            }

            if(FastService.Instance.ApplicationMode != ApplicationMode.DEBUG)
            {
                return;
            }
            
            string full_log = GetLog(type, context, error);

            full_log += "\n\n- Ignore to continue with the execution" +
                          "\n- Break to attach the debugger" +
                          "\n- Terminate to finish execution (won't pop more error windows)";

            int selection = UnityEditor.EditorUtility.DisplayDialogComplex(type.ToString(), full_log, "Ignore", "Terminate", "Break");

            switch (selection)
            {
                case 0:
                    {
                        // Do nothing

                        break;
                    }

                case 1:
                    {
                        terminated = true;

                        FastService.MApplication.Quit();

                        break;
                    }

                case 2:
                    {
                        Debugger.Break();

                        break;
                    }
            }
#endif

        }

        public void MessageWindowError(string error)
        {
            MessageWindowLog(LogType.ERROR, null, error);
        }

        public void MessageWindowError(object context, string error)
        {
            MessageWindowLog(LogType.ERROR, context, error);
        }

        public void MessageWindowWarning(string error)
        {
            MessageWindowLog(LogType.WARNING, null, error);
        }

        public void MessageWindowWarning(object context, string error)
        {
            MessageWindowLog(LogType.WARNING, context, error);
        }

        public void MessageWindowInfo(string error)
        {
            MessageWindowLog(LogType.INFO, null, error);
        }

        public void MessageWindowInfo(object context, string error)
        {
            MessageWindowLog(LogType.INFO, context, error);
        }
    }
}
