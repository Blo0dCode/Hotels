namespace Hotels.Auth;

public interface ITokenService
{
    string BuildToken(UserDto user);
}