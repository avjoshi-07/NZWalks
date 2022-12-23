using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
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
        
        [HttpGet()]
        public async Task<IActionResult> GetAllRegions()
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
    }
}
