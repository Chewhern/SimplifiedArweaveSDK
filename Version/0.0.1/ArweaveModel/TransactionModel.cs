using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedArweaveSDK.ArweaveModel
{
    public class TransactionModel
    {
        public int format { get; set; }

        public String id { get; set; }

        public String last_tx { get; set; }
        
        public String owner { get; set; }

        public TagsModel[] tags { get; set; }

        public String target { get; set; }

        public String quantity { get; set; }

        public String data_root { get; set; }

        public String data { get; set; }

        public String data_size { get; set; }

        public String reward { get; set; }

        public String signature { get; set; }
    }
}
