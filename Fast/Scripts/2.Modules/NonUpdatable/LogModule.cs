using System;

namespace Fast.Modules
{
    public enum LogType
    {
        ERROR,
        WARNING,
        INFO,
    }

    public class LogModule : Module
    {
        public LogModule(FastService fast) : base(fast)
        {

        }

        public void Log(LogType type, object context, string error)
        {
            if (FastService.ApplicationMode == ApplicationMode.DEBUG)
            {
                string full_log = "";

                if (context != null)
                {
                    full_log = $"[{type}]: {context.GetType().Name} -> {error}";
                }
                else
                {
                    full_log = $"[{type}]: {error}";
                }

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
    }
}
