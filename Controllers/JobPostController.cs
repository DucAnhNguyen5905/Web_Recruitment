using Microsoft.AspNetCore.Mvc;
using Recuitment_DataAccess.Data_Object.RequestData;
using Recuitment_DataAccess.Recuitment_Unitofwork;

namespace Recruitment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class JobPostController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        public JobPostController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        [HttpGet("Get_All_JobPosts")]
        public async Task<IActionResult> Get_All_JobPosts()
        {
            try
            {
                var result = await _unitofWork._jobPostingRepository.Get_All_JobPosts();

                if (result == null || result.Count == 0)
                {
                    return NotFound("Không có bài đăng nào.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi máy chủ: {ex.Message}");
            }
        }

        [HttpPost("Insert_JobPost")]
        public async Task<IActionResult> Insert_JobPost([FromBody] JobPostInsert_Request jobPostInsert_Request)
        {
            if (jobPostInsert_Request == null)
            {
                return BadRequest("Dữ liệu đầu vào không hợp lệ.");
            }

            try
            {
                var result = await _unitofWork._jobPostingRepository.Insert_JobPost(jobPostInsert_Request);

                if (result.ResponseCode == 0)
                {
                    return Ok(result); // Thành công
                }

                return BadRequest(result); // Có lỗi do nghiệp vụ, không phải exception
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi máy chủ: {ex.Message}");
            }
        }




    }
}
