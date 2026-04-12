using RecruitmentWebFE.Models;
using Recuitment_Common;
using Recuitment_Model.RequestData;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;


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

        public async Task<(bool Success, string Message)> CreateAsync(CreatePostViewModel model, string accessToken)
        {
            if (model.SalaryMin > model.SalaryMax)
            {
                return (false, "Lương tối thiểu lớn hơn lương tối đa !");
            }

            var request = new JobPostInsert_Request
            {
                Job_Title = model.JobTitle,
                Job_Description = model.JobDescription,
                Job_Requirements = model.JobRequirements,
                Salary_min = model.SalaryMin,
                Salary_max = model.SalaryMax,
                Contact_Type = model.ContactType,
                Job_Position_ID = model.JobPositionId,
                Job_Type_ID = model.JobTypeId,
                Job_Category_ID = model.JobCategoryId,
                CV_Language_ID = model.CVLanguageId,
                Expiry_Date = model.ExpiryDate,
                JobStatus = model.JobStatus,
                Office_List = model.OfficeAddressIds
                    .Select(id => new OfficeItem { OfficeAddress_ID = id })
                    .ToList(),
                Keywords_List = model.JobKeywordIds
                    .Select(id => new KeywordItem { Job_Keywords_ID = id })
                    .ToList()
            };

            var response = await HttpHelper.SendHttpRequestAsync(
                HttpMethod.Post,
                $"{BaseUrl}/api/jobpost/insert",
                accessToken,
                request
            );

            var raw = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return (true, "Tạo bài đăng thành công.");
            }

            try
            {
                using var doc = JsonDocument.Parse(raw);

                if (doc.RootElement.TryGetProperty("error", out var error))
                {
                    return (false, error.GetString() ?? "Tạo bài đăng thất bại.");
                }

                if (doc.RootElement.TryGetProperty("message", out var message))
                {
                    return (false, message.GetString() ?? "Tạo bài đăng thất bại.");
                }
            }
            catch
            {
            }

            return (false, $"Tạo bài đăng thất bại. HTTP {(int)response.StatusCode}");
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