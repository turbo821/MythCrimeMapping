using Application.Dtos;
using Application.Services.Interfaces;
using Application.UseCases.CreateCrime;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.UpdateCrime
{
    public class UpdateCrimeUseCase : IUpdateCrimeUseCase
    {
        private readonly ICrimeMarkRepository _markRepository;
        private readonly ICreateCrimeService _createCrimeService;
        private readonly ICrimeTypeRepository _typeRepository;  
        public UpdateCrimeUseCase(ICrimeMarkRepository markRepository, ICrimeTypeRepository typeRepository, ICreateCrimeService createCrimeService)
        {
            _markRepository = markRepository;
            _createCrimeService = createCrimeService;
            _typeRepository = typeRepository;
        }

        public async Task<CrimeReportResponse?> Handle(UpdateCrimeRequest request)
        {
            if(!_typeRepository.ContainCrimeType(request.CrimeTypeId))
                return null;

            CrimeBaseInfoDto createRequest = new CrimeBaseInfoDto(
                request.CrimeTypeId, request.WantedPersonId, 
                request.WantedPersonName, request.WantedPersonSurname, request.WantedPersonPatronymic, request.WantedPersonBirthDate,
                request.CrimeDate, request.Location, request.Description, request.PointLatitude, request.PointLongitude
            );

            Crime? updatedCrime = await _createCrimeService.CreateCrime(createRequest, editorId: request.EditorId);
            Crime? crime = await _markRepository.GetCrimeById(request.Id);

            if (updatedCrime is null || crime is null)
            {
                return null;
            }

            crime.TypeId = updatedCrime.TypeId;
            crime.WantedPersonId = updatedCrime.WantedPersonId;
            crime.Location = updatedCrime.Location;
            crime.EditorId = updatedCrime.EditorId;
            crime.EditAt = updatedCrime.EditAt;
            crime.CrimeDate = updatedCrime.CrimeDate;
            crime.Description = updatedCrime.Description;

            await _markRepository.UpdateCrime(crime);

            return new CrimeReportResponse(crime.Id, "Crime report successfully edited.");
        }
    }
}
