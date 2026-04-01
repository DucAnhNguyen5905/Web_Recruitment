using RecruitmentWebFE.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Recuitment_Common;


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
                PostStatus = 0,
                FromDate = new DateTime(2000, 1, 1),
                ToDate = new DateTime(2030, 1, 1)
            };

            var response = await HttpHelper.SendHttpRequestAsync( HttpMethod.Post,
                $"{BaseUrl}/api/jobpost/getAll",
                accessToken,
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
            var response = await HttpHelper.SendHttpRequestAsync(
        HttpMethod.Get,
        $"{BaseUrl}/api/jobpost/{id}",
        accessToken);

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
            var response = await HttpHelper.SendHttpRequestAsync(
                HttpMethod.Put,
                $"{BaseUrl}/api/jobpost/{model.Id}",
                accessToken,
                model);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id, string accessToken)
        {
            var response = await HttpHelper.SendHttpRequestAsync(
                HttpMethod.Delete,
                $"{BaseUrl}/api/jobpost/{id}",
                accessToken);

            return response.IsSuccessStatusCode;
        }
    }
}