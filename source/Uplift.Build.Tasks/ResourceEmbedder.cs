using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Uplift.Utility
{
    public class ResourceEmbedder
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr BeginUpdateResource(string pFileName,
           [MarshalAs(UnmanagedType.Bool)]bool bDeleteExistingResources);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool UpdateResource(IntPtr hUpdate, IntPtr lpType, IntPtr lpName, ushort wLanguage,
            IntPtr lpData, uint cbData);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

        //private const string RT_RCDATA = "#10";
        private IntPtr RT_RCDATA = (IntPtr)10;
        private const int EN_DEFAULT_LANG_ID = 1033;

        public void AddDataResource(string targetExePath, byte[] dataToEmbed, int resourceId)
        {
            IntPtr hResource = BeginUpdateResource(targetExePath, false);
            if (hResource.ToInt32() == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            // Get language identifier
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            int pid = ((ushort)currentCulture.LCID) & 0x3ff;
            int sid = ((ushort)currentCulture.LCID) >> 10;
            ushort languageID = (ushort)((((ushort)pid) << 10) | ((ushort)sid));

            GCHandle dataHandle = GCHandle.Alloc(dataToEmbed, GCHandleType.Pinned);

            try
            {
                if (UpdateResource(hResource, RT_RCDATA, (IntPtr)resourceId, EN_DEFAULT_LANG_ID, dataHandle.AddrOfPinnedObject(), (uint)dataToEmbed.Length) == false)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                if (EndUpdateResource(hResource, false) == false)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                dataHandle.Free();
            }
        }

        public void AddDataResource(string targetExePath, string fileToEmbed, int resourceId)
        {
            var fileData = File.ReadAllBytes(fileToEmbed);
            AddDataResource(targetExePath, fileData, resourceId);
        }
    }
}
