using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Serilog;
using System.Text.RegularExpressions;
using THFHA_V1._0.Model;

namespace THFHA_V1._0.Views
{
    public partial class huesettings : Form
    {
        private Settings settings;

        public huesettings()
        {

            InitializeComponent();

            settings = Settings.Instance;
            tb_hueip.Text = settings.Hueip;
            // Set the selected item in the list box to the previously selected light
            if (!string.IsNullOrEmpty(settings.SelectedLightId))
            {
                var selectedItem = settings.HueLight.FirstOrDefault(l => l.Id == settings.SelectedLightId);
                if (selectedItem != null)
                {
                    cb_huelights.Items.Clear();
                    cb_huelights.DataSource = settings.HueLight;
                    cb_huelights.DisplayMember = "Name";
                    cb_huelights.SelectedItem = selectedItem;
                }
            }
            // Wire up the SelectedIndexChanged event handler for the list box
            cb_huelights.SelectedIndexChanged += cb_huelights_SelectedIndexChanged;


        }


        public async void LinkHub()
        {
            if (settings.Hueip != "")
            {
                ILocalHueClient client = new LocalHueClient(settings.Hueip);

                //Make sure the user has pressed the button on the bridge before calling RegisterAsync
                //It will throw an LinkButtonNotPressedException if the user did not press the button
                var result = MessageBox.Show("Please press the button on the hub before proceeding", "Press button on hub",
                    MessageBoxButtons.OKCancel);
                cb_huelights.DataSource = null;
                cb_huelights.Items.Clear();

                cb_huelights.DisplayMember = "Name";
                if (result == DialogResult.OK)
                    try
                    {
                     
                        
                        if (settings.HueLight != null)
                        {
                            settings.HueLight.Clear();
                        }
                        settings.HueLight = new List<CustomLight>(); // change this to use your custom light class
                        Log.Information("Starting Hue linking to hub with ip {ip}", settings.Hueip.ToString());
                        var appKey = await client.RegisterAsync("TeamsMonitor", "THAM");
                        if (appKey != null)
                        {
                            settings.Hueusername = appKey; //Save the app key for later use
                        }

                        Log.Information("got Hue Key..");
                        var lights = await client.GetLightsAsync();
                        foreach (var currentLight in lights)
                        {
                            settings.HueLight.Add(new CustomLight(currentLight));

                            Log.Information("Added a Hue light called {light}", currentLight.Name);
                            settings.IsHueModuleSettingsValid= true;
                        }
                        if (cb_huelights.Items.Count == 0) // check if the list box is already populated
                        {
                            PopulateListBox();
                        }

                        foreach (var light in lights)
                        {
                            if (!settings.HueLight.Any(l => l.Id == light.Id))
                            {
                                settings.HueLight.Add(new CustomLight(light));
                                Log.Information("Added a Hue light called {light}", light.Name);
                            }
                        }
                        MessageBox.Show("Hub Linked", "Success!", MessageBoxButtons.OK);
                      
                        settings.Save();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hub Linking", "Linked failed with error " + ex.Message, MessageBoxButtons.OK);
                        Log.Information("An error occurred linking Hue hub " + ex.Message);
                        settings.IsHueModuleSettingsValid = false;
                        settings.Save();
                    }
            }
            else
            {
                var result = MessageBox.Show("Enter the IP of your Bridge First", "Wetware issue",
                    MessageBoxButtons.OKCancel);
            }
        }

        private void tb_hueip_TextChanged(object sender, EventArgs e)
        {
            settings.Hueip = tb_hueip.Text;
            settings.Save();

        }
        private void PopulateListBox()
        {
            cb_huelights.Items.Clear();
            cb_huelights.DataSource = settings.HueLight;
            cb_huelights.DisplayMember = "Name";
        }


        private void btn_link_Click(object sender, EventArgs e)
        {
            // lets validate its at least a valid IP...
            if (Regex.IsMatch(tb_hueip.Text, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
            {
                //valid ip continue
                LinkHub();
            }
            else
            {
                var title = "IP Error";
                var message = "Please Enter a valid ip address";
                MessageBox.Show(message, title);
            }
        }

        private void cb_huelights_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedLight = cb_huelights.SelectedItem as CustomLight;
            if (selectedLight != null)
            {
                settings.SelectedLightId = selectedLight.Id;
                settings.Save();
            }
        }
    }
}
