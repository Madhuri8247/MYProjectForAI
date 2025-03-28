using Microsoft.AspNetCore.Mvc;
using OnlineNews.Services;

namespace OnlineNews.Views.Shared.Components
{
    public class ElectricityPricesViewComponent:ViewComponent
    {
        private readonly IRequestService _requestService;
        public ElectricityPricesViewComponent(IRequestService requestService)
        {
            _requestService = requestService;
        }
        public async  Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _requestService.GetData();
            return View(data);
        }
        
    }
}
