using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;

namespace THFHA_V1._0.MyMqttClient
{
    public class MyMqttClient
    {
        private IManagedMqttClient MqttClient;
        private MqttClientOptionsBuilder clientOptions;
        private ManagedMqttClientOptions managedClientOptions;

        private string MqttUrl { get; set; }
        private string MqttUsername { get; set; }
        private string MqttPassword { get; set; }

        public event EventHandler MqttConnected;
        public event EventHandler MqttDisconnected;
        public static MyMqttClient Instance { get; private set; }

        static MyMqttClient()
        {
            Instance = new MyMqttClient();
        }

        private MyMqttClient()
        {
            // Your existing constructor logic, if any
        }

        public void SetConnectionParameters(string mqttUrl, string mqttUsername, string mqttPassword)
        {
            MqttUrl = "mqtt://"+mqttUrl;
            MqttUsername = mqttUsername;
            MqttPassword = mqttPassword;
        }

        public async Task ConnectAsync()
        {
            if (!Uri.TryCreate(MqttUrl, UriKind.Absolute, out Uri? uri) || uri is null)
                return;

            switch (uri.Scheme)
            {
                case "mqtt":
                    clientOptions = new MqttClientOptionsBuilder().WithTcpServer(uri.Host, uri.Port);
                    break;

                case "mqtts":
                    clientOptions = new MqttClientOptionsBuilder().WithTcpServer(uri.Host, uri.Port).WithTls();
                    break;

                case "ws":
                    clientOptions = new MqttClientOptionsBuilder().WithWebSocketServer(uri.ToString());
                    break;

                case "wss":
                    clientOptions = new MqttClientOptionsBuilder().WithWebSocketServer(uri.ToString()).WithTls();
                    break;

                default:
                    return;
            }

            if (!string.IsNullOrWhiteSpace(MqttUsername) && !string.IsNullOrEmpty(MqttPassword))
            {
                clientOptions = clientOptions.WithCredentials(MqttUsername, MqttPassword);
            }

            managedClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(clientOptions.Build())
                .Build();

            MqttClient = new MqttFactory().CreateManagedMqttClient();

            MqttClient.ConnectedAsync += (e) => { MqttConnected?.Invoke(MqttClient, EventArgs.Empty); return Task.CompletedTask; };
            MqttClient.DisconnectedAsync += (e) => { MqttDisconnected?.Invoke(MqttClient, EventArgs.Empty); return Task.CompletedTask; };

            await MqttClient.StartAsync(managedClientOptions);

            // Add your subscriptions here.
            // await MqttClient.SubscribeAsync($"your/subscription/topic");
        }

        public async Task DisconnectAsync()
        {
            if (MqttClient is not null && MqttClient.IsConnected)
                await MqttClient.StopAsync();
        }
        public async Task PublishAsync(string topic, string payload, bool retain = false)
        {
            if (MqttClient == null)
            {
                throw new InvalidOperationException("MqttClient is not initialized. Call ConnectAsync() before using PublishAsync.");
            }

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag(retain)
                .Build();

            await MqttClient.EnqueueAsync(message);
        }

    }

}
