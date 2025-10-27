using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedArweaveSDK.ArweaveModel
{
    public class TransactionStatusModel
    {
        public long block_height { get; set; }

        public String block_indep_hash { get; set; }

        public long number_of_confirmations { get; set; }
    }
}
