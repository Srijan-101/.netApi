using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalksController : ControllerBase
    {     
          private readonly IMapper mapper;
          private readonly IwalkRepository walkRepository;
          public WalksController(IMapper mapper,IwalkRepository walkRepository)
          {  
               this.mapper = mapper;
               this.walkRepository = walkRepository;
          }
          
         //Create Walk
         //POST : /api/walks
         [HttpPost]
         public async Task<ActionResult<WalkDto>> Create([FromBody] AddWalkRequestDto addWalkRequestDto){
              //Map Dto to Domain model
              var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
              await walkRepository.CreateAsync(walkDomainModel);

              //Map Domain model to Dto
              return Ok(mapper.Map<WalkDto>(walkDomainModel));
         }

         //Get Walks
         //GET : /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
         [HttpGet]
         public async Task<ActionResult<WalkDto>> GetAll([FromQuery] string? filterOn
         ,[FromQuery] string? filterQuery,[FromQuery] string? sortBy ,[FromQuery] bool? isAscending,
          [FromQuery] int pageNumber = 1 , [FromQuery] int pageSize = 1000
         ){

              var walksDomainModel =  await walkRepository.GetAllAsync(filterOn,filterQuery,sortBy
              ,isAscending ?? true,pageNumber,pageSize);

              return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
         }

         //Get Walk by Id
         //Get :/api/Walks{id}
         [HttpGet]
         [Route("{id:Guid}")]
         public async Task<IActionResult> GetById([FromRoute] Guid id){
              var walkDomainModel = await walkRepository.GetByIdAsync(id);
              if(walkDomainModel == null) {
                  return NotFound();
              }
              //Map domain model to Dto
              return Ok(mapper.Map<WalkDto>(walkDomainModel));
         }

         //Update Walk by Id
         //PUT :/api/walks/{id}
         [HttpPut]
         [Route("{id:Guid}")]
         public async Task<IActionResult> Update([FromRoute] Guid id,UpdateWalkRequestDto updateWalkRequestDto) {

               //Map DTO to Domain Model
               var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
               walkDomainModel = await walkRepository.UpdateAsync(id,walkDomainModel);
               if(walkDomainModel == null) {
                    return NotFound();
               }
               //Map Domain Model to DTO
               return Ok(mapper.Map<WalkDto>(walkDomainModel));
         }

         //Delete a walk by an Id
         //DELETE : /api/Walks/{id}
         [HttpDelete]
         [Route("{id:Guid}")]
         public async Task<IActionResult> Delete([FromRoute] Guid id){
             var deletedWalkDomainModel =  await walkRepository.DeleteAsync(id);
             if(deletedWalkDomainModel == null) {
                return NotFound();
             }
             //Map Domain model to DTO
             return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
         }

    }
}