using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Security.Claims;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Recuitment_DataAccess.Data_Object;
using Recuitment_DataAccess.EFCore;

namespace WebAPI.Filter
{
    public class RecuitmentAuthorizeAttribute : TypeFilterAttribute
    {

        public RecuitmentAuthorizeAttribute(string _functionCode, string _permission) : base(typeof(DemoAuthorizeActionFilter))
        {
            Arguments = new object[] { _functionCode, _permission };
        }

        public class DemoAuthorizeActionFilter : IAsyncAuthorizationFilter
        {
            public readonly Recruitment_DBContext _dbContext;
            private readonly string _functionCode;
            private readonly string _permission;

            public DemoAuthorizeActionFilter(Recruitment_DBContext dbContext, string functionCode , string permission)
            {
                _dbContext = dbContext;
                _functionCode = functionCode;
                _permission = permission;
            }
            public Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var identity = context.HttpContext.User.Identity as ClaimsIdentity;

                if (identity != null)
                {
                    var userClaims = identity.Claims;
                    var userId = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value != null
                        ? Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value) : 0;

                    if (userId == 0)
                    {
                        context.HttpContext.Response.ContentType = "application/json";
                        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                        context.Result = new JsonResult(new
                        {
                            ReturnCode = System.Net.HttpStatusCode.Unauthorized,
                            ReturnMessage = "Vui lòng đăng nhập để thực hiện chức năng này "
                        });

                    }
                    // lấy functionID từ functionCode
                    var function = _dbContext.function.FirstOrDefault(f => f.FunctionCode == _functionCode);
                    if (function == null || function.FunctionID <= 0)
                    {
                        context.HttpContext.Response.ContentType = "application/json";
                        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                        context.Result = new JsonResult(new
                        {
                            ReturnCode = System.Net.HttpStatusCode.Unauthorized,
                            ReturnMessage = "Chức năng không tồn tại"
                        });
                        
                    }

                    // Check permission
                        var permission  = _dbContext.permission.FirstOrDefault(x => x.EmployerID == userId && x.FunctionID == function.FunctionID);
                    if (permission == null || permission.PermissionID <= 0)
                    {
                        context.HttpContext.Response.ContentType = "application/json";
                        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                        context.Result = new JsonResult(new
                        {
                            ReturnCode = System.Net.HttpStatusCode.Unauthorized,
                            ReturnMessage = "Bạn không có quyền thực hiện chức năng này"
                        });
                    }

                    if (_permission == "ISVIEWS" && permission != null && permission.IsView == 0)
                    {
                        context.HttpContext.Response.ContentType = "application/json";
                        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                        context.Result = new JsonResult(new
                        {
                            ReturnCode = System.Net.HttpStatusCode.Unauthorized,
                            ReturnMessage = "Bạn không có quyền xem chức năng này"
                        });
                    }    
                }

                return Task.CompletedTask;
            }

        }
    }
}
