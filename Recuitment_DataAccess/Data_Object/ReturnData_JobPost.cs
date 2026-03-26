using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    public class ReturnData_JobPost
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
    }

    public class InsertReturnData : ReturnData_JobPost
    {
        public int Post_ID { get; set; }
    }   
}
