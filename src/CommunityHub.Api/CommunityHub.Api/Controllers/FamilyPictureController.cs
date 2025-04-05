using AutoMapper;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos.RegistrationInfo.UserInfo;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CommunityHub.Api.Controllers
{
    public class FamilyPictureController : ControllerBase
    {
        private readonly ILogger<FamilyPictureController> _logger;
        private readonly IMapper _mapper;

        private readonly IFamilyPictureService _service;

        public FamilyPictureController(
            ILogger<FamilyPictureController> logger,
            IMapper mapper,
            IFamilyPictureService service)
        {
            _logger = logger;
            _mapper = mapper;

            _service = service;
        }

        [HttpGet(ApiRoute.FamilyPicture.ById)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FamilyPictureDto))]
        public async Task<ActionResult<FamilyPictureDto>> AddFamilyPicture([FromBody]FamilyPictureCreateDto pictureDto)
        {
            var matchingPicture = await _service.GetFamilyPictureByPathAsync(pictureDto.ImageUrl);
            if (matchingPicture != null)
                return BadRequest("Duplicate request. Path already exists. " +
                    "Update the existing picture or create one with a different path.");

            var model = _mapper.Map<FamilyPicture>(pictureDto);
            var familyPicture = _service.CreateFamilyPictureAsync(model);

            var familyPictureDto = _mapper.Map<FamilyPictureDto> (familyPicture);
            return CreatedAtRoute("GetFamilyPicture", new { Id = familyPictureDto.Id }, familyPictureDto);
        }

        //[HttpGet(ApiRoute.FamilyPicture.ById, Name = "GetFamilyPicture")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FamilyPictureDto)]
        //public async Task<ActionResult<<FamilyPictureDto>> GetImageUrl(int id)
        //{
        //    var familyPicture = await _service.GetFamilyPictureByIdAsync(id);
        //    if (familyPicture == null)
        //        return NotFound();

        //    var familyPictureDto = _mapper.Map<FamilyPictureDto>(familyPicture);
        //    return Ok(familyPictureDto);
        //}

        //        [HttpGet(ApiRoute.FamilyPicture.ById, Name = "GetFamilyPicture")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status200OK, Type=typeof(FamilyPictureDto)]
        //public async Task<ActionResult<<FamilyPictureDto>> GetImageUrl(int id)
        //{
        //    var familyPicture = await _service.GetFamilyPictureByIdAsync(id);
        //    if (familyPicture == null)
        //        return NotFound();

        //    var familyPictureDto = _mapper.Map<FamilyPictureDto>(familyPicture);
        //    return Ok(familyPictureDto);
        //}
    }
}
