using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace anvlib.Utilites
{    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct FILE_INFO_3
    {
        public int fi3_id;
        public int fi3_permission;
        public int fi3_num_locks;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string fi3_pathname;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string fi3_username;
    }

    /// <summary>
    /// Вспомогательный класс, считывающий все открытые файл на локальной или сетевой машине
    /// через WinAPI функции
    /// </summary>
    public static class NetAPI32
    {
        [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int NetFileEnum(
         string servername,
         string basepath,
         string username,
         int level,
         ref IntPtr bufptr,
         int prefmaxlen,
         out int entriesread,
         out int totalentries,
         IntPtr resume_handle);

        [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int NetApiBufferFree(IntPtr Buffer);

        [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int NetFileGetInfo(string serv_name, int fieldid, int lvl, ref IntPtr buffer);

        public static void UsingOfNetFileEnum()
        {
            int dwReadEntries;
            int dwTotalEntries;
            IntPtr pBuffer = IntPtr.Zero;
            FILE_INFO_3 pCurrent = new FILE_INFO_3();
            int dwStatus = NetAPI32.NetFileEnum("mag-sql-srv", "e:\\svodki", null, 3, ref pBuffer, -1, out dwReadEntries, out dwTotalEntries, IntPtr.Zero);
            if (dwStatus == 0)
            {
                for (int dwIndex = 0; dwIndex < dwReadEntries; dwIndex++)
                {

                    IntPtr iPtr = new IntPtr(pBuffer.ToInt32() + (dwIndex * Marshal.SizeOf(pCurrent)));
                    pCurrent = (FILE_INFO_3)Marshal.PtrToStructure(iPtr, typeof(FILE_INFO_3));

                    Console.WriteLine("dwIndex={0}", dwIndex);
                    Console.WriteLine("id={0}", pCurrent.fi3_id);
                    Console.WriteLine("num_locks={0}", pCurrent.fi3_num_locks);
                    Console.WriteLine("pathname={0}", pCurrent.fi3_pathname);
                    Console.WriteLine("permission={0}", pCurrent.fi3_permission);
                    Console.WriteLine("username={0}", pCurrent.fi3_username);
                    UsingOfNetFileGetInfo(pCurrent.fi3_id);
                }
            }
            NetAPI32.NetApiBufferFree(pBuffer);
        }


        public static string UsingOfNetFileGetInfo(int fieldid)
        {
            string def_user = "[UNKNOWN USER]";

            if (fieldid == -1)
                return def_user;
                    
            IntPtr pBuffer = IntPtr.Zero;
            FILE_INFO_3 pCurrent = new FILE_INFO_3();
            int dwStatus = NetAPI32.NetFileGetInfo("mag-sql-srv", fieldid,3, ref pBuffer);
            if (dwStatus == 0)
            {                
                    IntPtr iPtr = new IntPtr(pBuffer.ToInt32());
                    pCurrent = (FILE_INFO_3)Marshal.PtrToStructure(iPtr, typeof(FILE_INFO_3));
                    
                    Console.WriteLine("pathname={0}", pCurrent.fi3_pathname);
                    Console.WriteLine("permission={0}", pCurrent.fi3_permission);
                    Console.WriteLine("username={0}", pCurrent.fi3_username);                
            }
            NetAPI32.NetApiBufferFree(pBuffer);

            return def_user;
        }
    }    
}
