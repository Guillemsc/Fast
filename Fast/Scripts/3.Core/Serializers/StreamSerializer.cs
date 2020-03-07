using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.Serializers
{
    class StreamSerializer
    {
        public static void SerializeToPathAsync(string full_path, Stream to_serialize, Action on_success, Action<string> on_fail)
        {
            FileInfo info = new FileInfo(full_path);
            Directory.CreateDirectory(info.Directory.FullName);

            FileStream outputFileStream = new FileStream(full_path, FileMode.Create);
            
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
