using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        { 
            this.imageRepository = imageRepository;
        }
        //POST: /api/Images
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request){
              ValidateFileUpload(request);
              if(ModelState.IsValid)
                {
                    //Convert DTO to Domain model
                      var imageDomainModel = new Image
                       {
                          File = request.File,
                          FileExtension = Path.GetExtension(request.File.FileName),
                          FileSizeInBytes = request.File.Length,
                          FileName = request.FileName,
                          FileDescription = request.FileDescription
                       };

                    //User repository to upload Image
                    await imageRepository.Upload(imageDomainModel);

                    return Ok(imageDomainModel);

                }
             return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request) {
            var allowedExtension = new string[] {".jpg" , ".jpeg" , ".png"};
            if(!allowedExtension.Contains(Path.GetExtension(request.File.FileName))) {
                   ModelState.AddModelError("File" , "Unsupported file extension");
            }

            if(request.File.Length > 10485769) {
                ModelState.AddModelError("File" , "File is more than 10MB.");
            }
        }
    }
}