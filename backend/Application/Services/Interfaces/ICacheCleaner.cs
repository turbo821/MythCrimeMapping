
namespace Application.Services.Interfaces
{
    public interface ICacheCleaner
    {
        Task RemoveByPatternAsync(string pattern);
    }
}
