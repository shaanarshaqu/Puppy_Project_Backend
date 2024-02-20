using System.IdentityModel.Tokens.Jwt;

namespace Puppy_Project.Secure
{
    public class TokenDecoder
    {
        public static int DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Read and validate the token
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken != null)
            {
                // Access and print the value of the "NameIdentifier" claim
                var nameIdentifierClaim = jwtToken.Claims.SingleOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

                if (nameIdentifierClaim == null)
                {
                    return -1;
                }
                return Convert.ToInt32(nameIdentifierClaim.Value);
            }
            return -1;
        }
    }
}
