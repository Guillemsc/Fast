using System;

namespace Fast
{
    public class Contract
    {
        public static void ThrowException(object obj, string class_name, string error, string info_message = null)
        {
            string log = "";

            if (info_message != null)
            {
                log = FastService.MLog.GetLog(Modules.LogType.ERROR, obj, $"{class_name} {error} -> [{info_message}]");
            }
            else
            {
                log = FastService.MLog.GetLog(Modules.LogType.ERROR, obj, $"{class_name} {error}");
            }

            throw new Exception(log);
        }

        public static void IsNull(object obj, string info_message = null)
        {
            if(obj != null)
            {
                ThrowException(obj, nameof(IsNull), "contract failed, object is not null", info_message);
            }
        }

        public static void IsNotNull(object obj, string info_message = null)
        {
            if (obj == null)
            {
                ThrowException(obj, nameof(IsNotNull), "contract failed, object is null", info_message);
            }
        }

        public static void Positive(double actual, string info_message = null)
        {
            if (actual <= 0)
            {
                ThrowException(actual, nameof(Positive), $"contract failed. Expected > 0, got:{actual}", info_message);
            }
        }

        public static void Positive(int actual, string info_message = null)
        {
            if (actual <= 0)
            {
                ThrowException(actual, nameof(Positive), $"contract failed. Expected > 0, got:{actual}", info_message);
            }
        }

        public static void Positive(decimal actual, string info_message = null)
        {
            if (actual <= 0)
            {
                ThrowException(actual, nameof(Positive), $"contract failed. Expected > 0, got:{actual}", info_message);
            }
        }

        public static void Positive(ulong actual, string info_message = null)
        {
            if (actual <= 0)
            {
                ThrowException(actual, nameof(Positive), $"contract failed. Expected > 0, got:{actual}", info_message);
            }
        }

        public static void Positive(long actual, string info_message = null)
        {
            if (actual <= 0)
            {
                ThrowException(actual, nameof(Positive), $"contract failed. Expected > 0, got:{actual}", info_message);
            }
        }

        public static void Positive(uint actual, string info_message = null)
        {
            if (actual <= 0)
            {
                ThrowException(actual, nameof(Positive), $"contract failed. Expected > 0, got:{actual}", info_message);
            }
        }

        public static void Positive(float actual, string info_message = null)
        {
            if (actual <= 0)
            {
                ThrowException(actual, nameof(Positive), $"contract failed. Expected > 0, got:{actual}", info_message);
            }
        }

        public static void Negative(float actual, string info_message = null)
        {
            if (actual >= 0)
            {
                ThrowException(actual, nameof(Negative), $"contract failed. Expected < 0, got:{actual}", info_message);
            }
        }

        public static void Negative(long actual, string info_message = null)
        {
            if (actual >= 0)
            {
                ThrowException(actual, nameof(Negative), $"contract failed. Expected < 0, got:{actual}", info_message);
            }
        }

        public static void Negative(double actual, string info_message = null)
        {
            if (actual >= 0)
            {
                ThrowException(actual, nameof(Negative), $"contract failed. Expected < 0, got:{actual}", info_message);
            }
        }

        public static void Negative(decimal actual, string info_message = null)
        {
            if (actual >= 0)
            {
                ThrowException(actual, nameof(Negative), $"contract failed. Expected < 0, got:{actual}", info_message);
            }
        }

        public static void Negative(ulong actual, string info_message = null)
        {
            if (actual >= 0)
            {
                ThrowException(actual, nameof(Negative), $"contract failed. Expected < 0, got:{actual}", info_message);
            }
        }

        public static void Negative(int actual, string info_message = null)
        {
            if (actual >= 0)
            {
                ThrowException(actual, nameof(Negative), $"contract failed. Expected < 0, got:{actual}", info_message);
            }
        }

        public static void Negative(uint actual, string info_message = null)
        {
            ThrowException(actual, nameof(Negative), $"contract failed. Expected < 0, got:{actual}", info_message);
        }

        public static void Greater(float arg1, float arg2, string info_message = null)
        {
            ThrowException(nameof(Greater), $"contract failed. Expected {arg1} > {arg2}, which is not true", info_message);
        }

        public static void Greater(double arg1, double arg2, string info_message = null)
        {
            if (arg1 <= arg2)
            {
                ThrowException(nameof(Greater), $"contract failed. Expected {arg1} > {arg2}, which is not true", info_message);
            }
        }

        public static void Greater(decimal arg1, decimal arg2, string info_message = null)
        {
            if (arg1 <= arg2)
            {
                ThrowException(nameof(Greater), $"contract failed. Expected {arg1} > {arg2}, which is not true", info_message);
            }
        }

        public static void Greater(ulong arg1, ulong arg2, string info_message = null)
        {
            if (arg1 <= arg2)
            {
                ThrowException(nameof(Greater), $"contract failed. Expected {arg1} > {arg2}, which is not true", info_message);
            }
        }

        public static void Greater(long arg1, long arg2, string info_message = null)
        {
            if (arg1 <= arg2)
            {
                ThrowException(nameof(Greater), $"contract failed. Expected {arg1} > {arg2}, which is not true", info_message);
            }
        }

        public static void Greater(uint arg1, uint arg2, string info_message = null)
        {
            if (arg1 <= arg2)
            {
                ThrowException(nameof(Greater), $"contract failed. Expected {arg1} > {arg2}, which is not true", info_message);
            }
        }

        public static void Greater(int arg1, int arg2, string info_message = null)
        {
            if (arg1 <= arg2)
            {
                ThrowException(nameof(Greater), $"contract failed. Expected {arg1} > {arg2}, which is not true", info_message);
            }
        }

        public static void Less(double arg1, double arg2, string info_message = null)
        {
            if (arg1 >= arg2)
            {
                ThrowException(nameof(Less), $"contract failed. Expected {arg1} < {arg2}, which is not true", info_message);
            }
        }

        public static void Less(decimal arg1, decimal arg2, string info_message = null)
        {
            if (arg1 >= arg2)
            {
                ThrowException(nameof(Less), $"contract failed. Expected {arg1} < {arg2}, which is not true", info_message);
            }
        }

        public static void Less(float arg1, float arg2, string info_message = null)
        {
            if (arg1 >= arg2)
            {
                ThrowException(nameof(Less), $"contract failed. Expected {arg1} < {arg2}, which is not true", info_message);
            }
        }

        public static void Less(ulong arg1, ulong arg2, string info_message = null)
        {
            if (arg1 >= arg2)
            {
                ThrowException(nameof(Less), $"contract failed. Expected {arg1} < {arg2}, which is not true", info_message);
            }
        }

        public static void Less(long arg1, long arg2, string info_message = null)
        {
            if (arg1 >= arg2)
            {
                ThrowException(nameof(Less), $"contract failed. Expected {arg1} < {arg2}, which is not true", info_message);
            }
        }

        public static void Less(uint arg1, uint arg2, string info_message = null)
        {
            if (arg1 >= arg2)
            {
                ThrowException(nameof(Less), $"contract failed. Expected {arg1} < {arg2}, which is not true", info_message);
            }
        }

        public static void Less(int arg1, int arg2, string info_message = null)
        {
            if (arg1 >= arg2)
            {
                ThrowException(nameof(Less), $"contract failed. Expected {arg1} < {arg2}, which is not true", info_message);
            }
        }
    }
}
