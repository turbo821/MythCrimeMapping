﻿using Microsoft.AspNetCore.Mvc;
using Application.UseCases.GetAllCrimes;
using Application.UseCases.GetCrime;
using Application.UseCases.CreateCrime;
using Application.UseCases.UpdateCrime;
using Application.UseCases.DeleteCrime;
using Domain.Models;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/crime-marks")]
    public class CrimeMarkApiController : ControllerBase
    {
        private readonly IGetAllCrimesUseCase _getAllCrimes;
        private readonly ICreateCrimeUseCase _createCrime;
        private readonly IGetCrimeUseCase _getCrime;
        private readonly IUpdateCrimeUseCase _updateCrime;
        private readonly IDeleteCrimeUseCase _deleteCrime;
        private readonly IHubContext<RealHub> _hubContext;

        public CrimeMarkApiController(
            IGetAllCrimesUseCase getAllCrime, 
            ICreateCrimeUseCase createCrime, 
            IGetCrimeUseCase getCrime, 
            IUpdateCrimeUseCase updateCrime,
            IDeleteCrimeUseCase deleteCrime,
            IHubContext<RealHub> hubContext)
        {
            _getAllCrimes = getAllCrime; 
            _createCrime = createCrime;
            _getCrime = getCrime;
            _updateCrime = updateCrime;
            _deleteCrime = deleteCrime;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> ShowAllCrimeMarks([FromQuery] CrimeFilterRequest filterRequest)
        {
            var response = await _getAllCrimes.Handle(filterRequest);

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ShowCrimeMark(Guid id)
        {
            var crimeDto = await _getCrime.Handle(id);

            if (crimeDto == null)
            {
                return NotFound(new { Message = $"Crime with ID {id} not found." });
            }

            return Ok(crimeDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCrimeMark([FromBody] CreateCrimeRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "You are not logged in" });
            }

            if (!ModelState.IsValid || userId != request.CreatorId.ToString())
                return BadRequest(ModelState);

            var response = await _createCrime.Handle(request);
            if (response is null)
                return BadRequest(response);

            return CreatedAtAction(nameof(ShowCrimeMark), new { id = response.Id }, response);
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> UpdateCrimeMark([FromBody] UpdateCrimeRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "You are not logged in" });
            }

            if (!ModelState.IsValid || userId != request.EditorId.ToString())
                return BadRequest(ModelState);

            var response = await _updateCrime.Handle(request);

            if (response is null)
                return BadRequest(response);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveCrimeMark(Guid id) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "You are not logged in" });
            }

            var response = await _deleteCrime.Handle(id);
            if(!response)
                return NotFound();

            return Ok();
        }
    }
}