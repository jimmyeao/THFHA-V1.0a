using Newtonsoft.Json;
using Serilog;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using THFHA_V1._0.Model;

namespace THFHA_V1._0.Views
{
    public partial class hasettings : Form
    {
        private Settings settings;
        private static HttpClient? httpClient;
        public hasettings()
        {
            InitializeComponent();
            this.settings = Settings.Instance;
            richTextBox1.Text = settings.Hatoken;
            tb_haurl.Text = settings.Haurl;

        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                richTextBox1.Text += (string)Clipboard.GetData("Text");
                e.Handled = true;
                settings.Save();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            settings.Hatoken = richTextBox1.Text;
            settings.Save();
        }

        private void tb_haurl_TextChanged(object sender, EventArgs e)
        {
            settings.Haurl = tb_haurl.Text;
            settings.Save();
        }

        private void btn_test_Click(object sender, EventArgs e)
        {
            string message = "";
            string title = "";
            if (string.IsNullOrWhiteSpace(settings.Haurl)) settings.Haurl = "http://homeassistant.local:8123";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            tb_haurl.Text = settings.Haurl;
            // clear the httpclient before testing
            httpClient = new HttpClient();
            string err = "";
            httpClient.BaseAddress = new Uri(settings.Haurl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Hatoken);
            try
            {
                var response = httpClient.GetAsync("/api/states").Result;
                response.EnsureSuccessStatusCode();
                var json = response.Content.ReadAsStringAsync().Result;
                GetHAConfig();
                //return "HA Connected!";
                message = "Homeassistant is connected ";
                title = "Home Assistant Check";
                MessageBox.Show(message, title);
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                    // Handle the custom exception.
                    if (ex is HttpRequestException)
                    {
                        err = "Error: " + ex.Message;
                        // Rethrow any other exception.
                        //return err;
                        message = "Homeassistant connection Failed " + ex.Message;
                        title = "Home Assistant Check";
                        MessageBox.Show(message, title);
                        httpClient.Dispose();
                    }
                    // Rethrow any other exception.
                    else
                    {
                        
                        httpClient.Dispose();
                        throw ex;
                    }
                Log.Error("Error checking HA connectivity {ae}", ae);
                message = "Homeassistant connection Failed " + ae.ToString;
                title = "Home Assistant Check";
                MessageBox.Show(message, title);
            }
            catch (HttpRequestException httpEx)
            {
                if (httpEx.StatusCode.HasValue)
                {
                    err = httpEx.StatusCode.Value.ToString();
                    httpClient.Dispose();
                    //return "Error: " + err;
                    message = "Homeassistant errored " + err;
                    title = "Home Assistant Check";
                    MessageBox.Show(message, title);
                }
                Log.Error("Error checking HA connectivity {httpEx}", httpEx);
                httpClient.Dispose();
                //return "Error: " + httpEx.Message;
                message = "Homeassistant Error " + httpEx.Message;
                title = "Home Assistant Check";
                MessageBox.Show(message, title);
            }
        }
        private async void GetHAConfig()
        {

            var wsURL = settings.Haurl;
            if (wsURL.StartsWith("https"))
            {
                wsURL = "wss" + wsURL.Substring(5);
            }
            else if (wsURL.StartsWith("http"))
            {
                wsURL = "ws" + wsURL.Substring(4);
            }

            var client = new ClientWebSocket();
            client.Options.UseDefaultCredentials = true;
            try
            {
                await client.ConnectAsync(new Uri(wsURL), CancellationToken.None);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error connecting to Home Assistant");
            }


            var receiveBuffer = new ArraySegment<byte>(new byte[1024]);
            while (client.State == WebSocketState.Open)
            {
                var receiveResult = await client.ReceiveAsync(receiveBuffer, CancellationToken.None);
                var receivedData = Encoding.UTF8.GetString(receiveBuffer.Array, 0, receiveResult.Count);
                var message = JsonConvert.DeserializeObject<Dictionary<string, object>>(receivedData);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (message.ContainsKey("type") && message["type"].ToString() == "auth_ok")
                {
                    var sendData = "{\"type\":\"config/core\",\"access_token\":\"" + settings.Hatoken + "\"}";
                    var sendBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(sendData));
                    await client.SendAsync(sendBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else if (message.ContainsKey("type") && message["type"].ToString() == "result" &&
                         message.ContainsKey("result"))
                {
                    var config = message["result"];
                    Console.WriteLine("Configuration: " + config);
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            client.Dispose();
        }
    }
}
