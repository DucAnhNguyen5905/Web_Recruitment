using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    public class ReturnData_Employer
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
    }

    public class InsertReturnData_Employer : ReturnData_Employer
    {
        public int Employer_ID { get; set; }
    }

    public class LoginReturnData_Employer : ReturnData_Employer
    {
        public int Employer_ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}

// This class is used to encapsulate the response data for employer-related operations.