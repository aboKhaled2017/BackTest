using Backend.DataModels;
using Backend.DtoModels;
using Backend.Repos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NamesProcessingController : ControllerBase
    {
        private readonly ILogger<NamesProcessingController> _logger;
        private readonly INamesProcessingRepo _nameRepository;
        public NamesProcessingController(ILogger<NamesProcessingController> logger, INamesProcessingRepo nameRepository)
        {
            _logger = logger;
            _nameRepository = nameRepository;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Tags("Insert 10 Random Names")]
        public async Task<IActionResult> INsert10RandomNames([FromBody] InsertRandomNamesReq req)
        {

            _logger.LogInformation($"trying to insert 10 names");

            await _nameRepository.Insert10NamesAsync(req.Names);

            _logger.LogInformation($"10 names  inserted to database successully");

            return CreatedAtAction(nameof(GetAllNames),null);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Tags("Get Inseretd Random Names")]
        public async Task<IActionResult> GetAllNames()
        {

            _logger.LogInformation($"trying to retrive all inserted names");

             var data=await _nameRepository.GetAllNamesSortedAsync();

            _logger.LogInformation($"the 10 names fetched successfully");

            return Ok(data);
        }

        [HttpGet("{index:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Tags("Get Names'Spelling Alphapetized By Name Index")]
        public async Task<IActionResult> GetSpellingAlphpetized(int index)
        {

            _logger.LogInformation($"trying to Get Names'Spelling Alphapetized By Name Index {index}");

            var name = await _nameRepository.GetNameSpellingAlphapetized(index);

            if (name is null)
                return NotFound($"cannot find name with index {index}");

            _logger.LogInformation($"the name {name.Value} is alphapetized successfully");

            return Ok(name.Value.Alpahapetize());
        }

    }
}
