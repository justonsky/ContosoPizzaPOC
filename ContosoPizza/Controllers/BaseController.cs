using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContosoPizza.Controllers
{
    public class BaseController : Controller
    {
        protected string HubConnectionId =>
            HttpContext?.Request.Headers["x-HubConnectionId"] ?? string.Empty;
        public BaseController()
        { }
    }
}