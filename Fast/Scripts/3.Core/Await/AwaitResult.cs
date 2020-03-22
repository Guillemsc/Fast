using System;
using System.Threading.Tasks;

namespace Fast
{
    public class AwaitResult
    {
        private Task task = null;

        private bool has_errors;
        private Exception exception = null;

        public AwaitResult(Task task)
        {
            this.task = task;
        }

        public Task Task => task;
        public bool HasErrors => has_errors;
        public Exception Exception => exception;

        public void SetException(Exception exception)
        {
            has_errors = true;
            this.exception = exception;
        }
    }
}
