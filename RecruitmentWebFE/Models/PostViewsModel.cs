namespace RecruitmentWebFE.Models
{
    public class PostViewsModel
    {
        public int Post_ID { get; set; }
        public int Employer_ID { get; set; }
        public string? Job_Title { get; set; }
        public string? Job_Description { get; set; }
        public string? Job_Requirements { get; set; }
        public int Salary_min { get; set; }
        public int Salary_max { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Expiry_Date { get; set; }
        public string? Contact_Type { get; set; }
        public int Job_Position_ID { get; set; }
        public int Job_Type_ID { get; set; }
        public int Job_Category_ID { get; set; }
        public int CV_Language_ID { get; set; }
        public int PostStatus { get; set; }
    }
}
