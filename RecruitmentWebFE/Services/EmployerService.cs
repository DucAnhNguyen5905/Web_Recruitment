using RecruitmentWebFE.Models;
using System.Net.Http.Json;

namespace RecruitmentWebFE.Services
{
    public class EmployerService
    {
        private readonly HttpClient _httpClient;
        private const string LoginUrl = "http://localhost:5108/api/employer/login";

        public EmployerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse?> Login(string email, string password)
        {
            var request = new
            {
                email,
                password
            };

            var response = await _httpClient.PostAsJsonAsync(LoginUrl, request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result;
        }
    }
}