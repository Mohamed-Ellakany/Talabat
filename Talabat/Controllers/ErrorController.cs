using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Errors;

namespace Talabat.Controllers
{
    [Route("error/{Code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        public ActionResult Error(int Code)
        {
            return NotFound(new ApiResponse(Code));
        }

    }
}
