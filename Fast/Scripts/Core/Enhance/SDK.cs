using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Enhance
{
    public class SDK
    {
        private string id = "";
        private string type_id = "";

        public SDK(string id, string type_id)
        {
            this.id = id;
            this.type_id = type_id;
        }

        public string ID
        {
            get { return id; }
        }

        public string TypeID
        {
            get { return type_id; }
        }

        public virtual string GenerateData()
        {
            return "";
        }
    }
}
