using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestSoft.Collections;

namespace TestSoft.Controllers
{

    [Route("[controller]")]
    [ApiController]

    public class AuthenticationController : ControllerBase
    {

        private readonly string secretkey;

        public AuthenticationController(IConfiguration config)
        {
            secretkey = config.GetSection("settings").GetSection("secretkey").ToString();
        }

        [HttpPost]
        [Route("Validar")]

        public IActionResult Validar([FromBody] User request)
        {
            if (request.Username == "UserDev" && request.Password == "Dev123")
            {
                var KeyBytes = Encoding.UTF8.GetBytes(secretkey);
                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Username));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddYears(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(KeyBytes), SecurityAlgorithms.HmacSha256Signature)

                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string tokenCreado = tokenHandler.WriteToken(tokenConfig);

                return StatusCode(StatusCodes.Status200OK, new { token = tokenCreado });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }
        }

    }


}
