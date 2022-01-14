using TestApp.Models;

namespace TestApp.Services
{
    public static class CreateUserService
    {
        public static User CreateUserWithCredentials(string login, string password)
        {
            return new User
            {
                Login = login,
                Password = password
            };
        }

        public static User CreateUserWithNoLoginAndPassword()
        {
            return new User
            {
                Login = string.Empty,
                Password = string.Empty
            };
        }

        public static User CreateUserWithNoPassword(string login)
        {
            return new User
            {
                Login = login,
                Password = string.Empty
            };
        }
    }
}
