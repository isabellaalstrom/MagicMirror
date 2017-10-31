using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MagicMirror.Models;

namespace MagicMirror.Services
{
    public class GCalendarService
    {
        private static readonly string[] Scopes = { CalendarService.Scope.CalendarReadonly };

        public List<CalendarItem> GetCalendar()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                var credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/google_calendar.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                var message = ("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            // Define parameters of request.
            var request = service.Events.List("e0f9nl8p2pprjn7sab05v4ad5s@group.calendar.google.com");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 5;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            var events = request.Execute();
            var upcomingEvents = new List<CalendarItem>();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (string.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }

                    var cEvent = new CalendarItem
                    {
                        Start = when,
                        Summary = eventItem.Summary
                    };
                    upcomingEvents.Add(cEvent);
                }
            }
            return upcomingEvents;
        }
    }
}
