using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace GestionComercial.Desktop.Helpers
{
    public static class TokenHelper
    {
        public static string GetClaim(string token, string claimType)
        {
            var handler = new JwtSecurityTokenHandler();
            string realToken = ExtractTokenValue(token);

            var jwtToken = handler.ReadJwtToken(realToken);

            // Buscar el claim exacto
            var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType);
            return claim?.Value ?? string.Empty;
        }

        // Atajos para los claims comunes
        public static string GetUsername(string token)
        {
            return GetClaim(token, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
        }

        public static string GetUserId(string token)
        {
            return GetClaim(token, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        }

        public static string GetRole(string token)
        {
            return GetClaim(token, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
        }

        public static string ExtractTokenValue(string jsonWithToken)
        {
            try
            {
                var jsonDoc = JsonDocument.Parse(jsonWithToken);
                return jsonDoc.RootElement.GetProperty("token").GetString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
