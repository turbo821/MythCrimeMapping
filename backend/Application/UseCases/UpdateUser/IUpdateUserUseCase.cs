namespace Application.UseCases.UpdateUser
{
    public interface IUpdateUserUseCase
    {
        Task<CrimeReportResponse?> Handle(Guid id, UpdateUserRequest request);
    }
}
