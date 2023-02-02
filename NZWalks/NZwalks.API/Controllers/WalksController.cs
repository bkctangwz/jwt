using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZwalks.API.Repositories;

namespace NZwalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() 
        {
            //fetch data from domain

            var walksDomain = await walkRepository.GetAllAsync();

            //convert domain to dto walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);
            //if (walksDomain == null)
            //{
            //    return NotFound();
            //}


            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            //get walks domain
            var walkDomain = await walkRepository.GetByIdAsync(id);

            //convert walks from domaion to dto
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            //if (walkDomain == null)
            //{
            //    return NotFound();
            //}
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody]Models.DTO.AddWalkRequest addWalkRequest)
        {
            //convert walk from dto request to domain
            var walkDomain = new Models.Domain.Walk()
            {
                length = addWalkRequest.length,
                Name= addWalkRequest.Name,
                RegionId=addWalkRequest.RegionId,
                WalkDifficultyId=addWalkRequest.WalkDifficultyId
            };

            //pass domain to repository
            walkDomain = await walkRepository.AddAsync(walkDomain);

            //convert back from domain to dto
            var walkDTO = new Models.DTO.Walk() //mapper.Map<Models.DTO.Walk>(walkDomain);
            {
                Id = walkDomain.Id,
                length = walkDomain.length,
                Name=walkDomain.Name,
                RegionId=walkDomain.RegionId,
                WalkDifficultyId=walkDomain.WalkDifficultyId
            };
            return CreatedAtAction(nameof(GetByIdAsync), new { id = walkDTO.Id}, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute]Guid id, [FromBody]Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //convert to domain object
            var walkDomain = new Models.Domain.Walk
            {
                length = updateWalkRequest.length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            //pass to repository
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            //handle null (not found)
            if (walkDomain == null)
            {
                return NotFound();
            }

            //convert back to walk DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var walkDomain = await walkRepository.DeleteAsync(id);
            if(walkDomain == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDTO);
        }
    }
}
