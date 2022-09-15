using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Repository;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class GamesController : Controller
    {
        public static IEFGameRepository _gameRepository;
        public GamesController(IEFGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        [HttpGet()]
        public IEnumerable<vGame> Get()
        {
            return _gameRepository.Get();
        }

        [HttpGet("{id}")]
        public vGame Get(int id)
        {
            return _gameRepository.Get(id);
        }

        [HttpPost]
        public IActionResult Post([FromBody] vGame vGame)
        {
            if (vGame == null){ return BadRequest(); }

            return _gameRepository.Create(vGame) ? Ok() : BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            return _gameRepository.Delete(id) ? Ok() : BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] vGame _vGame)
        {
            return _gameRepository.Edit(id, _vGame) ? Ok() : BadRequest();
        }

        [HttpPut("sort")]
        public IEnumerable<vGame> Put([FromBody] Sort _sort)
        {
            return _gameRepository.GetSortedGames(_sort);
        }
    }
}
