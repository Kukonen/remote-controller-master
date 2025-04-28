using Microsoft.AspNetCore.Mvc;


namespace RemoteControllerMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocsController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "docs";
        }
    }
}
