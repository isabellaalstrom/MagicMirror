using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MagicMirror.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MagicMirror.Services
{
    public class MqttService
    {
        public List<HassEntity> Entities = new List<HassEntity>();

        private readonly IHubContext<ReportsPublisher> _hubContext;
        private readonly IConfiguration _configuration;

        public MqttService(IConfiguration config, IHubContext<ReportsPublisher> hubContext)
        {
            _configuration = config;
            _hubContext = hubContext;
        }

        private string MqttBroker => _configuration["MqttBroker"];
        private string Username => _configuration["MqttUsername"];
        private string Password => _configuration["MqttPassword"];

        public void Subscribe()
        {
            // Create Client instance
            MqttClient myClient = new MqttClient(MqttBroker);

            // Register to message received
            myClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            myClient.Connect(clientId, Username, Password);

            // Subscribe to topic
            string[] topics = {"/homeassistant/light/#", "/homeassistant/sensor/#"};
            byte[] qosLevels = {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
            myClient.Subscribe(topics, qosLevels);
            //Console.ReadLine();
        }


        public void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // Handle message received
            var message = Encoding.Default.GetString(e.Message);
            var topic = e.Topic;

            SaveEntityState(topic, message);

            Console.WriteLine($"{topic}: {message} ");
        }

        private void SaveEntityState(string topic, string message)
        {
            var hub = _hubContext.Clients.All;

            var topicSplit = topic.Split("/");
            var entity = new HassEntity
            {
                EntityId = topicSplit[3],
                State = message
            };
            //Entities.Add(entity);
            if (entity.EntityId.EndsWith("door"))
            {
                hub.InvokeAsync("OnDoorUpdate", entity);
            }
            if (entity.EntityId.StartsWith("dark_sky"))
            {
                hub.InvokeAsync("OnWeatherUpdate", entity);

                //if (entity.EntityId.EndsWith("_1"))
                //{
                //    hub.InvokeAsync("OnForcastUpdate", entity);
                //}
            }

        }
    }
}
