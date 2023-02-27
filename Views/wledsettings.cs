using Kevsoft.WLED;
using Newtonsoft.Json;
using System.ComponentModel;
using THFHA_V1._0.Model;
using Zeroconf;
using Serilog;
using System.Windows.Forms;
using THFHA_V1._0.Model;

namespace THFHA_V1._0.Views
{



    public partial class wledsettings : Form
    {
        private Settings settings;
        private WLED _selectedWled;
        private bool isDiscovering = false;
        public event EventHandler<WLED>? DeviceDiscovered;
        private BindingList<WLED> _wledLights = new BindingList<WLED>();
        public List<WLED> WledLights { get { return _wledLights.OfType<WLED>().ToList(); } }

        public wledsettings()
        {
            InitializeComponent();
            this.settings = Settings.Instance;
            listbox_wledlights.DataSource = null;
            listbox_wledlights.DataSource = _wledLights;
            // Load the selected item from the settings object and select it in the listbox.
            if (_selectedWled != null)
            {
                listbox_wledlights.SelectedValue = _selectedWled.Ip + ":" + _selectedWled.Port;
            }

            var selectedItem = this.settings.SelectedWled;
            if (settings.WledDevices != null)
            {
                _wledLights = new BindingList<WLED>(settings.WledDevices); ;
                listbox_wledlights.DataSource = _wledLights;
                listbox_wledlights.DisplayMember = "Name";
            }
            if (selectedItem != null)
            {
                foreach (var wled in _wledLights)
                {
                    if (wled.Ip == selectedItem.Ip && wled.Port == selectedItem.Port)
                    {
                        listbox_wledlights.SelectedItem = wled;

                        break;
                    }
                }
            }

        }

        private async void btn_discover_Click(object sender, EventArgs e)
        {
            if (isDiscovering)
            {
                return;
            }

            isDiscovering = true;

            try
            {
                Log.Information("Starting Discovery of WLED lights");
                toolStripStatusLabel1.Text = "Discovering Lights";
                // Clear the existing list of WLED lights.
                _wledLights.Clear();

                try
                {
                    var results = await ZeroconfResolver.ResolveAsync("_wled._tcp.local.");
                    foreach (var result in results)
                    {
                        var host = result.DisplayName;
                        var ipAddress = result.IPAddress;
                        var port = result.Services.FirstOrDefault(x => x.Key == "_wled._tcp").Value?.Port ?? 80;

                        var wled = new WLED()
                        {
                            Name = host,
                            Ip = ipAddress,
                            Port = port
                        };

                        var client = new WLedClient($"http://{wled.Ip}:{wled.Port}");
                        var wLedRootResponse = client.Get();

                        if (wLedRootResponse.Status != null)
                        {
                            HttpClient myclient = new HttpClient();
                            try
                            {
                                HttpResponseMessage response = await myclient.GetAsync($"http://{wled.Ip}/json");

                                if (response.IsSuccessStatusCode)
                                {
                                    string json = await response.Content.ReadAsStringAsync();
                                    try
                                    {
                                        dynamic jsonObject = JsonConvert.DeserializeObject(json);
                                        string name = jsonObject.name;
                                        wled.Name = host;
                                        wled.Ip = ipAddress;
                                        wled.Port = port;
                                        if (!_wledLights.Contains(wled))
                                        {
                                            Log.Information("Found a new Wled light called {name}!", wled.Name);
                                            _wledLights.Add(wled);
                                            _wledLights = new BindingList<WLED>(_wledLights.OrderBy(wled => wled.Name).Distinct().ToList());

                                            // Update the DataSource property of the listbox control to reflect the changes in the _wledLights list.
                                            listbox_wledlights.DataSource = null;
                                            listbox_wledlights.DataSource = _wledLights;
                                            listbox_wledlights.DisplayMember = "Name";

                                            // Select the stored selected item in the listbox.
                                            var selectedItem = this.settings.SelectedWled;
                                            if (selectedItem != null && selectedItem.Ip == wled.Ip && selectedItem.Port == wled.Port)
                                            {
                                                listbox_wledlights.SelectedItem = wled;
                                            }

                                            DeviceDiscovered?.Invoke(this, wled);
                                        }
                                    }
                                    catch
                                    {
                                        //wasn't a wled device
                                    }
                                    // use the name variable to access the name of the device
                                }
                            }
                            catch (Exception ex)
                            {
                                //wasn't a wled device
                                Log.Error("Exception on Discovery of WLED lights {ex}", ex.Message.ToString());
                            }
                        }
                    }
                    settings.WledDevices = WledLights;
                    settings.Save();

                }
                catch (Exception ex)
                {
                    Log.Information("Exception on Discovery of WLED lights {ex}", ex.Message.ToString());
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
            }
            finally
            {
                isDiscovering = false;
                toolStripStatusLabel1.Text = "Stopped Discovering Lights";
            }
        }

        private void listbox_wledlights_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listbox_wledlights.SelectedItem is WLED selectedWled)
            {
                this.settings.SelectedWled = selectedWled;
                this.settings.Save();
            }
            settings.WledDevices = WledLights;
            settings.Save();
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            isDiscovering = false;
            toolStripStatusLabel1.Text = "Stopped Discovering Lights";
        }
    }
}
