using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bookings.Shared;

public static class Generics
{
    public static ClaimsPrincipal SetClaimsPrincipal(UserSession model)
    {
        return new ClaimsPrincipal(
            new ClaimsIdentity(
                new List<Claim>
                {
                    new (ClaimTypes.NameIdentifier, model.Id!), 
                    new (ClaimTypes.Name, model.Name!), 
                    new (ClaimTypes.Email, model.Email!), 
                    new (ClaimTypes.Role, model.Role!), 
                }, "JwtAuth"));
    }


    public static UserSession GetClaimsFromToken(string jwtToken)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var claims = token.Claims;

            var id = claims.First(c=> c.Type == ClaimTypes.NameIdentifier).Value; 
            var name = claims.First(c=> c.Type == ClaimTypes.Name).Value; 
            var email = claims.First(c=> c.Type == ClaimTypes.Email).Value; 
            var role = claims.First(c=> c.Type == ClaimTypes.Role).Value;

            return new UserSession(id, name, email, role);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
      
    }
}