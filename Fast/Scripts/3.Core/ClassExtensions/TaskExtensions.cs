using System.Text;
using System;
using System.Threading.Tasks;

public static class TaskExtensions
{
    public static bool HasErrors(this Task task, out string error_message, out Exception exception)
    {
        bool ret = true;

        error_message = "";
        exception = null;

        if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
        {
            ret = false;
        }
        else
        {
            ret = true;

            if (task.Exception != null)
            {
                exception = task.Exception;
                error_message = task.Exception.Message;
            }
            else
            {
                if (task.IsCanceled)
                {
                    error_message = "Task was canceled";
                }
                else if (task.IsFaulted)
                {
                    error_message = "Task was faulted";
                }
                else
                {
                    error_message = "Task unkown error";
                }
            }
        }

        return ret;
    }

    public static void ExecuteAsync(this Task task)
    {
        task.ContinueWith(
        delegate(Task t)
        {

        });
    }
}

