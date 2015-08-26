using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace anvlib.Network
{
    public class UDP
    {
        private UdpClient udp = new UdpClient(8585);

        public void test()
        {
            //TcpListener tl = new TcpListener(1);
            UdpClient ul = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 8585);
            byte[] msg = ASCIIEncoding.ASCII.GetBytes("tvoyumat!");
            ul.Send(msg, msg.Length, ip);
            StartListening();
        }

        public void StartListening()
        {
            udp.BeginReceive(test2, new object());
        }

        public void test2(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 8585);
            byte[] bytes = udp.EndReceive(ar, ref ip);
            string message = Encoding.ASCII.GetString(bytes);
            byte[] msg = ASCIIEncoding.ASCII.GetBytes("idi na hui!");
            IPEndPoint ip1 = new IPEndPoint(ip.Address, 8585);
            udp.Send(msg, msg.Length, ip1);
            StartListening();
        }
    }
}
