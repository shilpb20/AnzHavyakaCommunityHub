using AutoMapper;
using CommunityHub.Core;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Factory;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.Api.Controllers
{
    public class ContactUsController : ControllerBase
    {
        private readonly ILogger<ContactUsController> _logger;
        private readonly IMapper _mapper;
        private readonly IResponseFactory _responseFactory;

        private readonly IContactService _contactService;

        public ContactUsController(
            ILogger<ContactUsController> logger,
            IMapper mapper,
            IResponseFactory responseFactory,
            IContactService contactService)
        {
            _logger = logger;
            _mapper = mapper;
            _responseFactory = responseFactory;
            _contactService = contactService;
        }


        [HttpPost(ApiRoute.ContactUs.SubmitEnquiry)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ContactFormDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<ContactFormDto>>> AddEnquiry([FromBody] ContactFormCreateDto contactFormCreateDto)
        {
            if (contactFormCreateDto == null)
                return BadRequest(_responseFactory.Failure<ContactFormDto>(ErrorCode.InvalidData, $"Value cannot be null or empty for contact form object"));

            var contactForm = _mapper.Map<ContactForm>(contactFormCreateDto);
            var newForm = await _contactService.CreateAddUserEnquiryAsync(contactForm);

            var responseObject = _responseFactory.Success(_mapper.Map<ContactForm>(newForm));
            return Created();
        }
    }
}
