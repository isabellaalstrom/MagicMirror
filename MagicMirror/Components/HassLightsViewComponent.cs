using System.Threading.Tasks;
using MagicMirror.Services;
using Microsoft.AspNetCore.Mvc;

namespace MagicMirror.Components
{
    public class HassLightsViewComponent : ViewComponent
    {
        private readonly IHassService _hassService;

        public HassLightsViewComponent(IHassService hassService)
        {
            _hassService = hassService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _hassService.GetGroupEntitiesAsync("group.inside_lights_card");
            return View(result);
        }
    }
}