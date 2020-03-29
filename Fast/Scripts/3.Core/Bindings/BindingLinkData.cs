using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Bindings
{
    [System.Serializable]
    public class BindingLinkData
    {
        private string key = "";
        private string type = null;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
