using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    public class User_Sessions
    {
        public int Employer_ID { get; set; }
        public string token { get; set; } 

        public string device_id { get; set; }

        public DateTime exprired_time { get; set; }
    }
}
