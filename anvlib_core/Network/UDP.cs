using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace anvlib.Network
{
    //--на одном компе лучше не запускать сенд и рецив, надо принудительно как то убивать соккет!!!
    //--это тестовый класс, только для примера работы Броадкаста
    public class UDP
    {
        private int _port = 8585;
        private UdpClient udp = new UdpClient(8585);
        public event EventHandler NewMsg;

        /*public UDP(int Port)
        {
            _port = Port;
            udp = new UdpClient(_port);
        }*/

        public void SendBroadcastMessage(string message)
        {            
            UdpClient ul = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, _port);
            byte[] msg = ASCIIEncoding.ASCII.GetBytes(message);
            ul.Send(msg, msg.Length, ip);            
            //StartListening();
        }

        public void StartListening()
        {
            if (udp != null)
                udp.BeginReceive(ReceiveFromClient, new object());
        }

        public void ReceiveFromClient(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, _port);
            byte[] bytes = udp.EndReceive(ar, ref ip);
            string message = Encoding.UTF8.GetString(bytes);
            if (NewMsg != null)
                NewMsg(message, new EventArgs());
            StartListening();
        }
    }
}
