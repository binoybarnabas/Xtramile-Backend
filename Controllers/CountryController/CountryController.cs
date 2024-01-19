﻿using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.CountryService;

namespace XtramileBackend.Controllers.CountryControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryServices _countryServices;

        public CountryController(ICountryServices countryServices)
        {

            _countryServices = countryServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountriesAsync()
        {
            try
            {
                IEnumerable<TBL_COUNTRY> countryData = await _countryServices.GetCountriesAsync();
                return Ok(countryData);
            }
            catch (Exception ex)
            {               
                return StatusCode(500, "An error occurred while retrieving countries.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCountryAsync([FromBody] TBL_COUNTRY country)
        {
            try
            {
                await _countryServices.AddCountryAsync(country);
                return Ok(country);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding a country.");
            }
        }


    }
}
