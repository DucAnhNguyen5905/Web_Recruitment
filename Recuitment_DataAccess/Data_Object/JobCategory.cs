using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("JobCategory")]
    public class JobCategory
    {
        [Key]
        public int Job_Category_ID { get; set; }
        public string? Job_Category_Name { get; set; }
        public string? Isparent { get; set; }


    }
}
