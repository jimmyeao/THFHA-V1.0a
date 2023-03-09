using Serilog;
using System.Net.WebSockets;
using System.Text;

namespace WebSocketClientExample
{
    public class WebSocketClient
    {
        #region Private Fields

        private readonly ClientWebSocket _clientWebSocket;

        #endregion Private Fields

        #region Public Constructors

        public WebSocketClient(Uri uri)
        {
            _clientWebSocket = new ClientWebSocket();
            _ = ConnectAsync(uri);
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

        #endregion Private Methods
    }
}