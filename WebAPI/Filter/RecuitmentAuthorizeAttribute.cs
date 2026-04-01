using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Recuitment_DataAccess.EFCore;
using System.Security.Claims;

namespace WebAPI.Filter
{
    public class RecuitmentAuthorizeAttribute : TypeFilterAttribute
    {
        public RecuitmentAuthorizeAttribute(string functionCode, string permission)
            : base(typeof(DemoAuthorizeActionFilter))
        {
            Arguments = new object[] { functionCode, permission };
        }

        public class DemoAuthorizeActionFilter : IAsyncAuthorizationFilter
        {
            private readonly Recruitment_DBContext _dbContext;
            private readonly string _functionCode;
            private readonly string _permission;

            public DemoAuthorizeActionFilter(
                Recruitment_DBContext dbContext,
                string functionCode,
                string permission)
            {
                _dbContext = dbContext;
                _functionCode = functionCode;
                _permission = permission;
            }

            public Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var identity = context.HttpContext.User.Identity as ClaimsIdentity;

                if (identity == null || !identity.IsAuthenticated)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(new
                    {
                        returnCode = 401,
                        returnMessage = "Vui lòng đăng nhập để thực hiện chức năng này"
                    });
                    return Task.CompletedTask;
                }

                var userIdClaim = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value;
                if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(new
                    {
                        returnCode = 401,
                        returnMessage = "Token không hợp lệ"
                    });
                    return Task.CompletedTask;
                }

                var isAdminClaim = identity.Claims.FirstOrDefault(x => x.Type == "IsAdmin")?.Value ?? "0";
                if (isAdminClaim == "1")
                {
                    return Task.CompletedTask;
                }

                var function = _dbContext.function.FirstOrDefault(f => f.FunctionCode == _functionCode);
                if (function == null || function.FunctionID <= 0)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(new
                    {
                        returnCode = 401,
                        returnMessage = "Chức năng không tồn tại"
                    });
                    return Task.CompletedTask;
                }

                var permission = _dbContext.permission
                    .FirstOrDefault(x => x.EmployerID == userId && x.FunctionID == function.FunctionID);

                if (permission == null || permission.PermissionID <= 0)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(new
                    {
                        returnCode = 401,
                        returnMessage = "Bạn không có quyền thực hiện chức năng này"
                    });
                    return Task.CompletedTask;
                }

                bool isAllowed = _permission.ToUpper() switch
                {
                    "ISVIEWS" => permission.IsView == 1,
                    "ISINSERT" => permission.IsInsert == 1,
                    "ISDELETE" => permission.IsDelete == 1,
                    "ISUPDATE" => true, // thêm cột IsUpdate nếu DB của m có
                    _ => false
                };

                if (!isAllowed)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(new
                    {
                        returnCode = 401,
                        returnMessage = "You dont have permission to perform this function !!!"
                    });
                    return Task.CompletedTask;
                }

                return Task.CompletedTask;
            }
        }
    }
}