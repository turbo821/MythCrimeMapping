﻿
namespace Application.Dtos
{
    public record CrimeBaseInfoDto(
        Guid CrimeTypeId,
        Guid? WantedPersonId,
        string? WantedPersonName,
        string? WantedPersonSurname,
        string? WantedPersonPatronymic,
        DateTime? WantedPersonBirthDate,
        DateTime CrimeDate,
        string Location,
        string? Description,
        double PointLatitude,
        double PointLongitude);
}
