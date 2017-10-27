using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalR
{
    public class ReportsPublisher : Hub
    {
        public Task PublishReport(string reportName)
        {
            return Clients.All.InvokeAsync("OnReportPublished", reportName);
        }
    }
}
