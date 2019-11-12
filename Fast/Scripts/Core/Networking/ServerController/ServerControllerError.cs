using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public enum ServerControllerError
    {
        EMPTY,
    }

    public class ServerControllerErrorToString
    {
        public static string Get(ServerControllerError error)
        {
            string ret = "";

            switch (error)
            {
                case ServerControllerError.EMPTY:
                    {
                        break;
                    }
            }

            return ret;
        }
    }
}
