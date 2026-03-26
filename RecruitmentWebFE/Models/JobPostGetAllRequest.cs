namespace RecruitmentWebFE.Models
{
    public class JobPostGetAllRequest
    {
        public string? Post_ID_List { get; set; }
        public string? Employer_ID_List { get; set; }
        public string? Job_Type_List { get; set; }
        public string? Job_Category_List { get; set; }

        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }

        public int? PostStatus { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
