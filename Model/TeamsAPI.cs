using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Serilog;
using System.Net.WebSockets;
using System.Text;

namespace THFHA_V1._0.Model
{
    public class WebSocketClient
    {
        #region Private Fields

        private readonly ClientWebSocket _clientWebSocket;
        private State state;

        #endregion Private Fields

        #region Public Constructors


        public WebSocketClient(Uri uri, State state)
        {
            _clientWebSocket = new ClientWebSocket();
            this.state = state;
            _ = ConnectAsync(uri);

            // Subscribe to the MessageReceived event
            MessageReceived += OnMessageReceived;
        }


        #endregion Public Constructors

        #region Public Events

        public event EventHandler<string> MessageReceived;
        
        #endregion Public Events

        #region Public Methods

        public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await _clientWebSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, cancellationToken);
            Console.WriteLine($"Message sent: {message}");
        }

        #endregion Public Methods

        #region Private Methods


        private async Task ConnectAsync(Uri uri)
        {
            try
            {
                DateTimeOffset timestamp = DateTimeOffset.UtcNow;
                long unixTimestamp = timestamp.ToUnixTimeMilliseconds();
                await _clientWebSocket.ConnectAsync(uri, CancellationToken.None);
                Console.WriteLine("WebSocket connected");
                string message = "{\"apiVersion\":\"1.0.0\",\"service\":\"query-meeting-state\",\"action\":\"query-meeting-state\",\"manufacturer\":\"Jimmyeao\",\"device\":\"THFHA\",\"timestamp\":" + unixTimestamp.ToString() + "}";
                //string message = "{\"apiVersion\":\"1.0.0\",\"service\":\"query-meeting-state\",\"action\":\"query-meeting-state\",\"manufacturer\":\"Jimmyeao\",\"device\":\"THFHA\",\"timestamp\":1675341655453}";
                await SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred: " + ex.Message);
                Log.Error("Using URI: " + uri.ToString());
            }

            await ReceiveLoopAsync();
        }
        private Dictionary<string, object> meetingState = new Dictionary<string, object>()
        {
            { "isMuted", false },
            { "isCameraOn", false },
            { "isHandRaised", false },
            { "isInMeeting", "Not in a meeting" },
            { "isRecordingOn", false },
            { "isBackgroundBlurred", false },
        };

        private async Task ReceiveLoopAsync(CancellationToken cancellationToken = default)
        {
            
            const int bufferSize = 4096; // Starting buffer size
            byte[] buffer = new byte[bufferSize];
            int totalBytesReceived = 0;

            while (_clientWebSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer, totalBytesReceived, buffer.Length - totalBytesReceived), cancellationToken);
                totalBytesReceived += result.Count;

                if (result.EndOfMessage)
                {
                    string messageReceived = Encoding.UTF8.GetString(buffer, 0, totalBytesReceived);
                    Console.WriteLine($"Message received: {messageReceived}");

                    MessageReceived?.Invoke(this, messageReceived);

                    // Reset buffer and totalBytesReceived for next message
                    buffer = new byte[bufferSize];
                    totalBytesReceived = 0;
                }
                else if (totalBytesReceived == buffer.Length) // Resize buffer if it's too small
                {
                    Array.Resize(ref buffer, buffer.Length + bufferSize);
                }
            }
        }
        private void OnMessageReceived(object sender, string message)
        {
        // Update the Message property of the State class
        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new MeetingUpdateConverter() }
        };

        MeetingUpdate meetingUpdate = JsonConvert.DeserializeObject<MeetingUpdate>(message, settings);

        // Update the meeting state dictionary
        if (meetingUpdate.MeetingState != null)
        {
            meetingState["isMuted"] = meetingUpdate.MeetingState.IsMuted;
            meetingState["isCameraOn"] = meetingUpdate.MeetingState.IsCameraOn;
            meetingState["isHandRaised"] = meetingUpdate.MeetingState.IsHandRaised;
            meetingState["isInMeeting"] = meetingUpdate.MeetingState.IsInMeeting;
            meetingState["isRecordingOn"] = meetingUpdate.MeetingState.IsRecordingOn;
            meetingState["isBackgroundBlurred"] = meetingUpdate.MeetingState.IsBackgroundBlurred;
            if (meetingUpdate.MeetingState.IsCameraOn)
            {
                state.Camera = "On";
            }
            else
            {
                state.Camera = "Off";
            }
            if (meetingUpdate.MeetingState.IsInMeeting)
            {
                state.Activity = "In a meeting";
            }
            else
            {
                state.Activity = "Not in a Call";
            }
            if (meetingUpdate.MeetingState.IsMuted)
            {
                state.Microphone = "On";
            }
            else
            {
                state.Microphone = "Off";
            }
            if (meetingUpdate.MeetingState.IsHandRaised)
                {
                state.Handup = "Raised";
            }
            else
                {
                state.Handup = "Lowered";
            }
            if (meetingUpdate.MeetingState.IsRecordingOn)
                {    
                state.Recording = "On";
            }
            else
            { 
                state.Recording = "Off";
            }
            if (meetingUpdate.MeetingState.IsBackgroundBlurred)
              {   
                state.Blurred = "Blurred";
            }
            else
              {   
                state.Blurred = "Not Blurred";
            }

            // need to edit state class to add handraised, recording, and backgroundblur
        }

            
        Log.Debug(meetingUpdate.ToString());
        Log.Debug(meetingState.ToString());
    }


        public class MeetingUpdateConverter : JsonConverter<MeetingUpdate>
        {
            #region Public Methods

            public override MeetingUpdate ReadJson(JsonReader reader, Type objectType, MeetingUpdate existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                JObject jsonObject = JObject.Load(reader);

                var meetingState = jsonObject["meetingUpdate"]["meetingState"].ToObject<MeetingState>();
                var meetingPermissions = jsonObject["meetingUpdate"]["meetingPermissions"].ToObject<MeetingPermissions>();

                return new MeetingUpdate
                {
                    MeetingState = meetingState,
                    MeetingPermissions = meetingPermissions
                };
            }

            public override void WriteJson(JsonWriter writer, MeetingUpdate value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            #endregion Public Methods
        }

        #endregion Private Methods
    }
}