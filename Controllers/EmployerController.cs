using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Recuitment_DataAccess.Data_Object;
using Recuitment_DataAccess.Data_Object.RequestData;
using Recuitment_DataAccess.IRepository;
using Recuitment_DataAccess.Recuitment_Unitofwork;
using Recuitment_DataAccess.Repository;

namespace Recruitment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        public EmployerController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        [HttpPost("Employer_Delete")]
        public async Task<IActionResult> Employer_Delete([FromBody] EmployerDelete_Request employerDelete_Request)
        {
            try
            {
                var result = await _unitofWork._employerRepository.Employer_Delete(employerDelete_Request);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("Employer_Insert")]
        public async Task<IActionResult> Employer_Insert([FromBody] EmployerInsert_Request employerInsert_Request)
        {
            try
            {
                var result = await _unitofWork._employerRepository.Insert_Employer(employerInsert_Request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("Employer_Update")]
        public async Task<IActionResult> Employer_Update([FromBody] EmployerUpdate_Request employerUpdate_Request)
        {
            try
            {
                var result = await _unitofWork._employerRepository.Update_Employer(employerUpdate_Request);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("Employer_Login")]
        public async Task<IActionResult> Employer_Login([FromBody] EmployerLogin_Request employerLogin_Request)
        {
            if (employerLogin_Request == null ||
                string.IsNullOrEmpty(employerLogin_Request.Name) ||
                string.IsNullOrEmpty(employerLogin_Request.Password))
            {
                return BadRequest("Name and Password cannot be null.");
            }

            try
            {
                var result = await _unitofWork._employerRepository.Login_Employer(employerLogin_Request);

                if (result == null)
                {
                    return Unauthorized("Wrong name or password.");
                }

                return Ok(result); // có thể trả về DTO nếu không muốn trả password
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}
