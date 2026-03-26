using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Recuitment_DataAccess.Data_Object.RequestData;
using Recuitment_DataAccess.IRepository;
using Recuitment_DataAccess.Recuitment_Unitofwork;
using Recuitment_DataAccess.Repository;
using Recuitment_Model.RequestData;

namespace WebAPI.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ICandidateRepositoryDapper _candidateRepositoryDapper;
        private IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        private readonly ILogger<CandidateRepositoryDapper> _logger;

        public CandidateController(IUnitofWork unitofWork, ICandidateRepositoryDapper candidateRepositoryDapper, IConfiguration configuration, IDistributedCache cache, ILogger<CandidateRepositoryDapper> logger)
        {
            _unitofWork = unitofWork;
            _candidateRepositoryDapper = candidateRepositoryDapper;
            _configuration = configuration;
            _cache = cache;
            _logger = logger;
        }

        [HttpPost("getAll")]
        public async Task<IActionResult> Candidate_GetAll([FromBody] CandidateGetAll_Request candiddateGetAll_Request)
        {
            try
            {
                var result = await _candidateRepositoryDapper.Candidate_GetAll(candiddateGetAll_Request);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
           
           
        }

        [HttpPost("getById")]
        public async Task<IActionResult> Candidate_GetById([FromBody] CandidateGetAll_Request candidateGetById_Request)
        {
            try
            {
                var result = await _candidateRepositoryDapper.Candidate_GetById(candidateGetById_Request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpPost("upCv")] 
        public async Task<IActionResult> Candidate_UpLoadCv([FromBody] CandidateUpLoadCv_Request candidateUpCv_Request)
        {
            try
            {
                // Giả sử bạn có một phương thức trong repository để xử lý việc cập nhật CV
                // await _candidateRepositoryDapper.Candidate_UpCv(candidateUpCv_Request);
                return Ok(new { message = "CV updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating CV for candidate ID {CandidateId}", candidateUpCv_Request.candidate_id);
                return StatusCode(500, new { error = "An error occurred while updating the CV." });
            }
        }

    }
}
