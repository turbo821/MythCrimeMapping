using Application.Dtos;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.CreateCrime
{
    public class CreateCrimeUseCase : ICreateCrimeUseCase
    {
        private readonly ICrimeMarkRepository _repo;
        private readonly ICreateCrimeService _createCrimeService;
        public CreateCrimeUseCase(ICrimeMarkRepository _crimeRepository, ICreateCrimeService createCrimeService)
        {
            _repo = _crimeRepository;
            _createCrimeService = createCrimeService;
        }

        public async Task<CrimeReportResponse?> Handle(CreateCrimeRequest request)
        {
            CrimeBaseInfoDto data = new CrimeBaseInfoDto(
                request.CrimeTypeId, request.WantedPersonId, request.WantedPersonName,
                request.WantedPersonSurname, request.WantedPersonPatronymic, request.WantedPersonBirthDate, request.CrimeDate,
                request.Location, request.Description, request.PointLatitude, request.PointLongitude);

            Crime? crime = await _createCrimeService.CreateCrime(data, creatorId: request.CreatorId);

            if (crime is null)
            {
                return null;
            }

            await _repo.AddCrime(crime);
            return new CrimeReportResponse(crime.Id, "Crime report successfully created.");
        }
    }
}
