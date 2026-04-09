using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Recuitment_DataAccess.Data_Object;
using Recuitment_DataAccess.Data_Object.RequestData;
using Recuitment_DataAccess.IRepository;
using Recuitment_DataAccess.Recuitment_Unitofwork;
using Recuitment_DataAccess.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Recuitment_Model.RequestData;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using WebAPI.Filter;

namespace Recruitment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class employerController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IEmployerRepositoryDapper _employerRepositoryDapper;
        private IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        private readonly ILogger<EmployerRepositoryDapper> _logger;
        public employerController(IUnitofWork unitofWork, IEmployerRepositoryDapper employerRepositoryDapper, IConfiguration configuration, IDistributedCache cache, ILogger<EmployerRepositoryDapper> logger)
        {
            _unitofWork = unitofWork;
            _employerRepositoryDapper = employerRepositoryDapper;
            _configuration = configuration;
            _cache = cache;
            _logger = logger;
        }

        [HttpDelete]
        [RecuitmentAuthorizeAttribute("Employer_Delete", "ISDELETE")]
        public async Task<IActionResult> Employer_Delete([FromBody] EmployerDelete_Request employerDelete_Request)
        {
            int result = await _employerRepositoryDapper.Employer_Delete(employerDelete_Request);

            switch (result)
            {
                case 1:
                    return Ok(new { message = "Xóa thành công." });

                case -2:
                    return NotFound(new { error = "Employer_ID không tồn tại." });

                case -3:
                    return BadRequest(new { error = "Không có bản ghi nào bị xóa." });

                default:
                    return StatusCode(500, new { error = "Lỗi không xác định từ server." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Employer_Insert([FromBody] EmployerInsert_Request employerInsert_Request)
        {
            int result = await _employerRepositoryDapper.Employer_Insert(employerInsert_Request);

            return result switch
            {
                > 0 => Ok(new { message = "Thêm employer thành công." }),
                -2 => Conflict(new { error = "Tên đã tồn tại." }),
                -3 => Conflict(new { error = "Lỗi chèn dữ liệu." }),
                _ => StatusCode(500, new { error = "Lỗi không xác định từ server." })
            };
        }

        [HttpGet]
        [RecuitmentAuthorizeAttribute("Employer_GetAll", "VIEW")]
        public async Task<IActionResult> Employer_GetAll([FromBody] EmployerGetAll_Request employerGetAll_Request)
        {
            try
            {
                // kiểm tra cache
                var cacheKey = "EmployerGetAll_KeyCaching";
                var cachedData = await _cache.GetStringAsync(cacheKey);
                var result = new List<Employer>();

                if (cachedData != null)
                {
                    // có dữ liệu -> lấy
                    result = JsonConvert.DeserializeObject<List<Employer>>(cachedData);
                    return Ok(result);
                }

                // nếu cache chưa có -> lấy DB
                result = await _employerRepositoryDapper.Employer_GetAll(employerGetAll_Request);
                if (result == null)
                {
                    return NotFound();
                }

                var cacheOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                _logger.LogInformation(
                    DateTime.Now.ToString("dd/MM/yyyy")
                    + " | Employer_GetAll RequestData: "
                    + JsonConvert.SerializeObject(employerGetAll_Request)
                );

                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(result), cacheOptions);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Lỗi khi lấy danh sách employer: " + ex.Message });
            }
        }

//        [HttpPut]
//        [RecuitmentAuthorizeAttribute("Employer_Update", "ISUPDATE")]
//        public async Task<IActionResult> Employer_Update([FromBody] EmployerUpdate_Request employerInsert_Request)
//        {
//            try
//            {
//               
//               
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal Server Error: {ex.Message}");
//            }
//        }

        [HttpPost("login")]
        public async Task<IActionResult> Employer_Login([FromBody] EmployerLogin_Request employerLogin_Request)
        {
            var result = new LoginReturnData_Employer();
            try
            {
                var resultlogin = await _employerRepositoryDapper.Employer_Login(employerLogin_Request);

                if (resultlogin == null)
                {
                    result.ResponseCode = -1;
                    result.ResponseMessage = "Sai tên đăng nhập hoặc mật khẩu.";
                    return Ok(result);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, resultlogin.Email),
                    new Claim(ClaimTypes.PrimarySid, resultlogin.Employer_ID.ToString()),
                    new Claim("IsAdmin", (resultlogin.IsAdmin ?? 0).ToString()),
                    new Claim(ClaimTypes.GivenName, resultlogin.FullName ?? "")
                };

                var newtoken = CreateToken(claims);
                var token = new JwtSecurityTokenHandler().WriteToken(newtoken);
                // Lưu token vào DB/Redis
                var user_Sessions = new User_Sessions
                {
                    Employer_ID = resultlogin.Employer_ID,
                    token = token,
                    device_id = employerLogin_Request.Device_ID,
                    exprired_time = newtoken.ValidTo
                };

                var keycaching = $"User_Sessions_{ resultlogin.Employer_ID}_{ employerLogin_Request.Device_ID}";
                var option_cache = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(30) // hết hạn 30p
                };
                var data_cache =JsonConvert.SerializeObject(user_Sessions);
                await _cache.SetStringAsync(keycaching, data_cache, option_cache);


                result.ResponseCode = 1;
                result.ResponseMessage = "Đăng nhập thành công.";
                result.Token = token;
                result.Employer_ID = resultlogin.Employer_ID;
                result.Name = resultlogin.Name;
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Employer_LogOut([FromBody] EmployerLogout_Request requestData)
        {
            try
            {
                var EmployerID = UserMana_Session.Employer_ID;
                var keyCache = $"User_Sessions_{EmployerID}_{requestData.DeviceID}";

                _cache.Remove(keyCache);

                return Ok(new { mes = "LogOut Thành công !" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            _ = int.TryParse(_configuration["Jwt:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(

                issuer: _configuration["Jwt:ValidIssuer"],

                audience: _configuration["Jwt:ValidAudience"],

                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),

                claims: authClaims,

                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }



    }
}
