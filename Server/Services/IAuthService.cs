using Server.Dtos.Auth;
using Server.Models;
namespace Server.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Регистрирует нового пользователя
        /// </summary>
        /// <param name="nickname">Никнейм</param>
        /// <param name="email">Email</param>
        /// <param name="password">Пароль (в открытом виде)</param>
        /// <returns>Созданный пользователь</returns>
        /// <exception cref="ArgumentException">Если email или nickname уже заняты</exception>
        Task<User> RegisterAsync(string nickname, string email, string password);

        /// <summary>
        /// Входит в систему и возвращает JWT-токен
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <param name="password">Пароль (в открытом виде)</param>
        /// <returns>JWT-токен и время истечения</returns>
        /// <exception cref="UnauthorizedAccessException">Если email или пароль неверны</exception>
        Task<AuthResultDto> LoginAsync(string email, string password);
    }
}
