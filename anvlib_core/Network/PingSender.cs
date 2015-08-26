using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace anvlib.Network
{
    public static class PingSender
    {
        /// <summary>
        /// Ping network address
        /// </summary>
        /// <param name="address">IP Address or Hostname</param>
        /// <returns></returns>
        public static bool SendPing(string address, int tries)
        {
            int tries_count = 0;
            Ping pingSender = new Ping ();
            PingOptions options = new PingOptions ();

            // Use the default Ttl value which is 128, 
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted. 
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes (data);
            int timeout = 120;
            while (tries_count < tries)
            {
                PingReply reply = pingSender.Send(address, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)                                   
                    return true;

                tries_count++;
            }

            return false;
        }

        public static bool SendPing(string address)
        {
            return SendPing(address, 4);
        }
    }
}
