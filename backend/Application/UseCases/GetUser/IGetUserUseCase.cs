namespace Application.UseCases.GetUser
{
    public interface IGetUserUseCase
    {
        Task<GetUserResponse?> Handle(Guid id);
    }
}
