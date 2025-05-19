using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PruebaTecnica.Configuration;
using PruebaTecnica.Dto;
using PruebaTecnica.IPruebaTecnicaAppService;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



namespace PruebaTecnica.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class AuthController : AbpController
    {
        private readonly IJwtAppServices _jwtService;
        private readonly IRefreshTokeAppService _refreshTokenAppService;
        private readonly IUserAppService _userAppService;
        private readonly IConfiguration _iconfiguration;
        public AuthController(
            IJwtAppServices jwtService,
            IRefreshTokeAppService refreshTokenAppService,
            IUserAppService userAppService,
             IConfiguration iconfiguration)
        {
            _jwtService = jwtService;
            _refreshTokenAppService = refreshTokenAppService;
            _userAppService = userAppService;
            _iconfiguration = iconfiguration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _userAppService.ValiateUserAsync(request.Username, request.Password);
            if (user.Data == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Data.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                // agrega más claims según necesidades
            };

            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
            var _issuerString = configuration.GetSection("Jwt").GetValue<string>("Issuer");
            var _audienceString = configuration.GetSection("Jwt").GetValue<string>("Audience");
            var _keyString = configuration.GetSection("Jwt").GetValue<string>("Key");
            var _AccessTokenExpirationMinutes = configuration.GetSection("Jwt").GetValue<string>("AccessTokenExpirationMinutes");


            var accessToken = _jwtService.GenerateAccessToken(claims, _keyString, _issuerString, _audienceString, Convert.ToDouble(_AccessTokenExpirationMinutes));
            var refreshToken = _jwtService.GenerateRefreshToken();

            _ = await _refreshTokenAppService.SaveRefreshToken(user.Data.Id, refreshToken, DateTime.UtcNow.AddDays(7));

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Codigo=200,
                Mensaje= "Usuario ingreso correctamente",
                User = user.Data
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto request)
        {
            try
            {
                var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
                var _issuerString = configuration.GetSection("Jwt").GetValue<string>("Issuer");
                var _audienceString = configuration.GetSection("Jwt").GetValue<string>("Audience");
                var _keyString = configuration.GetSection("Jwt").GetValue<string>("Key");
                var _AccessTokenExpirationMinutes = configuration.GetSection("Jwt").GetValue<string>("AccessTokenExpirationMinutes");

                var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken, _keyString, _issuerString, _audienceString);
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                    return Unauthorized();

                var response = await _refreshTokenAppService.ValidateRefreshToken(int.Parse(userId), request.RefreshToken);

                if (! response.Data)
                    return Unauthorized();
                var newAccessToken = _jwtService.GenerateAccessToken(principal.Claims.ToArray(), _keyString, _issuerString, _audienceString, Convert.ToDouble(_AccessTokenExpirationMinutes));

                var newRefreshToken = _jwtService.GenerateRefreshToken();

                _ = _refreshTokenAppService.SaveRefreshToken(int.Parse(userId), newRefreshToken, DateTime.UtcNow.AddDays(7));

                return Ok(new
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            try
            {
                var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
                var _issuerString = configuration.GetSection("Jwt").GetValue<string>("Issuer");
                var _audienceString = configuration.GetSection("Jwt").GetValue<string>("Audience");
                var _keyString = configuration.GetSection("Jwt").GetValue<string>("Key");

                var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken,_keyString,_issuerString,_audienceString);

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                await _refreshTokenAppService.DeleteRefreshToken(int.Parse(userId), request.RefreshToken);

                return Ok(new
                {
                    Codigo = 200,
                    Mensaje = "Sesión cerrada correctamente"
                });
            }
            catch
            {
                return Unauthorized(new { Codigo = 401, Mensaje = "Token inválido o sesión ya cerrada." });
            }
        }
    }

   
}
