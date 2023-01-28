using AKI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
//using AKI.Win32.API.IpHlpApi;

namespace AKI.Communication.Util
{
    /// <summary>
    /// MIB_IPNETROW structure returned by GetIpNetTable
    /// DO NOT MODIFY THIS STRUCTURE.
    /// </summary>

    public class NetworkInfoUtil
    {
        private const int ERROR_INSUFFICIENT_BUFFER = 122;

        public static Dictionary<IPAddress, PhysicalAddress> GetAllDevicesOnLAN()
        {            
            Dictionary<IPAddress, PhysicalAddress> all = new Dictionary<IPAddress, PhysicalAddress>();
            // Add this PC to the list...
            all.Add(GetIPAddress(), GetMacAddress());
            int spaceForNetTable = 0;
            // Get the space needed
            // We do that by requesting the table, but not giving any space at all.
            // The return value will tell us how much we actually need.
            AKI.Win32.API.IpHlpApi.GetIpNetTable(IntPtr.Zero, ref spaceForNetTable, false);
            // Allocate the space
            // We use a try-finally block to ensure release.
            IntPtr rawTable = IntPtr.Zero;
            try
            {
                rawTable = Marshal.AllocCoTaskMem(spaceForNetTable);
                // Get the actual data
                int errorCode = AKI.Win32.API.IpHlpApi.GetIpNetTable(rawTable, ref spaceForNetTable, false);
                if (errorCode != 0)
                {
                    // Failed for some reason - can do no more here.
                    throw new Exception(string.Format("Unable to retrieve network table. Error code {0}", errorCode));
                }
                // Get the rows count
                int rowsCount = Marshal.ReadInt32(rawTable);
                IntPtr currentBuffer = new IntPtr(rawTable.ToInt64() + Marshal.SizeOf(typeof(Int32)));
                // Convert the raw table to individual entries
                AKI.Win32.API.IpHlpApi.MIB_IPNETROW[] rows = new AKI.Win32.API.IpHlpApi.MIB_IPNETROW[rowsCount];
                for (int index = 0; index < rowsCount; index++)
                {
                    rows[index] = (AKI.Win32.API.IpHlpApi.MIB_IPNETROW)Marshal.PtrToStructure(new IntPtr(currentBuffer.ToInt64() +
                                                (index * Marshal.SizeOf(typeof(AKI.Win32.API.IpHlpApi.MIB_IPNETROW)))
                                               ),
                                                typeof(AKI.Win32.API.IpHlpApi.MIB_IPNETROW));
                }
                // Define the dummy entries list (we can discard these)
                PhysicalAddress virtualMAC = new PhysicalAddress(new byte[] { 0, 0, 0, 0, 0, 0 });
                PhysicalAddress broadcastMAC = new PhysicalAddress(new byte[] { 255, 255, 255, 255, 255, 255 });
                foreach (AKI.Win32.API.IpHlpApi.MIB_IPNETROW row in rows)
                {
                    IPAddress ip = new IPAddress(BitConverter.GetBytes(row.dwAddr));
                    byte[] rawMAC = new byte[] { row.mac0, row.mac1, row.mac2, row.mac3, row.mac4, row.mac5 };
                    PhysicalAddress pa = new PhysicalAddress(rawMAC);
                    if (!pa.Equals(virtualMAC) && !pa.Equals(broadcastMAC) && !IsMulticast(ip))
                    {
                        //Console.WriteLine("IP: {0}\t\tMAC: {1}", ip.ToString(), pa.ToString());
                        if (!all.ContainsKey(ip))
                        {
                            all.Add(ip, pa);
                        }
                    }
                }
            }
            finally
            {
                // Release the memory.
                Marshal.FreeCoTaskMem(rawTable);
            }
            return all;
        }

        public static PhysicalAddress GetTargetDevicesOnLAN(string strIpAddress)
        {
            return GetTargetDevicesOnLAN(ConvertUtil.String2IPAddress(strIpAddress));
        }

        public static PhysicalAddress GetTargetDevicesOnLAN(IPAddress ipAddress)
        {
            int spaceForNetTable = 0;
            AKI.Win32.API.IpHlpApi.GetIpNetTable(IntPtr.Zero, ref spaceForNetTable, false);
            // Allocate the space
            // We use a try-finally block to ensure release.
            IntPtr rawTable = IntPtr.Zero;
            try
            {
                rawTable = Marshal.AllocCoTaskMem(spaceForNetTable);
                // Get the actual data
                int errorCode = AKI.Win32.API.IpHlpApi.GetIpNetTable(rawTable, ref spaceForNetTable, false);
                if (errorCode != 0)
                {
                    // Failed for some reason - can do no more here.
                    throw new Exception(string.Format("Unable to retrieve network table. Error code {0}", errorCode));
                }
                // Get the rows count
                int rowsCount = Marshal.ReadInt32(rawTable);
                IntPtr currentBuffer = new IntPtr(rawTable.ToInt64() + Marshal.SizeOf(typeof(Int32)));
                // Convert the raw table to individual entries
                AKI.Win32.API.IpHlpApi.MIB_IPNETROW[] rows = new AKI.Win32.API.IpHlpApi.MIB_IPNETROW[rowsCount];
                for (int index = 0; index < rowsCount; index++)
                {
                    rows[index] = (AKI.Win32.API.IpHlpApi.MIB_IPNETROW)Marshal.PtrToStructure(new IntPtr(currentBuffer.ToInt64() + (index * Marshal.SizeOf(typeof(AKI.Win32.API.IpHlpApi.MIB_IPNETROW)))), typeof(AKI.Win32.API.IpHlpApi.MIB_IPNETROW));
                }
                // Define the dummy entries list (we can discard these)
                PhysicalAddress virtualMAC = new PhysicalAddress(new byte[] { 0, 0, 0, 0, 0, 0 });
                PhysicalAddress broadcastMAC = new PhysicalAddress(new byte[] { 255, 255, 255, 255, 255, 255 });
                foreach (AKI.Win32.API.IpHlpApi.MIB_IPNETROW row in rows)
                {
                    IPAddress ip = new IPAddress(BitConverter.GetBytes(row.dwAddr));
                    if (ip.Address == ipAddress.Address)
                    {
                        byte[] rawMAC = new byte[] { row.mac0, row.mac1, row.mac2, row.mac3, row.mac4, row.mac5 };
                        PhysicalAddress pa = new PhysicalAddress(rawMAC);                        
                        if (!pa.Equals(virtualMAC) && !pa.Equals(broadcastMAC) && !IsMulticast(ip))
                            return pa;
                    }
                }
            }
            finally
            {
                // Release the memory.
                Marshal.FreeCoTaskMem(rawTable);
            }
            return null;
        }
        
        public static IPAddress GetIPAddress()
        {
            String strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            foreach (IPAddress ip in addr)
            {
                if (!ip.IsIPv6LinkLocal)
                    return (ip);
            }
            return addr.Length > 0 ? addr[0] : null;
        }

        public static PhysicalAddress GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }

        public static PhysicalAddress GetMacAddress(IPAddress ipAddress)
        {
            byte[] rawMAC = new byte[6];
            int nLength = rawMAC.Length;

            int r = AKI.Win32.API.IpHlpApi.SendARP((int)ipAddress.Address, 0, rawMAC, ref nLength);
            string mac = BitConverter.ToString(rawMAC, 0, 6);
            PhysicalAddress pa = new PhysicalAddress(rawMAC);

            return pa;
        }

        public static PhysicalAddress GetMacAddress(string strIpAddress)
        {
            byte[] rawMAC = new byte[6];
            int nLength = rawMAC.Length;

            IPAddress ipAddress = AKI.Util.ConvertUtil.String2IPAddress(strIpAddress);
            if (null == ipAddress)
                return null;

            int r = AKI.Win32.API.IpHlpApi.SendARP((int)ipAddress.Address, 0, rawMAC, ref nLength);
            string mac = BitConverter.ToString(rawMAC, 0, 6);
            PhysicalAddress pa = new PhysicalAddress(rawMAC);

            return pa;
        }

        private static bool IsMulticast(IPAddress ip)
        {
            bool result = true;
            if (!ip.IsIPv6Multicast)
            {
                byte highIP = ip.GetAddressBytes()[0];
                if (highIP < 224 || highIP > 239)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
