namespace Hotels.Auth;

public interface IUserRepository
{
    UserDto GetUser(UserModel userModel);
}