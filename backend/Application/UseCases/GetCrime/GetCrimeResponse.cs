namespace Application.UseCases.GetCrime
{
    public record GetCrimeResponse(
        Guid Id,
        Guid? CreatorId,
        Guid? EditorId,
        Guid CrimeTypeId,
        Guid? WantedPersonId,
        string? WantedPersonName,
        string? WantedPersonSurname,
        string? WantedPersonPatronymic,
        DateTime? WantedPersonBirthDate,
        DateTime CreateAt,
        DateTime? EditAt,
        DateTime CrimeDate,
        string? Location,
        string? Description,
        double PointLatitude,
        double PointLongitude);
}
