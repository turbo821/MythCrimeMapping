namespace Application.UseCases.DeleteUser
{
    public interface IDeleteUserUseCase
    {
        Task<bool> Handle(Guid id);
    }
}
