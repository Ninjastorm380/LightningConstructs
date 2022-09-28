namespace Lightning
{
    public partial class Socket
    {
        public System.Int32 Available => NetSocket.Available;
        public System.Boolean Listening => IsListening;
        public System.Boolean Connected
        {
            get
            {
                if (IsConnected == false) { return false; }

                lock (ReadLock)
                {
                    lock (WriteLock)
                    {
                        var DataAvailable = NetSocket.Poll(0, System.Net.Sockets.SelectMode.SelectRead);
                        var DataNotAvailable = (NetSocket.Available == 0);
                
                        //A - DataAvailable, B - DataNotAvailable
                        //
                        //A = true,  B = false - connected, data is available for reading. not inverts to true
                        //A = false, B = true  - connected, data is not available for reading. not inverts to true
                        //A = false, B = false - connected, although this value should be impossible to reach. not inverts to true
                        //A = true,  B = true  - disconnected, data is not available for reading. not inverts to false
                
                        IsConnected = !(DataAvailable == true && DataNotAvailable == true);
                        return IsConnected;
                    }
                }
            }
        }
    }
}