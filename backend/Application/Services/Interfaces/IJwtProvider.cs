using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
