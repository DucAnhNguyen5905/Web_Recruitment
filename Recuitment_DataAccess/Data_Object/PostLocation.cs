using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("PostLocation")]
    public class PostLocation
    {
        [Key]
        public int Post_ID { get; set; }

        public int Job_Location_ID { get; set; }
    }
}
