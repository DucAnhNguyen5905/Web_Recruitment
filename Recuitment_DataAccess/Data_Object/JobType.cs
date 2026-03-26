using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("JobType")]
    public class JobType
    {
        [Key]
        public int Job_Type_ID { get; set; }
        public string? Job_Type { get; set; }
    }
}
