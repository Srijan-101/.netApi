using System.Reflection;
using System.Security.Permissions;
using System.Text.Json;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.CutomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {    
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository RegionRepository;
        private readonly ILogger<RegionsController> logger;
        private readonly IMapper Imapper;
       
        public RegionsController(
        NZWalksDbContext dbContext,
        IRegionRepository regionRepository,
        IMapper Imapper,
        ILogger<RegionsController> logger
        )
        {
              this.dbContext = dbContext;
              this.Imapper = Imapper;
              this.RegionRepository = regionRepository;
              this.logger = logger;
        }
        
        //GET ALL REGIONS
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll() {

            logger.LogInformation("GetAll Region Action Method was invoked");

            //Get data From Database - Domain models 
            var regions = await RegionRepository.GetAllAsync();
            //Map Domain models to DTOs
            // var regionsDto = new List<RegionDto>();
            // foreach (var region in regions)
            // {
            //      regionsDto.Add(new RegionDto() {
            //          Id = region.Id,
            //          Code = region.Code,
            //          Name = region.Name,
            //          RegionImageUrl = region.RegionImageUrl
            //      });
            // }
            
            //Map Domain model to DTOs
            var regionsDto = Imapper.Map<List<RegionDto>>(regions);

            logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDto)}");

            return Ok(regionsDto); 
        }
        
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id){

            //Get region Domain model
            var region = await RegionRepository.GetByIdAsync(id);
            // var region = dbContext.Regions.FirstOrDefault( x=> x.Id == id);
            if(region == null) {
                 return NotFound();
            } 
            
            //Map domain model to DTOs
            // var RegionDTO = new RegionDto
            // {
            //          Id = region.Id,
            //          Code = region.Code,
            //          Name = region.Name,
            //          RegionImageUrl = region.RegionImageUrl
            // };

            var RegionDTO = Imapper.Map<List<RegionDto>>(region);
            return Ok(RegionDTO);
        }

        //POST to Create New Region
        //POST:
        [HttpPost]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto){
              
            if(ModelState.IsValid)  {
                          //Map DTO to domain Model
            var RegionDomainModel = Imapper.Map<Region>(addRegionRequestDto);

            //  var RegionDomainModel = new Region
            //  {
            //       Code = addRegionRequestDto.Code,
            //       Name = addRegionRequestDto.Name,
            //       RegionImageUrl = addRegionRequestDto.RegionImageUrl
            //  };
             
             //Use Domain Model to create Region
            //  await dbContext.Regions.AddAsync(RegionDomainModel);
            //  await dbContext.SaveChangesAsync();

             RegionDomainModel = await RegionRepository.CreateAsync(RegionDomainModel);

             //Map Domain model back to DTO
             var regionDto = Imapper.Map<Region>(RegionDomainModel);

            //  var regionDto = new RegionDto
            //  {
            //      Id = RegionDomainModel.Id,
            //      Code = RegionDomainModel.Code,
            //      Name = RegionDomainModel.Name,
            //      RegionImageUrl = RegionDomainModel.RegionImageUrl

            //  };
             return CreatedAtAction(nameof(GetById), new {id = RegionDomainModel.Id},regionDto);
            } else {
              return BadRequest(ModelState);
            }
       
        }

        //Update Region
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        { 
            //Map DTO to Domain Model
            var regionDomainModel = new Region
            {
                 Code = updateRegionRequestDto.Code,
                 Name = updateRegionRequestDto.Name,
                 RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            };

            regionDomainModel = await RegionRepository.UpdateAsync(id,regionDomainModel);
            if(regionDomainModel == null) 
            {
                return NotFound();
            }
            //convert domain model into DTO
            var RegionDto = new RegionDto
            {
                 Id = regionDomainModel.Id,
                 Code = regionDomainModel.Code,
                 Name = regionDomainModel.Name,
                 RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(RegionDto);
        } 


        [HttpDelete] 
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid id){

            var RegionDomainModel =  await RegionRepository.DeleteAsync(id);
            if(RegionDomainModel == null){
                 return NotFound();
            }
            //return deleted Region back
            //map domain Model to DTO back
              var RegionDto = new RegionDto
            {
                 Id = RegionDomainModel.Id,
                 Code = RegionDomainModel.Code,
                 Name = RegionDomainModel.Name,
                 RegionImageUrl = RegionDomainModel.RegionImageUrl
            };
            return Ok(RegionDto);
        }
    }
} 