using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("PostKeyword")]
    public class PostKeyword
    {
        [Key]
        public int PostKeyword_ID { get; set; }

        public int Post_ID { get; set; }

        public int Job_Keywords_ID { get; set; }
    }
}
