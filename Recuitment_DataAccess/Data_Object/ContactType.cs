using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("ContactType")]
    public class ContactType
    {
        [Key]
        public int Contact_type_id { get; set; }
        public string? Contact_type { get; set; }

    }
}
