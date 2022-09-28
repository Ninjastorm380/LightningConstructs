namespace Lightning
{
    public partial class Socket
    {
        private System.Net.Sockets.Socket NetSocket = new System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
        private System.Boolean IsListening = false;
        private System.Boolean IsConnected = false;
        private System.Boolean IsBaseDisposed = false;
        private Lightning.Governor ListenerGovernor = new Lightning.Governor(20);

        private System.Object ReadLock = new System.Object();
        private System.Object WriteLock = new System.Object();
    }
}