using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MagicMirror.Services
{
    public class MqttService
    {
        private readonly IConfiguration _configuration;

        public MqttService(IConfiguration config)
        {
            _configuration = config;
        }

        private string MqttBroker => _configuration["MqttBroker"];
        private string Username => _configuration["MqttUsername"];
        private string Password => _configuration["MqttPassword"];


        //public void Subscribe()
        //{
        //    // create client instance 
        //    MqttClient client = new MqttClient("test.mosquitto.org");
        //        //(IPAddress.Parse(MqttBroker));

        //    // register to message received 
        //    client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

        //    string clientId = Guid.NewGuid().ToString();
        //    client.Connect(clientId);

        //    // subscribe to the topic "/home/temperature" with QoS 2 
        //    var test = client.Subscribe(
        //        new[] { "/homeassistant/" }, 
        //        new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }
        //        );
        //}

        //public void Publish()
        //{
        //    // create client instance 
        //    MqttClient client = new MqttClient(IPAddress.Parse(MqttBroker));

        //    string clientId = Guid.NewGuid().ToString();
        //    client.Connect(clientId);

        //    string strValue = Convert.ToString(value);

        //    // publish a message on "/home/temperature" topic with QoS 2 
        //    client.Publish("/home/temperature", Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        //}


        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received 
            // access data bytes throug e.Message
            var dataBytes = e.Message;
        }
    }
}
