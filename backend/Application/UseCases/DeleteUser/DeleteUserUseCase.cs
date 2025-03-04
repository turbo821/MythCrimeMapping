using Domain.Interfaces;

namespace Application.UseCases.DeleteUser
{
    public class DeleteUserUseCase : IDeleteUserUseCase
    {
        private IUserRepository _repo;

        public DeleteUserUseCase(IUserRepository repository)
        {
            _repo = repository;
        }
        public async Task<bool> Handle(Guid id)
        {
            return await _repo.DeleteUser(id);
        }
    }
}
