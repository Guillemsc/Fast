using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public enum DatabaseActionTypes
    {
        DB_Null,
        DB_RegisterUser,
        DB_ValidUser,
        DB_InitializeTable,
        DB_AddColumn,
    }
}
