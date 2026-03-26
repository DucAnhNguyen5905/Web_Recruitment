using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("JobPosition")]
    public class JobPosition
    {
        [Key]
        public int Job_Position_ID { get; set; }
        public string? Position { get; set; }
    }
}
