using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("Status")]
    public class Status
    {
        [Key]
        public int Status_ID { get; set; }
        public string? Status_Name { get; set; }
    }
}
