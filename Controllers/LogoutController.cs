using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace licenseDemoNetCore.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class LogoutController : ControllerBase
    {
        private readonly ILogger<LogoutController> _logger;

        public LogoutController(ILogger<LogoutController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get() 
        {
            _logger.LogInformation("logout istegi geldi...");
            var redisManager = new RedisCacheManager();
            var expiredToken = HttpContext.Request.Headers["Authorization"];

            var tokens = new JwtSecurityToken(jwtEncodedString: expiredToken.ToString().Split(' ')[1]);
            var userId = tokens.Claims.First(c => c.Type == "unique_name").Value;

            redisManager.Remove(userId);

            return Ok();
        }
    }
}