using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recuitment_DataAccess.IRepository;
using System.Security.Claims;

namespace WebAPI.Controllers.Controllers
{
    [Route("api/master-data")]
    [ApiController]
    [Authorize]
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterDataRepository _masterDataRepository;

        public MasterDataController(IMasterDataRepository masterDataRepository)
        {
            _masterDataRepository = masterDataRepository;
        }

        [HttpGet("contact-types")]
        public async Task<IActionResult> GetContactTypes()
        {
            var data = await _masterDataRepository.GetContactTypesAsync();
            return Ok(data);
        }

        [HttpGet("job-positions")]
        public async Task<IActionResult> GetJobPositions()
        {
            var data = await _masterDataRepository.GetJobPositionsAsync();
            return Ok(data);
        }

        [HttpGet("job-types")]
        public async Task<IActionResult> GetJobTypes()
        {
            var data = await _masterDataRepository.GetJobTypesAsync();
            return Ok(data);
        }

        [HttpGet("job-categories")]
        public async Task<IActionResult> GetJobCategories()
        {
            var data = await _masterDataRepository.GetJobCategoriesAsync();
            return Ok(data);
        }

        [HttpGet("cv-languages")]
        public async Task<IActionResult> GetCvLanguages()
        {
            var data = await _masterDataRepository.GetCvLanguagesAsync();
            return Ok(data);
        }

        [HttpGet("office-addresses")]
        public async Task<IActionResult> GetOfficeAddresses()
        {
            var employerIdClaim = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            if (!int.TryParse(employerIdClaim, out int employerId))
            {
                return Unauthorized(new { error = "Token không hợp lệ." });
            }

            var data = await _masterDataRepository.GetOfficeAddressesAsync(employerId);
            return Ok(data);
        }

        [HttpGet("job-keywords")]
        public async Task<IActionResult> GetJobKeywords()
        {
            var data = await _masterDataRepository.GetJobKeywordsAsync();
            return Ok(data);
        }
    }
}