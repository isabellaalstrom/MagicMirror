using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MagicMirror.Models;
using Microsoft.Extensions.Configuration;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MagicMirror.Services
{
    public class MqttService
    {
        public List<HassEntity> Entities = new List<HassEntity>();

        private readonly IConfiguration _configuration;

        public MqttService(IConfiguration config)
        {
            _configuration = config;
        }

        private string MqttBroker => _configuration["MqttBroker"];
        private string Username => _configuration["MqttUsername"];
        private string Password => _configuration["MqttPassword"];

        public void Subscribe()
        {
            //BuildWebHost(args).Run();

            //Console.WriteLine("Hello, World!");
            // Create Client instance
            MqttClient myClient = new MqttClient(MqttBroker); //Cannot access from static?

            // Register to message received
            myClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            myClient.Connect(clientId, Username, Password); //Cannot access from static?

            // Subscribe to topic
            myClient.Subscribe(new String[] { "/homeassistant/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
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
            var topicSplit = topic.Split("/");
            var entity = new HassEntity
            {
                EntityId = $"{topicSplit[2]}.{topicSplit[3]}",
                State = message
            };
            Entities.Add(entity);
        }

        //public List<HassEntity> GetAllEntities()
        //{
            
        //}
    }
}
