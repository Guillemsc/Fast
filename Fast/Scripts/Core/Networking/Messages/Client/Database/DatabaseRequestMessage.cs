using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Fast.Networking
{
    [System.Serializable]
    public class DatabaseRequestMessage : ServerControllerMessage
    {
        private Dictionary<string, object> parameters;
        private DatabaseActionTypes type;

        public DatabaseRequestMessage(DatabaseActionTypes type, Dictionary<string,object> parameters) : base(ServerControllerMessageType.DATABASE_REQUEST)
        {
            this.parameters = parameters;
            this.type = type;
        }

        public Dictionary<string,object> Parameters
        {
            get { return parameters; }
        }

        public DatabaseActionTypes Type
        {
            get { return type; }
        }

    }
}
