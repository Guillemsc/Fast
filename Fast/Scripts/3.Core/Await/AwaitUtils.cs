using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast
{
    public class AwaitUtils
    {
        public static async Task<AwaitResult> AwaitTask(Task task)
        {
            AwaitResult ret = new AwaitResult(task);

            try
            {
                await task;
            }
            catch(Exception ex)
            {
                ret.SetException(ex);
            }

            return ret;
        }
    }
}
