using Microsoft.AspNetCore.Mvc;

namespace Basics.CC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new
        {
            Claims = User.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value,
            })
        });
    }
}
