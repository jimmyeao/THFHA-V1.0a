using Serilog;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using THFHA_V1._0.Model;

namespace THFHA_V1._0.Views
{
    public partial class hatchersettings : Form
    {
        private Settings settings;                  //set up our settings
        private static HttpClient? httpClient;      //set up for a http client

        public hatchersettings()
        {
            InitializeComponent();
            settings = Settings.Instance;      //instantiate our settings
            textBox1.Text = settings.Hatcherip;     //load the settings into the form
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            settings.Hatcherip = textBox1.Text;
            settings.Save();
        }

        private async void btn_test_Click(object sender, EventArgs e)
        {
            string ipAddress = textBox1.Text;
            if (!Regex.IsMatch(ipAddress, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
            {
                var title = "IP Error";
                var message = "Please enter a valid IP address.";
                MessageBox.Show(message, title);
                return;
            }
            Log.Information("Testing Connection");
            toolStripStatusLabel1.Text = "Testing Connection";
            int port = 5000; // hatcher server's port number

            try
            {
                // Create a new TCP client and connect to the server on a separate thread
                await Task.Run(() =>
                {
                    try
                    {
                        using (TcpClient client = new TcpClient(ipAddress, port))
                        {
                            // If the connection is successful, the server is running and listening

                            Log.Information("The server is running and listening on {0}:{1}", ipAddress, port);
                            var title = "Connection Success";
                            var message = "The server is running and listening on " + ipAddress + ", " + port;
                            MessageBox.Show(message, title);
                            toolStripStatusLabel1.Text = message;
                        }

                    }
                    catch (Exception ex)
                    {
                        // Display a user-friendly error message
                        var title = "Connection Error";
                        var message = "An error occurred while trying to connect to the server: " + ex.Message;
                        MessageBox.Show(message, title);
                        toolStripStatusLabel1.Text += message;

                        // Log the exception for debugging purposes
                        Log.Error(ex, "An error occurred while trying to connect to the server.");
                    }
                });

                // Close the connection
                Log.Information("Connection closed.");

            }
            catch (Exception ex)
            {
                // Display a user-friendly error message
                var title = "Connection Error";
                var message = "An error occurred while trying to connect to the server: " + ex.Message;
                MessageBox.Show(message, title);

                // Log the exception for debugging purposes
                Log.Error(ex, "An error occurred while trying to connect to the server.");
            }

        }



    }
}
