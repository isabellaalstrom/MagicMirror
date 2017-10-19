using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MagicMirror
{
    public class ReportsPublisher : Hub
    {
        public Task PublishReport(string reportName)
        {
            return Clients.All.InvokeAsync("OnReportPublished", reportName);
        }
    }
}