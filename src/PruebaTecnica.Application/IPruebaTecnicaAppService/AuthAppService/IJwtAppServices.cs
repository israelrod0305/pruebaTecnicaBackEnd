using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.IPruebaTecnicaAppService
{
    public interface IJwtAppServices
    {
        public string GenerateAccessToken(Claim[] claims, string keyInput, string issuer, string audience, double AccessTokenExpirationMinutes);


        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string keyInput, string issuer, string audience);
    }
}
