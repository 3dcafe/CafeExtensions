using CafeExtensions.Interfaces;

namespace CafeExtensions.Services;
/// <summary>
/// Базовый интерфейс для службы выдачи токенов
/// </summary>
public interface IJWTTokenService
{
    /// <summary>
    /// Получить токен базовый метод который должен быть реализован
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string GetJwtSecurityToken(IUser user);
}
