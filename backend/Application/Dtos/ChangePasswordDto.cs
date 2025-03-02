namespace Application.Dtos
{
    public record ChangePasswordDto(Guid UserId, string NewPassword, string Code);
}
