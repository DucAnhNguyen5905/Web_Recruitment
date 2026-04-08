using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using RecruitmentWebFE.Models;
using RecruitmentWebFE.Services;

namespace RecruitmentWebFE.Controllers
{
    [Authorize]
    public class PostController : BaseController
    {
        private readonly PostService _postService;
        private readonly ILogger<PostController> _logger;
        public PostController(PostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
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
        public IActionResult Create()
        {
            var model = BuildDropdowns();
            return View(model);
        }
        private CreatePostViewModel BuildDropdowns(CreatePostViewModel model = null)
        {
            model ??= new CreatePostViewModel();

            model.ContactTypeOptions = new List<SelectListItem>
            {
                new("Email", "1"),
                new("Phone", "2")
            };

            model.JobPositionOptions = new List<SelectListItem>
            {
                new("Intern", "1"),
                new("Junior", "2"),
                new("Senior", "3")
            };

            model.JobTypeOptions = new List<SelectListItem>
            {
                new("Full-time", "1"),
                new("Part-time", "2"),
                new("Remote", "3")
            };

            model.JobCategoryOptions = new List<SelectListItem>
            {
                new("IT", "1"),
                new("Marketing", "2"),
                new("Design", "3")
            };

            model.CVLanguageOptions = new List<SelectListItem>
            {
                new("English", "1"),
                new("Vietnamese", "2")
            };

            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = BuildDropdowns(model);
                return View(model);
            }

            try
            {
                var accessToken = GetAccessToken();
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    return RedirectToAction("Index", "Login");
                }

                var success = await _postService.CreateAsync(model, accessToken);

                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Tạo bài đăng thất bại.");
                    return View(model);
                }

                TempData["SuccessMessage"] = "Tạo bài đăng thành công.";
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