using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Recruitment_Common;
using Recuitment_Common;
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

        [HttpPost("/api/jobpost/getAll")]
        [RecuitmentAuthorizeAttribute("Get_All_JobPosts", "ISVIEWS")]
        public async Task<IActionResult> Get_All_JobPosts([FromBody] JobPostGetAll_Request jobPostGetAll_Request)
        {
            try
            {
                var cacheKey = "JobPostGetAll_KeyCaching_" + jobPostGetAll_Request.Post_ID_List;
                var cachedData = await _cache.GetStringAsync(cacheKey);
                var result = new List<JobPost>();

                if (cachedData != null)
                {
                    var cachedResult = System.Text.Json.JsonSerializer.Deserialize<List<JobPost>>(cachedData);
                    return Ok(cachedResult); // Trả về dữ liệu từ cache nếu có
                }

                result = await _jobpostingRepositoryDapper.Get_All_JobPosts(jobPostGetAll_Request);

                if (result == null)

                {
                    return NotFound();
                }

                var cacheOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(1));

                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(result), cacheOptions);

                return Ok(result); // Trả về danh sách bài đăng công việc
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Lỗi khi lấy danh sách bài đăng công việc: " + ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get_JobPost_ByID(int id)
        {
            var result = await _jobpostingRepositoryDapper.Get_JobPost_By_Id(new JobPostGetById_Request { Post_ID = id });
            if (result == null || result.Count == 0)
            {
                return NotFound(new { error = "Post ID not found !!!." });
            }
            return Ok(result);
        }


        [HttpPost("/api/jobpost/insert")]
        public async Task<IActionResult> Insert_JobPost([FromBody] JobPostInsert_Request jobPostInsert_Request)
        {
            try
            {
                Validate_Data.Validate_Data_JobPostInsert(jobPostInsert_Request);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }


            int result = await _jobpostingRepositoryDapper.Jobposting_Insert(jobPostInsert_Request);

            return result switch
            {
                1 => Ok(new { message = "Đăng bài thành công." }),
                -2 => Conflict(new { error = "Title đã tồn tại." }),
                _ => StatusCode(500, new { error = "Lỗi không xác định từ server." })
            };
        }


        [HttpPut()]
        public async Task<IActionResult> Update_JobPost([FromBody] JobPostUpdate_Request jobPostUpdate_Request)
        {
            int result = await _jobpostingRepositoryDapper.Jobposting_Update(jobPostUpdate_Request);
            return result switch
            {
                1 => Ok(new { message = "Update Post Successful." }),
                2 => Ok(new { message = "Update PostStatus Successfull. " }),
                -2 => Conflict(new { error = "Title đã tồn tại." }),
                _ => StatusCode(500, new { error = "Lỗi không xác định từ server." })
            };
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

        [HttpDelete()]
        public async Task<IActionResult> Delete_JobPost([FromBody] JobPostDelete_Request jobPostDelete_Request)
        {
            int result = await _jobpostingRepositoryDapper.Jobposting_Delete(jobPostDelete_Request);
            return result switch
            {
                1 => Ok(new { message = "Delete Successful." }),
                -2 => Conflict(new { error = "Bài đăng không tồn tại." }),
                _ => StatusCode(500, new { error = "Lỗi không xác định từ server." })
            };

        }

    }


    internal class HttpUpdateAttribute : Attribute
    {
    }
}
