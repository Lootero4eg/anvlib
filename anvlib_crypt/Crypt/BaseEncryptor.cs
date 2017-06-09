using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security;
using System.Security.Cryptography;

namespace anvlib.Crypt
{
    /// <summary>
    /// Вспомогательный класс позволяющий шифровать/дешифровать данные заданным алгоритмом
    /// Этот класс заменяет алгоритмы DES, Aes, RC2, Triple DES, Rindjael
    /// Параметр это не базовый класс DES,  а алгоритм DESCryptoServiceProvider, помни об этом!
    /// </summary>
    public static class BaseEncryptor<T> where T : new()
    {
        //--Ключ шифрование, одинаковы, но можно сделать и разные
        const string skey = "FC12AB78";        

        /// <summary>
        /// Основной метод шифрование с 2-мя ключами
        /// </summary>
        /// <param name="text">пароль в открытом виде</param>
        /// <param name="KEY">Первый ключ</param>
        /// <param name="IV">Второй ключ</param>
        /// <returns></returns>
        public static byte[] Encrypt(string text, string KEY, string IV)
        {
            byte[] key = UTF8Encoding.UTF8.GetBytes(KEY); 
            byte[] key2 = UTF8Encoding.UTF8.GetBytes(IV);
            T Algorithm = new T(); 
            (Algorithm as SymmetricAlgorithm).Key = key;
            (Algorithm as SymmetricAlgorithm).IV = key2;
            ICryptoTransform AlgorithmCrypto = (Algorithm as SymmetricAlgorithm).CreateEncryptor();
            byte[] bytesForEncryption = Encoding.UTF8.GetBytes(text);
            byte[] pass = AlgorithmCrypto.TransformFinalBlock(bytesForEncryption, 0, bytesForEncryption.Length);

            return pass;
        }

        /// <summary>
        /// Основной метод дешифрование с 2-мя ключами
        /// </summary>
        /// <param name="text">пароль в открытом виде</param>
        /// <param name="KEY">Первый ключ</param>
        /// <param name="IV">Второй ключ</param>
        /// <returns></returns>
        public static string Decrypt(byte[] text, string KEY, string IV)
        {
            if (text == null)
                return string.Empty;

            byte[] key = UTF8Encoding.UTF8.GetBytes(KEY);
            byte[] key2 = UTF8Encoding.UTF8.GetBytes(IV);
            T Algorithm = new T();
            (Algorithm as SymmetricAlgorithm).Key = key;
            (Algorithm as SymmetricAlgorithm).IV = key2;
            ICryptoTransform AlgorithmDecr = (Algorithm as SymmetricAlgorithm).CreateDecryptor();

            byte[] recoveredpass = AlgorithmDecr.TransformFinalBlock(text, 0, text.Length);

            return Encoding.UTF8.GetString(recoveredpass);
        }

        /// <summary>
        /// Самый простой шифрователь с одним ключем для обоих ключей
        /// </summary>
        /// <param name="text">Пароль в открытом виде</param>
        /// <returns></returns>
        public static byte[] Encrypt(string text)
        {
            return Encrypt(text, skey);
        }

        /// <summary>
        /// Самый простой дешифратор с одним ключем для обоих ключей
        /// </summary>
        /// <param name="text">Пароль в открытом виде</param>
        /// <returns></returns>
        public static string Decrypt(byte[] text)
        {
            return Decrypt(text, skey);
        }

        /// <summary>
        /// Метод шифрование с заданным ключем для обоих ключей
        /// </summary>
        /// <param name="text">Пароль в открытом виде</param>
        /// <param name="KEY">8-ми значный ключ в виде "XXXXXXXX"</param>
        /// <returns></returns>
        public static byte[] Encrypt(string text, string KEY)
        {            
            return Encrypt(text,KEY,KEY);
        }

        /// <summary>
        /// Метод дешифрование с заданным ключем для обоих ключей
        /// </summary>
        /// <param name="text">Пароль в открытом виде</param>
        /// <param name="KEY">8-ми значный ключ в виде "XXXXXXXX"</param>
        /// <returns></returns>
        public static string Decrypt(byte[] text, string KEY)
        {
            return Decrypt(text, KEY, KEY);
        }        
    }
}
