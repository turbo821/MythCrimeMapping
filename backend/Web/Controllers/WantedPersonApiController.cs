using Application.UseCases.CreateWantedPerson;
using Application.UseCases.DeleteWantedPerson;
using Application.UseCases.SelectAllWantedPersons;
using Application.UseCases.GetWantedPerson;
using Application.UseCases.UpdateWantedPerson;
using Microsoft.AspNetCore.Mvc;
using Application.UseCases.GetAllWantedPerson;
using Application.Pagination;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Application.Services.Interfaces;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/wanted-persons")]
    public class WantedPersonApiController : ControllerBase
    {
        private readonly ISelectAllWantedPersonsUseCase _selectAllWantedPersons;
        private readonly IGetAllWantedPersonUseCase _getAllWantedPersons;
        private readonly IGetWantedPersonUseCase _getWantedPerson;
        private readonly ICreateWantedPersonUseCase _createWantedPerson;
        private readonly IUpdateWantedPersonUseCase _updateWantedPerson;
        private readonly IDeleteWantedPersonUseCase _deleteWantedPerson;
        private readonly ICacheService _cache;
        private readonly ICacheKeyTracker _cacheKeyTracker;
        private readonly ICacheCleaner _cacheCleaner;

        public WantedPersonApiController(
            ISelectAllWantedPersonsUseCase selectAllWantedPersons,
            IGetAllWantedPersonUseCase getAllWantedPersons,
            IGetWantedPersonUseCase getWantedPerson,
            ICreateWantedPersonUseCase createWantedPerson,
            IUpdateWantedPersonUseCase updateWantedPerson,
            IDeleteWantedPersonUseCase deleteWantedPerson,
            ICacheService cache,
            ICacheKeyTracker cacheKeyTracker,
            ICacheCleaner cacheCleaner)
        {
            _selectAllWantedPersons = selectAllWantedPersons;
            _getAllWantedPersons = getAllWantedPersons;
            _getWantedPerson = getWantedPerson;
            _createWantedPerson = createWantedPerson;
            _updateWantedPerson = updateWantedPerson;
            _deleteWantedPerson = deleteWantedPerson;
            _cache = cache;
            _cacheKeyTracker = cacheKeyTracker;
            _cacheCleaner = cacheCleaner;
            _cacheCleaner = cacheCleaner;
        }

        [HttpGet("basic")]
        public async Task<IActionResult> SelectAllWantedPersons()
        {
            var cacheKey = "AllWantedPersons";
            if (!await _cache.ExistsAsync(cacheKey))
            {
                var response = await _selectAllWantedPersons.Handle();
                await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10));
                _cacheKeyTracker.AddKey(cacheKey);
                return Ok(response);
            }

            var cached = await _cache.GetAsync<object>(cacheKey);
            return Ok(cached);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWantedPersons([FromQuery] PaginationSearchParameters request)
        {
            var cacheKey = $"AllWantedPersons_{request.GetCacheKey()}";
            if (!await _cache.ExistsAsync(cacheKey))
            {
                var response = await _getAllWantedPersons.Handle(request);
                await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10));
                _cacheKeyTracker.AddKey(cacheKey);
                return Ok(response);
            }

            var cached = await _cache.GetAsync<object>(cacheKey);
            return Ok(cached);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetWantedPerson(Guid id)
        {
            var cacheKey = $"WantedPerson_{id}";
            if (!await _cache.ExistsAsync(cacheKey))
            {
                var response = await _getWantedPerson.Handle(id);

                if (response == null)
                {
                    return NotFound(new { Message = $"Wanted person with ID {id} not found." });
                }

                await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10));
                _cacheKeyTracker.AddKey(cacheKey);
                return Ok(response);
            }

            var cached = await _cache.GetAsync<object>(cacheKey);
            return Ok(cached);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddWantedPerson([FromBody] CreateWantedPersonRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "You are not logged in" });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _createWantedPerson.Handle(request);

            if (response is null)
                return BadRequest(new { Message = "Such a wanted person already exists." });

            await ClearCrimeCache(response.Id);

            return CreatedAtAction(nameof(GetWantedPerson), new { response.Id }, response);
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateWantedPerson([FromBody] UpdateWantedPersonRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "You are not logged in" });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _updateWantedPerson.Handle(request);

            if (response is null)
                return BadRequest(new { Message = "Not found a type of crime with this id." });

            await ClearCrimeCache(response.Id);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveWantedPerson(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "You are not logged in" });
            }

            var response = await _deleteWantedPerson.Handle(id);

            if(!response)
                return NotFound();

            await ClearCrimeCache(id);

            return Ok();
        }

        private async Task ClearCrimeCache(Guid crimeTypeId)
        {
            await _cache.RemoveAsync($"WantedPerson_{crimeTypeId}");
            await _cacheCleaner.RemoveByPatternAsync("AllCrimes");
            await _cacheCleaner.RemoveByPatternAsync("AllWantedPersons");
        }
    }
}
