using Basics.IntrApi.Client.AutoRefresh.Models;
using Basics.IntrApi.Client.AutoRefresh.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basics.IntrApi.Client.AutoRefresh.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        private readonly ApiInvokerService _apiInvokerService;

        public DataController(ApiInvokerService apiInvokerService)
        {
            _apiInvokerService = apiInvokerService;
        }

        public async Task<IActionResult> Index()
        {
            DataResponse? response = await _apiInvokerService.Get("/api/data");
            return View(response);
        }
    }
}
