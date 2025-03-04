namespace Application.UseCases.GetUser
{
    public record class GetUserResponse(Guid Id, string Name, string Surname, string? Patronymic, string Position, string Email);
}
