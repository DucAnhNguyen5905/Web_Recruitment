using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("JobKeyword")]
    public class JobKeyword
    {
        [Key]
        public int Job_Keywords_ID { get; set; }
        public string? Keywords { get; set; }

    }
}
