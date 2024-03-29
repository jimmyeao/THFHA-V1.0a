﻿using MQTTnet;
using MQTTnet.Client;
using System.Text.RegularExpressions;
using THFHA_V1._0.Model;

namespace THFHA_V1._0.Views
{
    public partial class mqttsettings : Form
    {
        #region Private Fields

        private Settings settings;

        #endregion Private Fields

        #region Public Constructors

        public mqttsettings()
        {
            InitializeComponent();
            settings = Settings.Instance;
            tb_mqttip.Text = settings.Mqttip;
            tb_mqttuser.Text = settings.Mqttusername;
            tb_mqttpass.Text = settings.Mqttpassword;
            tb_mqtttopic.Text = settings.Mqtttopic;
        }

        #endregion Public Constructors

        #region Private Methods

        private async void buttontest_Click(object sender, EventArgs e)
        {
            // Let's validate its at least a valid IP.
            if (!Regex.IsMatch(tb_mqttip.Text, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
            {
                var title = "IP Error";
                var message = "Please enter a valid IP address.";
                MessageBox.Show(message, title);
                settings.IsMqttModuleSettingsValid = false;
                return;
            }

            // Create a new MQTT client instance.
            var factory = new MqttFactory();
            var client = factory.CreateMqttClient();

            // Set up the client options.
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(tb_mqttip.Text)
                .WithCredentials(tb_mqttuser.Text, tb_mqttpass.Text)
                .WithClientId("test-client")
                .Build();

            try
            {
                // Try to connect to the MQTT server.
                await client.ConnectAsync(options);

                // Connection successful.
                var title = "Connection Successful";
                var message = "Successfully connected to the MQTT server.";
                MessageBox.Show(message, title);
                settings.IsMqttModuleSettingsValid = true;
                // Disconnect from the MQTT server.
                await client.DisconnectAsync();
            }
            catch (Exception ex)
            {
                // Connection failed.
                var title = "Connection Error";
                settings.IsMqttModuleSettingsValid = false;
                var message = $"Failed to connect to the MQTT server. Error: {ex.Message}";
                MessageBox.Show(message, title);
            }
        }

        private void mqttip_TextChanged(object sender, EventArgs e)
        {
            settings.Mqttip = tb_mqttip.Text;
            settings.Save();
        }

        private void mqttpass_TextChanged(object sender, EventArgs e)
        {
            settings.Mqttpassword = tb_mqttpass.Text;
            settings.Save();
        }

        private void mqtttopic_TextChanged(object sender, EventArgs e)
        {
            settings.Mqtttopic = tb_mqtttopic.Text;
            settings.Save();
        }

        private void mqttuser_TextChanged(object sender, EventArgs e)
        {
            settings.Mqttusername = tb_mqttuser.Text;
            settings.Save();
        }

        #endregion Private Methods
    }
}