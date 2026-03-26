using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("JobLocation")]
    public class JobLocation
    {
        [Key]
        public int Job_Location_ID { get; set; }
        public string? Office_Name { get; set; }
        public string? Location_Detail { get; set; }
        public int District_ID  { get; set; } 
    }
}
