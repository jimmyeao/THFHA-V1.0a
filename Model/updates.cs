using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Serilog;
using System.Windows.Forms;


namespace THFHA_V1._0.Model
{
    public class UpdateChecker
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly string currentVersion; // Replace with your current app version
        public UpdateChecker()
        {
            // Get the current version from the assembly
            currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        public async Task CheckForUpdatesAsync()
        {
            string owner = "jimmyeao";
            string repo = "THFHA-V1.0a";
            string apiUrl = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
            
        
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("AppName");
            HttpResponseMessage response = null;

            try
            {
                response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Log.Error(e, "Error checking for updates");
            }

            if (response != null && response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(jsonString);
                string latestVersion = json["tag_name"].ToString().TrimStart('v');

                if (CompareVersions(currentVersion, latestVersion) < 0)
                {
                    string message = $"A new version of the app is available: {latestVersion}\nClick OK to download the latest version.";
                    string caption = "Update Available";
                    DialogResult result = MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    if (result == DialogResult.OK)
                    {
                        // Open the URL in the default browser
                        string url = json["html_url"].ToString();
                       
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });
                    }
                }
                else
                {
                   Log.Information("Your app is up to date.");
                }
            }
        }

        private int CompareVersions(string version1, string version2)
        {
            Version v1 = new Version(version1);
            Version v2 = new Version(version2);

            return v1.CompareTo(v2);
        }

    }
}
