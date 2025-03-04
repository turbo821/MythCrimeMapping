namespace Application.UseCases.UpdateUser
{
    public record UpdateUserRequest(string Name, string Surname, string? Patronymic, string Position, string Email);
}
