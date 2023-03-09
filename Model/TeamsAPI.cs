using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace WebSocketClientExample
{
    public class WebSocketClient
    {
        public event EventHandler<string> MessageReceived;
        private readonly ClientWebSocket _clientWebSocket;

        public WebSocketClient(Uri uri)
        {
            _clientWebSocket = new ClientWebSocket();
            _ = ConnectAsync(uri);
        }

        private async Task ConnectAsync(Uri uri)
        {
            try
            {
                await _clientWebSocket.ConnectAsync(uri, CancellationToken.None);
                Console.WriteLine("WebSocket connected");
                string message = "{\"apiVersion\":\"1.0.0\",\"service\":\"query-meeting-state\",\"action\":\"query-meeting-state\",\"manufacturer\":\"Elgato\",\"device\":\"StreamDeck\",\"timestamp\":1675341655453}";
                await SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred: " + ex.Message);
                Log.Error("Using URI: "+uri.ToString());
            }

            await ReceiveLoopAsync();
        }

        public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await _clientWebSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, cancellationToken);
            Console.WriteLine($"Message sent: {message}");
        }

        private async Task ReceiveLoopAsync(CancellationToken cancellationToken = default)
        {
            byte[] buffer = new byte[1024];
            while (_clientWebSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                string messageReceived = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Message received: {messageReceived}");

                MessageReceived?.Invoke(this, messageReceived);
            }
        }
    }
}
