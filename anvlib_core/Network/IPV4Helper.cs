using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace anvlib.Network
{
    public static class IPV4Helper
    {
        /// <summary>
        /// Метод определяющий IP адресс по имени компьютера или адресу в формате IpV6
        /// </summary>
        /// <param name="IpV6AddrOrHostname">Имя компьютера или адрес в формате IpV6</param>
        /// <param name="networkpattern">Параметр позволяющий вернуть верный IP адрес, т.к. их может быть несколько
        /// Формат параметра "x.x.x.0"</param>
        /// <returns></returns>
        public static string GetIP4Address(string IpV6AddrOrHostname,string networkpattern)
        {
            string IP4Address = String.Empty;            

            foreach (IPAddress IPA in Dns.GetHostAddresses(IpV6AddrOrHostname))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {                    
                    IP4Address = IPA.ToString();
                    if (!string.IsNullOrEmpty(networkpattern))
                    {
                        string[] digits = networkpattern.Split('.');
                        if (digits.Length < 2)
                            throw new Exception("Wrong network pattern");
                        else
                        {
                            string[] digits2 = IP4Address.Split('.');
                            if (digits[0] == digits2[0] && digits[1] == digits2[1] && digits[2] == digits2[2])
                                break;
                        }
                    }
                    else
                        break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    if (!string.IsNullOrEmpty(networkpattern))
                    {
                        string[] digits = networkpattern.Split('.');
                        if (digits.Length < 2)
                            throw new Exception("Wrong network pattern");
                        else
                        {                            
                            string[] digits2 = IP4Address.Split('.');
                            if (digits[0] == digits2[0] && digits[1] == digits2[1] && digits[2] == digits2[2])
                                break;
                        }
                    }
                    else
                        break;
                }
            }

            return IP4Address;
        }

        /// <summary>
        /// Метод определяющий IP адресс по имени компьютера или адресу в формате IpV6
        /// </summary>
        /// <param name="IpV6AddrOrHostname">Имя компьютера или адрес в формате IpV6</param>
        /// <returns></returns>
        public static string GetIP4Address(string IpV6AddrOrHostname)
        {
            return GetIP4Address(IpV6AddrOrHostname, null);
        }
    }
}
