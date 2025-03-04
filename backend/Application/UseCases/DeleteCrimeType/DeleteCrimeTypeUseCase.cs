using Domain.Interfaces;

namespace Application.UseCases.DeleteCrimeType
{
    public class DeleteCrimeTypeUseCase : IDeleteCrimeTypeUseCase
    {
        private ICrimeTypeRepository _repo;

        public DeleteCrimeTypeUseCase(ICrimeTypeRepository repository)
        {
            _repo = repository;
        }
        public async Task<bool> Handle(Guid id)
        {
            return await _repo.DeleteCrimeType(id);
        }
    }
}
