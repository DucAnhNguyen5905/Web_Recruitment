using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    public class EmployerUpdate_Request
    {
        public int Employer_ID { get; set; }
        public string? Name { get; set; }

        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Company_Name { get; set; }
        public string? Company_Description { get; set; }

    }
}
