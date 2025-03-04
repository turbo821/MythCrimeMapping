namespace Application.Dtos
{
    public record ChangePasswordDto(Guid UserId, string Password, string Code);
}
