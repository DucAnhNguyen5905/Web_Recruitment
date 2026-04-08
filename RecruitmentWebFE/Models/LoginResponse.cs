namespace RecruitmentWebFE.Models
{
    public class LoginResponse
    {
        public int employer_ID { get; set; }
        public string name { get; set; }
        public string Email { get; set; }
        public string token { get; set; }
        public DateTime expiration { get; set; }
        public int responseCode { get; set; }
        public string responseMessage { get; set; }
    }

}
