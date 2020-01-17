using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Prefabs.DebugConsole
{
    class DebugConsoleLog
    {
        public DebugConsoleLog(string condition, string stack_trace, UnityEngine.LogType type)
        {
            this.condition = condition;
            this.stack_trace = stack_trace;
            this.type = type;
        }

        private string condition = "";
        private string stack_trace = "";
        private UnityEngine.LogType type = UnityEngine.LogType.Log;

        public string Condition
        {
            get { return condition; }
        }

        public string StackTrace
        {
            get { return stack_trace; }
        }

        public UnityEngine.LogType Type
        {
            get { return type; }
        }
    }
}
