﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MagicMirror.Data;
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

        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ICredentialsRepository _credentialsRepo;
        private readonly IConfiguration _configuration;

        public MqttService(IConfiguration config, IHubContext<SignalRHub> hubContext, ICredentialsRepository credentialsRepo)
        {
            _configuration = config;
            _hubContext = hubContext;
            _credentialsRepo = credentialsRepo;
        }

        //private string MqttBroker => _configuration["MqttBroker"];
        //private string Username => _configuration["MqttUsername"];
        //private string Password => _configuration["MqttPassword"];
        private string MqttBroker => _credentialsRepo.GetMqttBroker();
        private string Username => _credentialsRepo.GetMqttUsername();
        private string Password => _credentialsRepo.GetMqttPassword();

        public void Subscribe()
        {
            // Create Client instance
            MqttClient myClient = new MqttClient(MqttBroker);

            // Register to message received
            myClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            myClient.Connect(clientId, Username, Password);

            // Subscribe to topic
            string[] topics = { "/homeassistant/sensor/#" };
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
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
            var entity = new HassEntity();

            if (topicSplit[4] == "state")
            {
                entity.EntityId = topicSplit[3];
                entity.State = message;
                if (entity.EntityId == "dark_sky_summary" 
                    || entity.EntityId.Contains("dark_sky_precip") && !entity.EntityId.Contains("probability"))
                {
                hub.InvokeAsync("OnIconUpdate", entity);
                }
                else if (entity.EntityId.StartsWith("dark_sky"))
                {
                    hub.InvokeAsync("OnWeatherUpdate", entity);
                }
                else if (entity.EntityId.EndsWith("door"))
                {
                    hub.InvokeAsync("OnDoorUpdate", entity);
                }
            }
            //else if (topicSplit[4] == "friendly_name")
            //{
            //    entity.EntityId = topicSplit[3];
            //    entity.Attributes.FriendlyName = message;
            //}
            //else if (topicSplit[4] == "entity_picture")
            //{
            //    entity.EntityId = topicSplit[3];
            //    entity.Message = message;
            //    hub.InvokeAsync("OnIconUpdate", entity);

            //}
        }
    }
}
