using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.UpdateUser
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly IUserRepository _repo;
        public UpdateUserUseCase(IUserRepository repository)
        {
            _repo = repository;
        }

        public async Task<CrimeReportResponse?> Handle(Guid id, UpdateUserRequest request)
        {
            User? user = await _repo.GetUserById(id);
            if (user is null)
            {
                return null;
            }

            user.Name = request.Name ?? user.Name;
            user.Surname = request.Surname ?? user.Surname;
            user.Patronymic = request.Patronymic ?? user.Patronymic;
            user.Position = request.Position ?? user.Position;
            user.Email = request.Email ?? user.Email;

            await _repo.UpdateUser(user);

            return new CrimeReportResponse(user.Id, "User info successfully edited.");
        }
    }
}
