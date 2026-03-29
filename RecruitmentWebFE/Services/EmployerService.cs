using RecruitmentWebFE.Models;
using System.Net.Http.Json;
using Recuitment_Common;
using System.Buffers.Text;
using System.Net.Http;

namespace RecruitmentWebFE.Services
{
    public class EmployerService
    {
        
        private const string LoginUrl = "http://localhost:5108/api/employer";

        public async Task<LoginResponse?> Login(string email, string password)
        {
            var request = new
            {
                email,
                password
            };
            
            var response = await HttpHelper.SendPostAsync(
                    HttpMethod.Post,
                $"{LoginUrl}/login",
                null,
                   request
                );

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result;
        }
    }
}