using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicMirror.Services;
using Microsoft.AspNetCore.Mvc;

namespace MagicMirror.Components
{
    public class GoogleCalendarViewComponent : ViewComponent
    {
        private readonly GCalendarService _service;

        public GoogleCalendarViewComponent(GCalendarService service)
        {
            _service = service;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var calendar = _service.GetCalendar();
          
            return View(calendar);
        }
    }
}
