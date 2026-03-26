using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using RecruitmentWebFE.Models;

namespace RecruitmentWebFE.Services
{
    public class PostService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PostService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private string BaseUrl => _configuration["ApiSettings:BaseUrl"]!;

        public async Task<List<PostViewsModel>> GetAllPostsAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var request = new JobPostGetAllRequest
            {
                Post_ID_List = null,
                Employer_ID_List = null,
                Job_Type_List = null,
                Job_Category_List = null,
                Search = null,
                SortBy = null,
                SortOrder = null,
                PostStatus = null,
                FromDate = null,
                ToDate = null
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"{BaseUrl}/api/jobpost/getall",
                request
            );

            var raw = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API lỗi: {response.StatusCode} - {raw}");
            }

            var result = JsonSerializer.Deserialize<List<PostViewsModel>>(raw, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new List<PostViewsModel>();
        }

        public async Task<PostViewsModel?> GetByIdAsync(int id, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"{BaseUrl}/api/jobpost/{id}");
            var raw = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return JsonSerializer.Deserialize<PostViewsModel>(raw, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<bool> CreateAsync(CreatePostViewModel model, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/api/jobpost", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(UpdatePostViewModel model, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/api/jobpost/{model.Id}", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.DeleteAsync($"{BaseUrl}/api/jobpost/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}