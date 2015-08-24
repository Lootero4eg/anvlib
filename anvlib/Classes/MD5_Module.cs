using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace anvlib.Classes
{
    /// <summary>
    /// Вспомогательный класс по создание хэшей в формате MD5
    /// </summary>
    public static class MD5_Module
    {
        public static string CreateHash(string Password)
        {
            UTF8Encoding enc = new UTF8Encoding();
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(enc.GetBytes(Password));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sBuilder.Append(hash[i].ToString("x2"));
            }

            return sBuilder.ToString();            
        }
    }
}
