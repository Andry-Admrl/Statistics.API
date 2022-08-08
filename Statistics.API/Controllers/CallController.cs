using Microsoft.AspNetCore.Mvc;
using Statistics.API.Interfaces;

namespace Statistics.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("[controller]")]
    [ApiController]
    public class CallController : Controller
    {
        private readonly ICallService _callService;
        public CallController(ICallService callService)
        {
            _callService = callService;
        }

        [HttpGet("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult AddCall()
        {
            return Ok();
        }
    }
}
