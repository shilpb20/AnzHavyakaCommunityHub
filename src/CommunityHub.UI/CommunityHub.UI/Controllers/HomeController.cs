using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Factory;
using CommunityHub.Core.Models;
using CommunityHub.UI.Constants;
using CommunityHub.UI.Models;
using CommunityHub.UI.Services;
using CommunityHub.UI.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CommunityHub.UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _service;
    private readonly IResponseFactory _responseFactory;

    public HomeController(
        ILogger<HomeController> logger, 
        IResponseFactory responseFactory,
        IUserService userService)
    {
        _logger = logger;
        _service = userService;
        _responseFactory = responseFactory;
    }
   
    [Authorize(Policy = "AuthenticatedUser")]
    [HttpGet(UiRoute.Home.Index)]
    public async Task<IActionResult> Index(string? sortBy = null, bool ascending = true)
    {
        ApiResponse<List<UserInfoDto>> users = await _service.GetAllUsers(sortBy, ascending);
        ViewBag.SelectedSortBy = sortBy;
        ViewBag.SelectedAscending = ascending;
        return View(users.Data);
    }

    [HttpGet(UiRoute.Home.Privacy)]
    public IActionResult Privacy()
    {
        return View();
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
