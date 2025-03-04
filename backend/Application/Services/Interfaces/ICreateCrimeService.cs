using Application.Dtos;
using Application.UseCases.CreateCrime;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface ICreateCrimeService
    {
        Task<Crime?> CreateCrime(CrimeBaseInfoDto request, Guid? creatorId = null, Guid? editorId = null);
    }
}
