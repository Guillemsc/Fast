using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public enum DatabaseActionTypes
    {
        NULL,
        REGISTER_USER,
        VALIDATE_USER,
        INITIALIZE_TABLE,
        ADD_COLUMN,
    }
}
