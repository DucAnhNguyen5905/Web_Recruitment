using System.ComponentModel.DataAnnotations;

namespace RecruitmentWebFE.Models
{
    public class UpdatePostViewModel
    {
        [Required]
        public int Id { get; set; }
        public string ? JobTitle { get; set; }

        public string ? JobDescription { get; set; }

        public string? JobLocation { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

    }
}
