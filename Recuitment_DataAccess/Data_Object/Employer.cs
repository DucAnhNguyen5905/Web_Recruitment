using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("Employer")]
    public class Employer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employer_ID { get; set; }
        public string? Name { get; set; }
                     
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? Company_Name { get; set; }
        public string? Company_Description { get; set; }
        public DateTime? Created_at { get; set; } = DateTime.Now;

        public int? Benefits_ID { get; set; }


        public string? Avartar { get; set; }

        public int? IsAdmin { get; set; }

        public int EmployerStatus { get; set; }



    }
}
