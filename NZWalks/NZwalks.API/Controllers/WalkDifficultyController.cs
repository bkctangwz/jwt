using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZwalks.API.Models.DTO;
using NZwalks.API.Repositories;

namespace NZwalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepos walkDifficultyRepos;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepos walkDifficultyRepos, IMapper mapper)
        {
            this.walkDifficultyRepos = walkDifficultyRepos;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var walksDifficulty = await walkDifficultyRepos.GetAllAsync();
            if (walksDifficulty == null)
            {
                return NotFound();
            }

            var walksDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walksDifficulty);

            return Ok(walksDifficultyDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepos.GetAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);  
            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //convert from dto to domain
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Id = new Guid(),
                Code = addWalkDifficultyRequest.Code
            };

            //pass details to repository
            walkDifficultyDomain = await walkDifficultyRepos.AddAsync(walkDifficultyDomain);

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = walkDifficultyDTO.Id}, walkDifficultyDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //convert from dto to domain object
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code
            };

            //pass details to repository
            walkDifficultyDomain = await walkDifficultyRepos.UpdateAsync(id, walkDifficultyDomain);

            //handle null
            if (walkDifficultyDomain == null) { return NotFound(); }

            //convert back from domain to dto
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            //return results
            return Ok(walkDifficultyDTO);

        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepos.DeleteAsync(id);
            if(walkDifficulty == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficultyDTO);
        }
    }
}
