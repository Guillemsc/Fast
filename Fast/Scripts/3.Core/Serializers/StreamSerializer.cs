using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.Serializers
{
    class StreamSerializer
    {
        public static void SerializeToPathAsync(string filepath, Stream to_serialize, Action on_success, Action<string> on_fail)
        {
            FileUtils.CreateAllFilepathDirectories(filepath);

            FileStream outputFileStream = new FileStream(filepath, FileMode.Create);
            
            Task task = to_serialize.CopyToAsync(outputFileStream);

            task.ContinueWith(delegate (Task t)
            {
                string error_message = "";
                Exception exception = null;
                bool has_errors = t.HasErrors(out error_message, out exception);

                if (!has_errors)
                {
                    on_success?.Invoke();
                }
                else
                {
                    on_fail?.Invoke(error_message);
                }

                outputFileStream.Dispose();
            });
            
        }
    }
}
