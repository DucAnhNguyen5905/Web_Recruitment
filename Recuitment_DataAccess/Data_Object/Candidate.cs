using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("Candidate")]
    public class Candidate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int candidate_id { get; set; }
        public string? candidate_password { get; set; }
        public string? candidate_name { get; set; }
        public string? career_goals { get; set; }
        public int degree_id { get; set; }
        public int Job_Position_ID { get; set; }
        public string? candidate_skills { get; set; }
        public string? email { get; set; }
    } 
}
