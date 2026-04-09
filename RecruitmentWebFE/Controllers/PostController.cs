using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecruitmentWebFE.Models;
using RecruitmentWebFE.Services;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            var accessToken = GetAccessToken();
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return RedirectToAction("Index", "Login");
            }

            // Build dropdown trước để có dữ liệu map text -> id
            model = await BuildDropdownsAsync(model, accessToken);

            // validate model cơ bản trước
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // map OfficeText -> OfficeAddressIds
            model.OfficeAddressIds = MapOfficeTextToIds(model.OfficeText, model.OfficeAddressOptions);

            // map KeywordText -> JobKeywordIds
            model.JobKeywordIds = MapKeywordTextToIds(model.KeywordText, model.JobKeywordOptions);

            // validate sau khi map
            if (!model.OfficeAddressIds.Any())
            {
                ModelState.AddModelError(nameof(model.OfficeText), "Không tìm thấy văn phòng hợp lệ từ nội dung đã nhập.");
            }

            if (!model.JobKeywordIds.Any())
            {
                ModelState.AddModelError(nameof(model.KeywordText), "Không tìm thấy keyword hợp lệ từ nội dung đã nhập.");
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
        private List<string> ParseTextList(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<string>();

            return input
                .Split(new[] { ',', ';', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private List<int> MapOfficeTextToIds(string? officeText, List<SelectListItem> officeOptions)
        {
            var inputs = ParseTextList(officeText);
            if (!inputs.Any())
                return new List<int>();

            var results = new List<int>();

            foreach (var input in inputs)
            {
                var normalizedInput = input.Trim().ToLower();

                var matchedOption = officeOptions.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(x.Text) &&
                    (
                        x.Text.Trim().ToLower().Equals(normalizedInput) ||
                        x.Text.Trim().ToLower().Contains(normalizedInput) ||
                        normalizedInput.Contains(x.Text.Trim().ToLower())
                    ));

                if (matchedOption != null && int.TryParse(matchedOption.Value, out var officeId))
                {
                    results.Add(officeId);
                }
            }

            return results.Distinct().ToList();
        }
        private List<int> MapKeywordTextToIds(string? keywordText, List<SelectListItem> keywordOptions)
        {
            var inputs = ParseTextList(keywordText);
            if (!inputs.Any()) return new List<int>();

            return keywordOptions
                .Where(x => !string.IsNullOrWhiteSpace(x.Text) &&
                            inputs.Any(i => string.Equals(i.Trim(), x.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                .Select(x => int.TryParse(x.Value, out var id) ? id : 0)
                .Where(id => id > 0)
                .Distinct()
                .ToList();
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
                    JobTitle = post.Job_Title
                };

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

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Cập nhật bài đăng thất bại.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var accessToken = GetAccessToken();

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return RedirectToAction("Index", "Login");
            }

            await _postService.DeleteAsync(id, accessToken);
            return RedirectToAction(nameof(Index));
        }
    }
}