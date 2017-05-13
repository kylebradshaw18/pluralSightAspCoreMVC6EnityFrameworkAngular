using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using TheWorld.ViewModels;
using AutoMapper;

namespace TheWorld.Controllers.Api
{
	[Route("api/trips")]
	public class TripsController : Controller
	{
		private IWorldRepository _repository;

		public TripsController(IWorldRepository repository)
		{
			_repository = repository;
		}

		[HttpGet("")]
		public IActionResult Get()
		{
			var results = _repository.GetAllTrips();
			return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
		}

		[HttpPost("")]
		public IActionResult Post([FromBody] TripViewModel theTrip)
		{
			if (ModelState.IsValid)
			{
				//Save to the database
				var newTrip = Mapper.Map<Trip>(theTrip);

				return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
			}

			return BadRequest(ModelState);
		}

	}
}
