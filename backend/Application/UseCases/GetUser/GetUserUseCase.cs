using Application.UseCases.GetCrimeType;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.GetUser
{
    public class GetUserUseCase : IGetUserUseCase
    {
        private IUserRepository _repo;
        public GetUserUseCase(IUserRepository repository)
        {
            _repo = repository;
        }
        public async Task<GetUserResponse?> Handle(Guid id)
        {
            var user = await _repo.GetUserById(id);

            if (user == null)
            {
                return null;
            }

            return new GetUserResponse(user.Id, user.Name, user.Surname, user.Patronymic, user.Position, user.Email);
        }
    }
}
