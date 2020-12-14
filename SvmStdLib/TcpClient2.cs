using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SvmStdLib
{
    public class TcpClient2
    {
        public enum SocketCommand
        {
            Unknown,
            Connect,
            Disconnect,
            Close
        }
        public enum SocketState
        {
            Undefined,
            CreatingSocket,
            Connecting,
            ConnectError,
            Connected,
            Disconnecting,
            Disconnected,
            Closing,
            Closed
        }
        public string Host { get; set; }
        public int PortNo { get; set; }
        public SocketCommand Command
        {
            set
            {
                if (State < SocketState.Closed)
                {
                    switch (value)
                    {
                        case SocketCommand.Connect:
                            if (socket==null)
                            {
                                State = SocketState.CreatingSocket;
                            }
                            else
                            {
                                State = SocketState.Connecting;
                            }
                            break;
                        case SocketCommand.Disconnect:
                            if (State > SocketState.CreatingSocket)
                            {
                                State = SocketState.Disconnecting;
                            }
                            break;
                        case SocketCommand.Close:
                            if (State > SocketState.CreatingSocket)
                            {
                                State = SocketState.Closing;
                            }
                            break;
                    }
                }
            }
        }
        public SocketState State { get; private set; }
        public event EventHandler<SocketState> StateChanged;
        private Socket socket;
        private UInt32 t1;
        private SocketState oldState;
        public void Execute()
        {
            switch (State)
            {
                case SocketState.Undefined:
                    break;
                case SocketState.CreatingSocket:
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Blocking = false;
                    State = SocketState.Connecting;
                    break;
                case SocketState.Connecting:
                    try
                    {
                        socket.Connect(Host, PortNo);
                    }
                    catch (SocketException ex)
                    {
                        if (!(ex.ErrorCode == 10035))//WSAEWOULDBLOCK
                        {
                            throw;
                        }
                    }
                    if (socket.Poll(1, SelectMode.SelectWrite))
                    {
                        State = SocketState.Connected;
                    }
                    else
                    {
                        t1 = Time.Ticks;
                        State = SocketState.ConnectError;
                    }
                    break;
                case SocketState.ConnectError:
                    if (Time.Ticks - t1 >= 5000)
                    {
                        State = SocketState.Connecting;
                    }
                    break;
                case SocketState.Connected:
                    break;
                case SocketState.Disconnecting:
                    if (socket.Connected)
                    {
                        socket.Disconnect(true);
                    }
                    State = SocketState.Disconnected;
                    break;
                case SocketState.Disconnected:
                    break;
                case SocketState.Closing:
                    socket.Close();
                    break;
                case SocketState.Closed:
                    State = SocketState.Closed;
                    break;
                default:
                    break;
            }
            if (State != oldState)
            {
                oldState = State;
                StateChanged?.Invoke(this, State);
            }
        }
        public void Test()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Blocking = false;
            Console.WriteLine("Ready for connect");
            Console.ReadLine();
            Console.WriteLine("Connecting");
            try
            {
                socket.Connect("127.0.0.1",2000);
            }
            catch (SocketException ex)
            {
                if (!(ex.ErrorCode == 10035))//WSAEWOULDBLOCK
                {
                    throw;
                }
            }
            if (socket.Poll(1, SelectMode.SelectWrite))
            {
                Console.WriteLine("Connected");
            }
            else
            {
                Console.WriteLine("Not Connected");
            }

            if (socket.Poll(1000, SelectMode.SelectWrite))
            {
                socket.Disconnect(false);
                Console.WriteLine("Disconnecting");
            }

            Console.WriteLine("Closing");
            socket.Close();
        }
    }
}
