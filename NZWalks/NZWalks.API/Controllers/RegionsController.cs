using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await _regionRepository.GetAllAsync();
            
            // Return Region DTOs, instead of Domain Model

            //var regionsDto = new List<Models.DTO.Region>();
            
            //regions.ToList().ForEach(domainRegion =>
            //{
            //    var regionDto = new Models.DTO.Region()
            //    {
            //        Id = domainRegion.Id,
            //        Population = domainRegion.Population,
            //        Area = domainRegion.Area,
            //        Code = domainRegion.Code,
            //        Lat = domainRegion.Lat,
            //        Long = domainRegion.Long,
            //        Name = domainRegion.Name
            //    };

            //    regionsDto.Add(regionDto);
            //});

            var regionsDto = mapper.Map<List<Models.DTO.Region>>(regions);
            
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await _regionRepository.GetAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDto);
        }

        [HttpPost]
        [Route("AddRegion")]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                Console.WriteLine("Code can't be empty");
                return BadRequest();
            }
            var doesItAlreadyExist = await _regionRepository.GetByNameAsync(addRegionRequest.Name);

            if (doesItAlreadyExist != null)
            {
                Console.WriteLine($"Region with name {addRegionRequest.Name} already exists");
                return BadRequest();
            }

            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population,
            };

            region = await _regionRepository.AddAsync(region);

            var regionDto = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDto.Id }, regionDto);
        }

        [HttpDelete]
        [Route("id:guid")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            var wasItDeleted = await _regionRepository.DeleteAsync(id);
            if (wasItDeleted != null)
            {
                var regionDto = new Models.DTO.Region()
                {
                    Code = wasItDeleted.Code,
                    Name = wasItDeleted.Name,
                    Area = wasItDeleted.Area,
                    Lat = wasItDeleted.Lat,
                    Long = wasItDeleted.Long,
                    Population = wasItDeleted.Population,
                };

                return Ok(regionDto);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //Convert DTO to Domain model
            var regionDomain = new Models.Domain.Region()
            {
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };

            //Update Region using repository
            var updatedRegion = await _regionRepository.UpdateAsync(id, regionDomain);

            //If null, then not found
            if (updatedRegion == null) { 
                return NotFound();
            }

            //If not null, convert domain back to DTO
            var regionDto = new Models.DTO.Region()
            {
                Id = id,
                Area = updatedRegion.Area,
                Lat = updatedRegion.Lat,
                Long = updatedRegion.Long,
                Name = updatedRegion.Name,
                Population = updatedRegion.Population,
                Code = updatedRegion.Code,
            };

            //Return Ok response
            return Ok(regionDto);
        }
    }
}
