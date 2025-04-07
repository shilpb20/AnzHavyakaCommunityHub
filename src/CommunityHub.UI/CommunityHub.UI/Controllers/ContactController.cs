using CommunityHub.Core.Dtos;
using CommunityHub.UI.Services.Contact;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.UI.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IContactService _contactService;

        public ContactController(
            ILogger<ContactController> logger,
            IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }

        [HttpGet(UiRoute.Contact.Index)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost(UiRoute.Contact.Index)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ContactFormCreateDto contactFormDto)
        {
            if (!ModelState.IsValid)
            {
                return View(contactFormDto);
            }

            try
            {
                var result = await _contactService.SendUserEnquiryAsync(contactFormDto);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Your enquiry has been successfully submitted.";
                    return RedirectToAction(nameof(ThankYou));
                }
                else
                {
                    TempData["ErrorMessage"] = "There was an error submitting your enquiry. Please try again.";
                    return View(contactFormDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while submitting the contact form.");
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return View(contactFormDto);
            }
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
