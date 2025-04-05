using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.UI.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet(UiRoute.Error.ByStatusCode)]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 401:
                    return View("Unauthorized");
                case 403:
                    return View("AccessDenied");
                case 404:
                    return View("NotFound");
                default:
                    return View("Error");
            }
        }

        [HttpGet(UiRoute.Error.GeneralError)]
        public IActionResult GeneralError()
        {
            return View("Error");
        }
    }
}
