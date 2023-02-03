using Hotels.Auth;
using Hotels.Options;
using Microsoft.Extensions.Options;

namespace Hotels.Apis;

public class AuthApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/login", [AllowAnonymous]([FromQuery] string username, [FromQuery] string password,
            [FromServices] ITokenService tokenService, [FromServices] IUserRepository userRepository,
            [FromServices] IOptions<JwtOptions> jwtOptions) =>
        {
            UserModel userModel = new()
            {
                UserName = username,
                Password = password
            };
            var userDto = userRepository.GetUser(userModel);
            if (userDto == null) return Results.NotFound();
            var token = tokenService.BuildToken(userDto);
            return Results.Ok(token);
        });
    }
}