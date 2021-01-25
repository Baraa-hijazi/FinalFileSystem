using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MyFileSystem.Controllers
{

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BaseController : ControllerBase
    {
        [NonAction]
        protected string GetCurrentUserName()
        {
            return User.Claims.First(i => i.Type == "Username").Value;
        }

        //[NonAction]
        //protected string GetCurrentUserPassword()
        //{
        //    return User.Claims.First(i => i.Type == "Password").Value;
        //}
    }
}
