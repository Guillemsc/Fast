using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Parsers
{
    class ByteParser
    {
        public static T ParseObject<T>(byte[] data)
        {
            T ret = default(T);

            using (var memStream = new MemoryStream(data))
            {
                var binForm = new BinaryFormatter();

                object obj = binForm.Deserialize(memStream);

                ret = (T)obj;
            }

            return ret;
        }

        public static byte[] ComposeObject(object obj)
        {
            byte[] ret = new byte[0];

            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);

                ret =  ms.ToArray();
            }

            return ret;
        }
    }
}
