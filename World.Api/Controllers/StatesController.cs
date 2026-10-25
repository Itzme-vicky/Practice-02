using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using World.Api.Data;
using World.Api.DTO.Country;
using World.Api.DTO.States;
using World.Api.Models;
using World.Api.Repository;
using World.Api.Repository.IRepository;


namespace World.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly IStateRepository _stateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StatesController> _logger;

        public StatesController(IStateRepository stateRepository, IMapper mapper, ILogger<StatesController> logger  )
        {
            _stateRepository = stateRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StatesDto>>> GetAll()
        {
            var states =await _stateRepository.GetAll(); 

            if (states == null)
            {
                return Conflict("No Data in the Database..!");
            }

            var StatesDto = _mapper.Map<List<StatesDto>>(states);

            return Ok(StatesDto);
        }

        [HttpGet("id:int")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StatesDto>> GetById(int id)
        {
            var state = await _stateRepository.GetById(id);

            if (state == null)
            {
                _logger.LogError($"Error While tring to get id : {id}");
                return Conflict($"Id :{id} not exits in the database..!");
            }
            var StatesDto = _mapper.Map<StatesDto>(state);

            return Ok(StatesDto);


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CreateStatesDto>> Create(CreateStatesDto createStatesDto)
        {
            var states =  _stateRepository.IsRecordExsits(x=>x.Name.ToLower().Trim()==createStatesDto.Name.ToLower().Trim());

            if (states)
            {
                return Conflict($"The {createStatesDto.Name} is already Exist in the database...!");
            }
            var StatesDto = _mapper.Map <States> (createStatesDto);

            await _stateRepository.Create(StatesDto);
            return CreatedAtAction("GetById", new { id = createStatesDto.Id }, StatesDto);
        

        }
        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> Update(int id, [FromBody] UpdateStatesDto updateStatesDto)
        {
            if (updateStatesDto.Id != id || updateStatesDto == null)
            {
                return BadRequest($"Input :{id} And States Id :{updateStatesDto.Id} both not equal...!");
            }

          
          var states=_mapper.Map<States> (updateStatesDto);
            //  _dbContext.Entry(result).CurrentValues.SetValues(states);
           await _stateRepository.Update(states);

           return NoContent();
        
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<ActionResult> Delete(int id) 
        {
            var result = await _stateRepository.GetById(id);

            if (result==null)
            {
                return Conflict($"Id  :{id} not found in database...!");
            }
            await _stateRepository.Delete(result);
            
            return NoContent();
        }


    }
}
