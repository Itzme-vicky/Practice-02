using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using World.Api.Data;
using World.Api.DTO.Country;
using World.Api.Models;
using World.Api.Repository.IRepository;

namespace World.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper= mapper;
        }



        [HttpGet]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAll()
        {
            var countries = await _countryRepository.GetAll();

            //Mapper config
           

            if (countries == null)
            {
                return NoContent(); 
            }
            var countriesDto = _mapper.Map<List<CountryDto>>(countries);

            return Ok(countriesDto);
        }




        [HttpGet("(id:int)")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CountryDto>> GetById(int id) 
        {

           var countries =  await _countryRepository.GetById(id);
            var countriesDto=_mapper.Map<CountryDto>(countries);

            if (countries == null)
            {
               // return NoContent();
                return Conflict($"Id :{id} not exist in the Database ...!");
            
            }

            return Ok(countriesDto);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateCountryDto>> Create([FromBody]CreateCountryDto createCountryDto) 
        {
            var result = _countryRepository.IsCountryExists(createCountryDto.Name);

            if (result)
            {
                return Conflict("Country Already Exit in Database...!");
            
            }

            //Mapper
            var country = _mapper.Map<Country>(createCountryDto);


           await _countryRepository.Create(country);
            return CreatedAtAction("GetById",new { id = country.Id },country);
        
        
        }


        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Country>> Update(int id,[FromBody] UpdateCountryDto countryDto)
        { 
            if (countryDto == null || countryDto.Id!= id)
            {
                return BadRequest("Id Values not Matched From Input and Body...!");
            }

            //var countryFromDb = _dbContext.Countries.Find(id);

            //if (countryFromDb == null)
            //{
            //    return NotFound($"Id : {id} Not Found in the Database...!");
            //}


            //countryFromDb.Name = country.Name;
            //countryFromDb.ShortName = country.ShortName;
            //countryFromDb.CountryCode = country.CountryCode;

            //Mapper
            var country =_mapper.Map<Country>(countryDto);

           await  _countryRepository.Update(country);

            return NoContent();
        
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest("Id value start with greaterthan 0 ");
            }

            var country =await _countryRepository.GetById(id);

            //if (country == null)
            //{
            //    return NotFound($"Id : {id} Not Found in the Database...!");
            //}

             await _countryRepository.Delete(country);
            return NoContent();
        
        }

    }
}
