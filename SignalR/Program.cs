using System;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SignalR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();

            //System.Console.WriteLine("Hello, World!");
            //// Create Client instance
            //MqttClient myClient = new MqttClient(MqttBroker); //Cannot access from static?

            //// Register to message received
            //myClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            //string clientId = Guid.NewGuid().ToString();
            //myClient.Connect(clientId, Username, Password); //Cannot access from static?

            //// Subscribe to topic
            //myClient.Subscribe(new String[] { "/homeassistant/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            //System.Console.ReadLine();
        }


        //static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        //{
        //    // Handle message received
        //    var message = System.Text.Encoding.Default.GetString(e.Message);
        //    var topic = e.Topic;

        //    System.Console.WriteLine($"{topic}: {message} ");
        //}


        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}