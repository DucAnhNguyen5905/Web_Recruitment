using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_Model.RequestData
{
    public class EmployerInsert_Request
    {
        public int Employer_ID { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? Company_Name { get; set; }
        public string? Company_Description { get; set; }
        public int? Benefits_ID { get; set; }
        public int? EmployerStatus { get; set; }
        public string? Avartar { get; set; }
    }

}
