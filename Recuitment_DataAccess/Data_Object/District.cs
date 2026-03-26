using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("District")]
    public class District
    {
        [Key]
        public int District_ID { get; set; }
        public string? District_Name { get; set; }
    }
}
