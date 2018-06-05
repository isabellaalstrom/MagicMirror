using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MagicMirror.Data;
using MagicMirror.Extensions;
using MagicMirror.Models;
using MagicMirror.Services.HassWebSocket.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharp;
using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;
using EventData = MagicMirror.Services.HassWebSocket.Models.EventData;

namespace MagicMirror.Services.HassWebSocket
{
    public class HassWebSocketService
    {
        //public event EventHandler<LogEventArgs> TraceOutput;
        //public event EventHandler<LogEventArgs> DebugOutput;
        //public event EventHandler<LogEventArgs> InfoOutput;
        //public event EventHandler<LogEventArgs> WarnOutput;
        //public event EventHandler<LogEventArgs> ErrorOutput;

        private bool _inited;
        private WebSocket _ws;
        private IRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly ICredentialsRepository _credentialsRepo;


        public HashSet<string> EncounteredEntityIdsWithoutSubscription { get; set; } = new HashSet<string>();
        private bool _started;
        private readonly SignalRHub _hub;
        private readonly List<HassEntity> _entities;

        //private string HassWebsocketUri => _configuration["HassWebsocketUri"];
        private string HassWebsocketUri => $"ws://{_credentialsRepo.GetHassBaseUrl()}/api/websocket";
        //private string ApiPasswordQuery => _configuration["ApiPasswordQuery"];
        private string ApiPasswordQuery => $"?api_password={_credentialsRepo.GetHassPassword()}"; //?api_password={password}

        public HassWebSocketService(IRepository repo, SignalRHub hub, IConfiguration config, ICredentialsRepository credentialsRepo)
        {
            _repo = repo;
            _hub = hub;
            _configuration = config;
            _credentialsRepo = credentialsRepo;
            _entities = _repo.GetEntities();
        }

        public void Start()
        {
            if (!_inited)
                Initialize();
            if (_started)
                throw new InvalidOperationException($"{nameof(HassWebSocketService)} already started!");

            if (string.IsNullOrEmpty(ApiPasswordQuery))
            {
                _ws = new WebSocket(HassWebsocketUri);
            }
            else
            {
                _ws = new WebSocket($"{HassWebsocketUri}{ApiPasswordQuery}");
            }
            _ws.Log.Output += OnWebsocketLog;
            _ws.OnError += OnError;
            _ws.OnMessage += OnMessage;
            _ws.Connect();

            SubscribeToEvents();

            _started = true;
        }

        public void Stop()
        {
            if (_ws == null)
                return;

            if (_ws.ReadyState == WebSocketState.Connecting || _ws.ReadyState == WebSocketState.Open)
                _ws.Close();

            _ws = null;
            _started = false;
        }

        private void Initialize()
        {
            _inited = true;
        }

        private void SubscribeToEvents()
        {
            var cmd1 = new SubscribeToEventsCommand(EventType.state_changed, 1);
            var cmd2 = new SubscribeToEventsCommand(EventType.click, 2);
            _ws.Send(JsonConvert.SerializeObject(cmd1));
            _ws.Send(JsonConvert.SerializeObject(cmd2));
        }

        private async void OnMessage(object sender, MessageEventArgs e)
        {
            if (!e.IsText && !e.Data.IsValidJson())
                return;

            var json = JToken.Parse(e.Data);

            if (json.IsAuthMessage())
            {
                //InfoOutput?.Invoke(this, new LogEventArgs { Text = $"Authorize message: {e.Data.ToPrettyJson()}" });
                return;
            }

            if (json.IsResult())
            {
                // Isn't an event, log and exit.
                //DebugOutput?.Invoke(this, new LogEventArgs { Text = $"Result message: {e.Data.ToPrettyJson()}" });
                return;
            }

            if (!json.IsEvent())
            {
                // Isn't an event! And event's are what we're working with.
                //WarnOutput?.Invoke(this, new LogEventArgs { Text = $"Unsupported message (not an 'event'): {e.Data.ToPrettyJson()}" });
                return;
            }

            var entId = json.ExtractEntityId().ToLowerInvariant();

            var eventData = new EventData { EntityId = entId };

            if (json.IsClickEvent())
            {
                eventData.ClickData = new Click { ClickType = (string)json["event"]["data"]["click_type"] };
            }

            if (json.IsStateChangeEvent())
            {
                if (_entities.Any(x=>x.EntityId == entId && x.SelectedEntity))
                {
                    if (!json.HasNewState())
                        return;

                    var rawGraph = JsonConvert.DeserializeObject<HassEventRawModel>(e.Data);
                    var stateChange = new StateChanged();
                    stateChange.NewState = rawGraph.@event.data.new_state?.state;
                    stateChange.OldState = rawGraph.@event.data.old_state?.state;
                    stateChange.Attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                        (rawGraph.@event.data.new_state ?? rawGraph.@event.data.old_state ?? new StateRaw()).attributes
                        .ToString());
                    eventData.StateChangeData = stateChange;

                    var hassEntity = _repo.GetEntity(entId);
                    hassEntity.State = stateChange.NewState;
                    await _hub.SendHassEntitiesToView(hassEntity);
                }
            }
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            //ErrorOutput?.Invoke(this, new LogEventArgs { Text = e.Message, Exception = e.Exception });
        }

        private void OnWebsocketLog(LogData data, string arg2)
        {
            //TraceOutput?.Invoke(this, new LogEventArgs { Text = data.Message + " [arg2]" });
        }

    }
}
