using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recuitment_DataAccess.Data_Object
{
    [Table("JobPost")]
    public class JobPost
    {
        [Key]
        public int Post_ID { get; set; }
        public int Employer_ID { get; set; }

        public string? Job_Title { get; set; }
        public string? Job_Description { get; set; }
        public string? Job_Requirements { get; set; }

        public int Salary_min { get; set; }
        public int Salary_max { get; set; }

        public DateTime Created_at { get; set; }
        public DateTime? Expiry_Date { get; set; }

        public string? Contact_Type { get; set; }

        public int Job_Position_ID { get; set; }
        public int Job_Type_ID { get; set; }
        public int Job_Category_ID { get; set; }
        public int CV_Language_ID { get; set; }

        public int PostStatus { get; set; }


        public string? Job_Type { get; set; }
        public string? Job_Category_Name { get; set; }
        public string? Position { get; set; }
        public string? Language { get; set; }
        public string? Keywords { get; set; }
        public string? Locations { get; set; }
        public string? Keyword_IDs { get; set; }
        public string? OfficeAddress_IDs { get; set; }
    }
}