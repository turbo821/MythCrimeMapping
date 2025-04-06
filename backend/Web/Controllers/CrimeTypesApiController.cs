using Application.UseCases.CreateCrimeType;
using Application.UseCases.DeleteCrimeType;
using Application.UseCases.SelectAllCrimeTypes;
using Application.UseCases.GetCrimeType;
using Application.UseCases.UpdateCrimeType;
using Microsoft.AspNetCore.Mvc;
using Application.UseCases.GetAllCrimeTypes;
using Application.Pagination;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Application.Services.Interfaces;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/crime-types")]
    public class CrimeTypesApiController : ControllerBase
    {
        private readonly ISelectAllCrimeTypesUseCase _selectAllCrimeTypes;
        private readonly IGetAllCrimeTypesUseCase _getAllCrimeTypes;
        private readonly IGetCrimeTypeUseCase _getCrimeType;
        private readonly ICreateCrimeTypeUseCase _createCrimeType;
        private readonly IUpdateCrimeTypeUseCase _updateCrimeType;
        private readonly IDeleteCrimeTypeUseCase _deleteCrimeType;
        private readonly IMemoryCache _cache;
        private readonly ICacheKeyTracker _cacheKeyTracker;

        public CrimeTypesApiController(
            ISelectAllCrimeTypesUseCase selectAllCrimeTypes,
            IGetAllCrimeTypesUseCase getAllCrimeTypes,
            IGetCrimeTypeUseCase getCrimeType,
            ICreateCrimeTypeUseCase createCrimeType,
            IUpdateCrimeTypeUseCase updateCrimeType,
            IDeleteCrimeTypeUseCase deleteCrimeType,
            IMemoryCache cache,
            ICacheKeyTracker cacheKeyTracker)
        {
            _selectAllCrimeTypes = selectAllCrimeTypes;
            _getAllCrimeTypes = getAllCrimeTypes;
            _getCrimeType = getCrimeType;
            _createCrimeType = createCrimeType;
            _updateCrimeType = updateCrimeType;
            _deleteCrimeType = deleteCrimeType;
            _cache = cache;
            _cacheKeyTracker = cacheKeyTracker;
        }

        [HttpGet("titles")]
        public async Task<IActionResult> SelectAllCrimeTypes()
        {
            var cacheKey = "AllCrimeTypes";
            if (!_cache.TryGetValue(cacheKey, out var response))
            {
                response = await _selectAllCrimeTypes.Handle();


                _cache.Set(cacheKey, response, TimeSpan.FromMinutes(10));
                _cacheKeyTracker.AddKey(cacheKey);
            }
                
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCrimeTypes([FromQuery] PaginationSearchParameters request)
        {
            var cacheKey = $"AllCrimeTypes_{request.GetCacheKey()}";
            if (!_cache.TryGetValue(cacheKey, out var response))
            {
                response = await _getAllCrimeTypes.Handle(request);

                _cache.Set(cacheKey, response, TimeSpan.FromMinutes(10));
                _cacheKeyTracker.AddKey(cacheKey);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCrimeType(Guid id)
        {
            var cacheKey = $"CrimeType_{id}";
            if (!_cache.TryGetValue(cacheKey, out var response))
            {
                response = await _getCrimeType.Handle(id);

                if (response == null)
                {
                    return NotFound(new { Message = $"Crime type with ID {id} not found." });
                }

                _cache.Set(cacheKey, response, TimeSpan.FromMinutes(10));
            }

            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCrimeType([FromBody] CreateCrimeTypeRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "You are not logged in" });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _createCrimeType.Handle(request);

            if(response is null)
                return BadRequest(new { Message = "A type of crime with this title already exists." });

            ClearCrimeCache(response.Id);

            return CreatedAtAction(nameof(GetCrimeType), new { Id = response.Id }, response);
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateCrimeType([FromBody] UpdateCrimeTypeRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "You are not logged in" });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _updateCrimeType.Handle(request);

            if (response is null)
                return BadRequest(new { Message = "Not found a type of crime with this id." });

            ClearCrimeCache(response.Id);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveCrimeType(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "You are not logged in" });
            }
            
            var response = await _deleteCrimeType.Handle(id);
            if(!response)
                return NotFound();

            ClearCrimeCache(id);

            return Ok();
        }

        private void ClearCrimeCache(Guid crimeTypeId)
        {
            _cache.Remove($"CrimeType_{crimeTypeId}");

            var keys = _cacheKeyTracker.GetKeys("AllCrimeTypes");

            foreach (var key in keys)
            {
                _cache.Remove(key);
                _cacheKeyTracker.RemoveKey(key);
            }
        }
    }
}
