using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("CVLanguage")]
    public class CVLanguage
    {
        [Key]
        public int CV_Language_ID { get; set; }
        public string? Language { get; set; }
    }
}
