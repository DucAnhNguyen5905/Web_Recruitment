using RecruitmentWebFE.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RecruitmentWebFE.Services
{
    public class MasterDataService
    {
        private readonly IConfiguration _configuration;

        public MasterDataService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string BaseUrl => _configuration["ApiSettings:BaseUrl"]!;

        private async Task<List<DropdownItemViewModel>> GetDropdownAsync(string endpoint, string accessToken)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"{BaseUrl}{endpoint}");
            var raw = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return new List<DropdownItemViewModel>();

            return JsonSerializer.Deserialize<List<DropdownItemViewModel>>(raw, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<DropdownItemViewModel>();
        }

        public Task<List<DropdownItemViewModel>> GetContactTypesAsync(string accessToken)
            => GetDropdownAsync("/api/master-data/contact-types", accessToken);

        public Task<List<DropdownItemViewModel>> GetJobPositionsAsync(string accessToken)
            => GetDropdownAsync("/api/master-data/job-positions", accessToken);

        public Task<List<DropdownItemViewModel>> GetJobTypesAsync(string accessToken)
            => GetDropdownAsync("/api/master-data/job-types", accessToken);

        public Task<List<DropdownItemViewModel>> GetJobCategoriesAsync(string accessToken)
            => GetDropdownAsync("/api/master-data/job-categories", accessToken);

        public Task<List<DropdownItemViewModel>> GetCvLanguagesAsync(string accessToken)
            => GetDropdownAsync("/api/master-data/cv-languages", accessToken);

        public Task<List<DropdownItemViewModel>> GetOfficeAddressesAsync(string accessToken)
            => GetDropdownAsync("/api/master-data/office-addresses", accessToken);

        public Task<List<DropdownItemViewModel>> GetJobKeywordsAsync(string accessToken)
            => GetDropdownAsync("/api/master-data/job-keywords", accessToken);
    }
}