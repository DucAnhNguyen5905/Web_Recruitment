namespace RecruitmentWebFE.Models
{
    public class LoginResponse
    {
        public int employer_ID { get; set; }
        public string Email { get; set; }
        public string token { get; set; }
        public string responseMessage { get; set; }
    }

}
