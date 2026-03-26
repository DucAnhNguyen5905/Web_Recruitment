using Microsoft.AspNetCore.Mvc;

namespace RecruitmentWebFE.Controllers
{
    public class BaseController : Controller
    {
        protected bool HasCookie(string cookieName)
        {
            return Request.Cookies.ContainsKey(cookieName);
        }

        protected string? GetCookieValue(string cookieName)
        {
            if (!Request.Cookies.TryGetValue(cookieName, out var value))
            {
                return null;
            }

            return value;
        }

        protected string? GetAccessToken()
        {
            return GetCookieValue("AccessToken");
        }
    }
}