﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace anvlib.Utilites
{
    public class IconExtractor
    {
        /* CONSTRUCTORS */
        static IconExtractor()
        {
        }

        // HIDE INSTANCE CONSTRUCTOR
        private IconExtractor()
        {
        }

        [DllImport("Shell32", CharSet = CharSet.Auto)]
        private static unsafe extern int ExtractIconEx(
          string lpszFile,
          int nIconIndex,
          IntPtr[] phIconLarge,
          IntPtr[] phIconSmall,
          int nIcons);

        [DllImport("user32.dll", EntryPoint = "DestroyIcon", SetLastError = true)]
        private static unsafe extern int DestroyIcon(IntPtr hIcon);

        public static Icon ExtractIconFromExe(string file, bool large)
        {
            unsafe
            {
                int readIconCount = 0;
                IntPtr[] hDummy = new IntPtr[1] { IntPtr.Zero };
                IntPtr[] hIconEx = new IntPtr[1] { IntPtr.Zero };

                try
                {
                    if (large)
                        readIconCount = ExtractIconEx(file, 0, hIconEx, hDummy, 1);
                    else
                        readIconCount = ExtractIconEx(file, 0, hDummy, hIconEx, 1);

                    if (readIconCount > 0 && hIconEx[0] != IntPtr.Zero)
                    {
                        // GET FIRST EXTRACTED ICON
                        Icon extractedIcon = (Icon)Icon.FromHandle(hIconEx[0]).Clone();

                        return extractedIcon;
                    }
                    else // NO ICONS READ
                        return null;
                }
                catch (Exception ex)
                {
                    /* EXTRACT ICON ERROR */

                    // BUBBLE UP
                    throw new ApplicationException("Could not extract icon", ex);
                }
                finally
                {
                    // RELEASE RESOURCES
                    foreach (IntPtr ptr in hIconEx)
                        if (ptr != IntPtr.Zero)
                            DestroyIcon(ptr);

                    foreach (IntPtr ptr in hDummy)
                        if (ptr != IntPtr.Zero)
                            DestroyIcon(ptr);
                }
            }
        }
    }
}
