namespace Hotels.Auth;

public class UserRepository : IUserRepository
{
    private static List<UserDto> Users => new()
    {
        new UserDto("Vadim", "123"),
        new UserDto("Andrey", "123")
    };

    public UserDto? GetUser(UserModel userModel) =>
        Users.FirstOrDefault(u =>
            u.UserName == userModel.UserName &&
            u.Password == userModel.Password);
}