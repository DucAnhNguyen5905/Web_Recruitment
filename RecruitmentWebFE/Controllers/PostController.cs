using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecruitmentWebFE.Models;
using RecruitmentWebFE.Services;
using Recuitment_Common;
using System.Buffers.Text;
using System.IdentityModel.Tokens.Jwt;


namespace RecruitmentWebFE.Controllers
{
    [Authorize]
    public class PostController : BaseController
    {
        private readonly PostService _postService;
        private readonly MasterDataService _masterDataService;
        private readonly ILogger<PostController> _logger;

        public PostController(
            PostService postService,
            MasterDataService masterDataService,
            ILogger<PostController> logger)
        {
            _postService = postService;
            _masterDataService = masterDataService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var accessToken = GetAccessToken();

                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    return RedirectToAction("Index", "Login");
                }
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(accessToken);
                var validTo = jwt.ValidTo;

                var posts = await _postService.GetAllPostsAsync(accessToken);
                TempData.Remove("ErrorMessage");

                return View(posts ?? new List<PostViewsModel>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải danh sách bài đăng.");
                TempData["ErrorMessage"] = "Không thể tải danh sách bài đăng.";
                return View(new List<PostViewsModel>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var accessToken = GetAccessToken();

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return RedirectToAction("Index", "Login");
            }

            var post = await _postService.GetByIdAsync(id, accessToken);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var accessToken = GetAccessToken();
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return RedirectToAction("Index", "Login");
            }

            var model = new CreatePostViewModel
            {
                ExpiryDate = DateTime.Today.AddDays(30),
                JobStatus = 1
            };

            model = await BuildDropdownsAsync(model, accessToken);
            return View(model);
        }

        private async Task<CreatePostViewModel> BuildDropdownsAsync(CreatePostViewModel model, string accessToken)
        {
            var contactTypes = await _masterDataService.GetContactTypesAsync(accessToken);
            var jobPositions = await _masterDataService.GetJobPositionsAsync(accessToken);
            var jobTypes = await _masterDataService.GetJobTypesAsync(accessToken);
            var jobCategories = await _masterDataService.GetJobCategoriesAsync(accessToken);
            var cvLanguages = await _masterDataService.GetCvLanguagesAsync(accessToken);
            var officeAddresses = await _masterDataService.GetOfficeAddressesAsync(accessToken);
            var jobKeywords = await _masterDataService.GetJobKeywordsAsync(accessToken);

            model.ContactTypeOptions = contactTypes
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            model.JobPositionOptions = jobPositions
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            model.JobTypeOptions = jobTypes
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            model.JobCategoryOptions = jobCategories
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            model.CVLanguageOptions = cvLanguages
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            model.OfficeAddressOptions = officeAddresses
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            model.JobKeywordOptions = jobKeywords
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            return model;
        }
        private async Task<UpdatePostViewModel> BuildDropdownsAsync(UpdatePostViewModel model, string accessToken)
        {
            var contactTypes = await _masterDataService.GetContactTypesAsync(accessToken);
            var jobPositions = await _masterDataService.GetJobPositionsAsync(accessToken);
            var jobTypes = await _masterDataService.GetJobTypesAsync(accessToken);
            var jobCategories = await _masterDataService.GetJobCategoriesAsync(accessToken);
            var cvLanguages = await _masterDataService.GetCvLanguagesAsync(accessToken);
            var officeAddresses = await _masterDataService.GetOfficeAddressesAsync(accessToken);
            var jobKeywords = await _masterDataService.GetJobKeywordsAsync(accessToken);

            model.ContactTypeOptions = contactTypes
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            model.JobPositionOptions = jobPositions
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            model.JobTypeOptions = jobTypes
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            model.JobCategoryOptions = jobCategories
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            model.CVLanguageOptions = cvLanguages
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            model.OfficeAddressOptions = officeAddresses
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            model.JobKeywordOptions = jobKeywords
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            return model;
        }



        private static List<int> ParseIdList(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<int>();
            }

            return input
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => int.TryParse(x, out _))
                .Select(int.Parse)
                .Distinct()
                .ToList();
        }

        private sealed class DropdownData
        {
            public List<SelectListItem> ContactTypeOptions { get; set; } = new();
            public List<SelectListItem> JobPositionOptions { get; set; } = new();
            public List<SelectListItem> JobTypeOptions { get; set; } = new();
            public List<SelectListItem> JobCategoryOptions { get; set; } = new();
            public List<SelectListItem> CVLanguageOptions { get; set; } = new();
            public List<SelectListItem> OfficeAddressOptions { get; set; } = new();
            public List<SelectListItem> JobKeywordOptions { get; set; } = new();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {

            var accessToken = GetAccessToken();
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return RedirectToAction("Index", "Login");
            }

            // Build dropdown  
            model = await BuildDropdownsAsync(model, accessToken);

            if (model.OfficeAddressIds == null || !model.OfficeAddressIds.Any())
            {
                ModelState.AddModelError(nameof(model.OfficeAddressIds), "Vui lòng chọn ít nhất một văn phòng.");
            }

            if (model.JobKeywordIds == null || !model.JobKeywordIds.Any())
            {
                ModelState.AddModelError(nameof(model.JobKeywordIds), "Vui lòng chọn ít nhất một keyword.");
            }

            if (model.OfficeAddressIds == null || !model.OfficeAddressIds.Any())
            {
                ModelState.AddModelError(nameof(model.OfficeAddressIds), "Vui lòng chọn ít nhất một văn phòng.");
            }

            if (model.JobKeywordIds == null || !model.JobKeywordIds.Any())
            {
                ModelState.AddModelError(nameof(model.JobKeywordIds), "Vui lòng chọn ít nhất một keyword.");
            }


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _postService.CreateAsync(model, accessToken);

                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);
                }

                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo bài đăng.");
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi tạo bài đăng.");
                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var accessToken = GetAccessToken();

                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    return RedirectToAction("Index", "Login");
                }

                var post = await _postService.GetByIdAsync(id, accessToken);

                if (post == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy bài đăng.";
                    return RedirectToAction(nameof(Index));
                }

                var model = new UpdatePostViewModel
                {
                    Id = post.Post_ID,
                    JobTitle = post.Job_Title,
                    JobDescription = post.Job_Description,
                    JobRequirements = post.Job_Requirements,
                    SalaryMin = post.Salary_min,
                    SalaryMax = post.Salary_max,
                    ContactType = post.Contact_Type,
                    JobPositionId = post.Job_Position_ID,
                    JobTypeId = post.Job_Type_ID,
                    JobCategoryId = post.Job_Category_ID,
                    CVLanguageId = post.CV_Language_ID,
                    ExpiryDate = post.Expiry_Date,
                    JobStatus = post.PostStatus,
                    OfficeAddressIds = ParseIdList(post.OfficeAddress_IDs),
                    JobKeywordIds = ParseIdList(post.Keyword_IDs)
                };
                model = await BuildDropdownsAsync(model, accessToken);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải form sửa bài đăng {PostId}", id);
                TempData["ErrorMessage"] = "Không thể tải dữ liệu bài đăng.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var accessToken = GetAccessToken();

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return RedirectToAction("Index", "Login");
            }

            var success = await _postService.UpdateAsync(model, accessToken);



            var result = await _postService.UpdateAsync(model, accessToken);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var accessToken = GetAccessToken();
                if (string.IsNullOrWhiteSpace(accessToken))
                    return RedirectToAction("Index", "Login");

                var (success, message) = await _postService.DeleteAsync(id, accessToken);

                TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa bài đăng {PostId}", id);
                TempData["ErrorMessage"] = "Không thể xóa bài đăng.";
                return RedirectToAction(nameof(Index));
            }
        }
    }



}