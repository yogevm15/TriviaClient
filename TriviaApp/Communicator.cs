using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TriviaApp
{
    public class Communicator
    {
        
        private Socket s;
        private Mutex m;
        public Communicator(string address, int port)
        {
            IPAddress ipAddr = IPAddress.Parse(address);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);
            Socket tempSocket =
                new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            tempSocket.Connect(localEndPoint);

            if (tempSocket.Connected)
            {
                s = tempSocket;
                m = new Mutex();
            }


        }
        public async Task<ResponseInfo> SocketSendReceive(Byte[] bytesSent)
        {
            
            Task<ResponseInfo> t = Task.Run(() =>
            {
                // Send request to the server.
                m.WaitOne();
                s.Send(bytesSent, bytesSent.Length, 0);
                ResponseInfo responseInfo = new ResponseInfo();
                responseInfo.code = getCode();
                responseInfo.buffer = getJsonBuffer();
                m.ReleaseMutex();
                return responseInfo;

            });
            return await t;
            
                        
            
        }
        public void forceRelease()
        {
            try
            {
                m.ReleaseMutex();
            }
            catch
            {

            }
            
        }

        private Byte[] getJsonBuffer()
        {

            int size = getSize();
            Byte[] b = new Byte[size];
            s.Receive(b, b.Length, 0);
            return b;
        }

        private int getSize()
        {

            Byte[] b = new Byte[4];
            s.Receive(b, b.Length, 0);
            Array.Reverse(b);
            return BitConverter.ToInt32(b, 0);
        }
        private int getCode()
        {
            int bytes = 0;
            Byte[] b = new Byte[1];
            bytes = s.Receive(b, b.Length, 0);
            return Encoding.ASCII.GetString(b)[0];
        }
    }
    
}
