using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Recruitment_Common;
using Recuitment_DataAccess.Data_Object;
using Recuitment_DataAccess.Data_Object.RequestData;
using Recuitment_DataAccess.IRepository;
using Recuitment_DataAccess.Recuitment_Unitofwork;
using Recuitment_DataAccess.Repository;
using Recuitment_Model.RequestData;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Filter;


namespace Recruitment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class jobpostController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IJobpostingRepositoryDapper _jobpostingRepositoryDapper;
        private readonly IDistributedCache _cache;
        private IConfiguration _configuration;
        private readonly ILogger<JobPostingRepositoryDapper> _logger;

        public jobpostController(IUnitofWork unitofWork, IJobpostingRepositoryDapper jobpostingRepositoryDapper, IDistributedCache cache, IConfiguration configuration, ILogger<JobPostingRepositoryDapper> logger)
        {
            _unitofWork = unitofWork;
            _jobpostingRepositoryDapper = jobpostingRepositoryDapper;
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("/api/jobpost/getAll")]
        [RecuitmentAuthorizeAttribute("Get_All_JobPosts", "ISVIEWS")]
        public async Task<IActionResult> Get_All_JobPosts([FromBody] JobPostGetAll_Request jobPostGetAll_Request)
        {
            try
            {
                var employerIdClaim = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (!int.TryParse(employerIdClaim, out int currentEmployerId))
                {
                    return Unauthorized(new { error = "Token không hợp lệ." });
                }

                var isAdminClaim = User.FindFirst("IsAdmin")?.Value ?? "0";
                bool isAdmin = isAdminClaim == "1";

                // inject dữ liệu từ token xuống request
                jobPostGetAll_Request.CurrentEmployerID = currentEmployerId;
                jobPostGetAll_Request.IsAdmin = isAdmin ? 1 : 0;

                // employer thường không được tự truyền employer list
                if (!isAdmin)
                {
                    jobPostGetAll_Request.Employer_ID_List = null;
                }

                var cacheKey =
                    $"JobPostGetAll_User_{currentEmployerId}_Admin_{jobPostGetAll_Request.IsAdmin}_" +
                    $"{jobPostGetAll_Request.Post_ID_List}_{jobPostGetAll_Request.Job_Type_List}_" +
                    $"{jobPostGetAll_Request.Job_Category_List}_{jobPostGetAll_Request.Search}_" +
                    $"{jobPostGetAll_Request.SortBy}_{jobPostGetAll_Request.SortOrder}_" +
                    $"{jobPostGetAll_Request.PostStatus}_{jobPostGetAll_Request.FromDate}_{jobPostGetAll_Request.ToDate}_" +
                    $"{jobPostGetAll_Request.Employer_ID_List}";

                var cachedData = await _cache.GetStringAsync(cacheKey);
                if (cachedData != null)
                {
                    var cachedResult = System.Text.Json.JsonSerializer.Deserialize<List<JobPost>>(cachedData);
                    return Ok(cachedResult);
                }

                var result = await _jobpostingRepositoryDapper.Get_All_JobPosts(jobPostGetAll_Request);

                if (result == null || result.Count == 0)
                {
                    return Ok(new List<JobPost>());
                }

                var cacheOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(result), cacheOptions);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Lỗi khi lấy danh sách bài đăng công việc: " + ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get_JobPost_ByID(int id)
        {
            var result = await _jobpostingRepositoryDapper.Get_JobPost_By_Id(new JobPostGetById_Request { Post_ID = id });
            var post = result?.FirstOrDefault();
            if (post == null)
            {
                return NotFound(new { error = "Post not found !!!." });
            }
            

            var employerIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.PrimarySid)?.Value;
            var isAdminClaim = User.FindFirst("IsAdmin")?.Value ?? "0";

            if (!int.TryParse(employerIdClaim, out int currentEmployerId))
            {
                return Unauthorized(new { error = "Token không hợp lệ." });
            }

            bool isAdmin = isAdminClaim == "1";

            if (!isAdmin && post.Employer_ID != currentEmployerId)
            {
                return Forbid();
            }

            return Ok(post);
        }

        [Authorize]
        [HttpPost("/api/jobpost/insert")]
        public async Task<IActionResult> Insert_JobPost([FromBody] JobPostInsert_Request jobPostInsert_Request)
        {
            try
            {
                var employerIdClaim = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (!int.TryParse(employerIdClaim, out int employerId))
                {
                    return Unauthorized(new { error = "Token không hợp lệ." });
                }

                jobPostInsert_Request.Employer_ID = employerId;

                Validate_Data.Validate_Data_JobPostInsert(jobPostInsert_Request);

                var result = await _jobpostingRepositoryDapper.Jobposting_Insert(jobPostInsert_Request);

                return result switch
                {
                    1 => Ok(new { message = "Đăng bài thành công." }),
                    -9999 => StatusCode(500, new { error = "Lỗi khi tạo bài đăng từ store procedure." }),
                    _ => BadRequest(new { error = $"Tạo bài đăng thất bại. ResponseCode = {result}" })
                };
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [Authorize]
        [HttpPut("/api/jobpost")]
        public async Task<IActionResult> Update_JobPost([FromBody] JobPostUpdate_Request jobPostUpdate_Request)
        {
            try
            {
                var employerIdClaim = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (!int.TryParse(employerIdClaim, out int employerId))
                {
                    return Unauthorized(new { error = "Token không hợp lệ." });
                }

                jobPostUpdate_Request.Employer_ID = employerId;

                if (jobPostUpdate_Request.Post_ID <= 0)
                {
                    return BadRequest(new { error = "Post_ID không hợp lệ." });
                }

                if (string.IsNullOrWhiteSpace(jobPostUpdate_Request.Job_Title))
                {
                    return BadRequest(new { error = "Tiêu đề công việc không được để trống." });
                }

                if (jobPostUpdate_Request.Office_List == null || !jobPostUpdate_Request.Office_List.Any())
                {
                    return BadRequest(new { error = "Vui lòng chọn ít nhất một văn phòng." });
                }

                if (jobPostUpdate_Request.Keywords_List == null || !jobPostUpdate_Request.Keywords_List.Any())
                {
                    return BadRequest(new { error = "Vui lòng chọn ít nhất một keyword." });
                }

                var existing = await _jobpostingRepositoryDapper.Get_JobPost_By_Id(
                    new JobPostGetById_Request { Post_ID = jobPostUpdate_Request.Post_ID });

                var post = existing?.FirstOrDefault();
                if (post == null)
                {
                    return NotFound(new { error = "Không tìm thấy bài đăng." });
                }

                var isAdminClaim = User.FindFirst("IsAdmin")?.Value ?? "0";
                bool isAdmin = isAdminClaim == "1";

                if (!isAdmin && post.Employer_ID != employerId)
                {
                    return Forbid();
                }

                var result = await _jobpostingRepositoryDapper.Jobposting_Update(jobPostUpdate_Request);

                return result switch
                {
                    1 => Ok(new { message = "Cập nhật bài đăng thành công." }),
                    2 => Ok(new { message = "Cập nhật trạng thái bài đăng thành công." }),
                    -2 => Conflict(new { error = "Title đã tồn tại." }),
                    -1 => NotFound(new { error = "Không tìm thấy bản ghi cần cập nhật." }),
                    _ => StatusCode(500, new { error = "Lỗi không xác định từ server." })
                };
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPatch("update-partial/{id}")]
        public async Task<IActionResult> Update_Partial_JobPost(int id, [FromBody] JobPostUpdate_Request jobPostUpdatePartial_Request)
        {
            jobPostUpdatePartial_Request.Post_ID = id;
            int result = await _jobpostingRepositoryDapper.Jobposting_Update_Partial(jobPostUpdatePartial_Request);
            return result switch
            {
                1 => Ok(new { message = "Update Partial Post Successful." }),
                -2 => Conflict(new { error = "Title đã tồn tại." }),
                -1 => Conflict(new { error = "Không tìm thấy bản ghi nào." }),
                _ => StatusCode(500, new { error = "Lỗi không xác định từ server." })
            };
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete_JobPost(int id)
        {
            var resultPost = await _jobpostingRepositoryDapper.Get_JobPost_By_Id(
                new JobPostGetById_Request { Post_ID = id });

            var post = resultPost?.FirstOrDefault();
            if (post == null)
                return NotFound(new { error = "Bài đăng không tồn tại." });

            var employerIdClaim = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            var isAdminClaim = User.FindFirst("IsAdmin")?.Value ?? "0";

            if (!int.TryParse(employerIdClaim, out int currentEmployerId))
                return Unauthorized(new { error = "Token không hợp lệ." });

            bool isAdmin = isAdminClaim == "1";
            if (!isAdmin && post.Employer_ID != currentEmployerId)
                return Forbid();

            int result = await _jobpostingRepositoryDapper.Jobposting_Delete(
                new JobPostDelete_Request { Post_ID = id });

            return result switch
            {
                1 => Ok(new { message = "Xóa bài đăng thành công." }),
                -2 => NotFound(new { error = "Bài đăng không tồn tại." }),
                _ => StatusCode(500, new { error = "Lỗi không xác định từ server." })
            };
        }

    }


    internal class HttpUpdateAttribute : Attribute
    {
    }
}
