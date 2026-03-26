using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    public class Permission
    {
        public int PermissionID { get; set; }
        public int EmployerID { get; set; }
        public int FunctionID { get; set; }
        public int IsView { get; set; }
        public int IsInsert { get; set; }
        public int IsDelete { get; set; }

    }
}
