using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object.RequestData
{
    public class EmployerLogin_Request
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string? Device_ID { get; set; }

        public string? Location_ID { get; set; }
    }
}
