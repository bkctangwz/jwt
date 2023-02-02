using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NZwalks.API.Repositories;

namespace NZwalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepos walkDifficultyRepos;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepos walkDifficultyRepos)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepos = walkDifficultyRepos;
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
            //validate input data
            if(!(await ValidateAddAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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
            //validate data
            if(!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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

        #region Private methods
        private async Task<bool> ValidateAddAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest),
                    $"{nameof(addWalkRequest)} requires input data.");
                return false;
            }

            if (String.IsNullOrEmpty(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} cannot be null or empty");
            }

            if(addWalkRequest.length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.length), $"{nameof(addWalkRequest.length)} cannot be less than or equal to zero.");
            }

            //validate region id
            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is not valid.");
            }

            //validate walkdifficulty id
            var walkDifficulty = await walkDifficultyRepos.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is not valid.");
            }

            if(ModelState.ErrorCount> 0)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest),
                    $"{nameof(updateWalkRequest)} requires input data.");
                return false;
            }

            if (String.IsNullOrEmpty(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} cannot be null or empty");
            }

            if (updateWalkRequest.length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.length), $"{nameof(updateWalkRequest.length)} cannot be less than or equal to zero.");
            }

            //validate region id
            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is not valid.");
            }

            //validate walkdifficulty id
            var walkDifficulty = await walkDifficultyRepos.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)} is not valid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
