namespace RecruitmentWebFE.Models
{
    public class CreatePostViewModel
    {
        public string ? JobTitle { get; set; }

        public string? JobDescription { get; set; }

        public string? JobLocation { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }
    }
}
