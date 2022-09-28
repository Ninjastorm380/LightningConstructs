namespace Lightning
{
    public partial class Socket
    {
        public Socket()
        {
            
            NetSocket.DualMode = true;
            NetSocket.LingerState = new System.Net.Sockets.LingerOption(true, 30);
        }

        public delegate void SocketConnectedHandler(Socket NewSocket);
        public event SocketConnectedHandler? ServerSocketConnected;
        public event SocketConnectedHandler? ClientSocketConnected;

        private void HookupSocket(System.Net.Sockets.Socket BaseSocket)
        {
            NetSocket = BaseSocket;
            NetSocket.LingerState = new System.Net.Sockets.LingerOption(true, 30);
            IsConnected = true;
        }

        public void Connect(System.Net.IPEndPoint Endpoint)
        {
            if (IsBaseDisposed == true)
            {
                NetSocket = new System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            }
            NetSocket.Connect(Endpoint);
            IsConnected = true;
            
            if (ClientSocketConnected == null)
            {
                Disconnect();
            }
            else
            {
                System.Threading.Thread AsyncThread = new System.Threading.Thread(AsyncThreadMethod => { ClientSocketConnected(this); });
                AsyncThread.Start();
            }
        }
        
        public void Disconnect()
        {
            IsConnected = false;
            NetSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            NetSocket.Disconnect(true);
            NetSocket.Dispose();
            IsBaseDisposed = true;
        }
        
        public void Listen(System.Net.IPEndPoint Endpoint)
        {
            NetSocket.Bind(Endpoint);
            NetSocket.Listen(100);
            IsListening = true;
            var AsyncListener = new System.Threading.Thread(AsyncListenerMethod);
            AsyncListener.Start();
        }

        public void Deafen()
        {
            IsListening = false;
            NetSocket.Disconnect(true);
        }

        public System.Int32 Read(System.Byte[] Buffer, System.Int32 Offset, System.Int32 Length, System.Net.Sockets.SocketFlags Flags)
        {
            lock (ReadLock)
            {
                return NetSocket.Receive(Buffer, Offset, Length, Flags);
            }
            
        }

        public System.Int32 Write(System.Byte[] Buffer, System.Int32 Offset, System.Int32 Length, System.Net.Sockets.SocketFlags Flags)
        {
            lock (WriteLock)
            {
                return NetSocket.Send(Buffer, Offset, Length, Flags);
            }
        }

        private void AsyncListenerMethod()
        {
            do
            {
                if (NetSocket.Poll(0, System.Net.Sockets.SelectMode.SelectRead))
                {
                    var NewSocketBase = NetSocket.Accept();
                
                    var NewSocket = new Socket();
                    NewSocket.HookupSocket(NewSocketBase);
                    if (ServerSocketConnected == null)
                    {
                        NewSocket.Disconnect();
                    }
                    else
                    {
                        System.Threading.Thread AsyncThread = new System.Threading.Thread(AsyncThreadMethod => { ServerSocketConnected(NewSocket); });
                        AsyncThread.Start();
                        //SocketConnected(NewSocket);
                    }
                }
                ListenerGovernor.Limit();
            } while (IsListening);

            
        }
    }
}