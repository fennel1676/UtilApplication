using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace AKI.Util
{
    public class ConvertUtil
    {
        //public string GetUriEncorde(string HttpUrl)
        //{
        //    //URL 인코딩 유니코드 처리하기 
        //    char[] ch = HttpUrl.ToCharArray();
        //    StringBuilder sb = new StringBuilder();
        //    Encoding ksc = Encoding.GetEncoding("ks_c_5601-1987");

        //    for (int idx = 0; idx < ch.Length; idx++)
        //    {
        //        int temp = Convert.ToInt32(ch[idx]);
        //        string unistr;

        //        if (temp < 0 || temp >= 128)
        //        {
        //            byte[] bysrc = ksc.GetBytes(ch[idx].ToString());
        //            unistr = HttpUtility.UrlEncode(bysrc);
        //            sb.Append(unistr);
        //        }
        //        else
        //        {
        //            sb.Append(ch[idx]);
        //        }
        //    }
        //    return sb.ToString();
        //}

        public static string ByteArray2String(byte[] data, int nLength, int nCutLineNum)
        {
            StringBuilder sb = new StringBuilder();

            int nLine = nLength / nCutLineNum;
            int nReminder = nLength % nCutLineNum;

            for (int i = 0; i < nLine; i++)
            {
                sb.AppendLine(System.BitConverter.ToString(data, i, nCutLineNum));
            }

            if (0 < nReminder)
            {
                sb.AppendLine(System.BitConverter.ToString(data, nLine, nReminder));
            }

            return sb.ToString();
        }

        public static IPAddress String2IPAddress(string source)
        {
            IPAddress ipAddress;
            bool result = IPAddress.TryParse(source, out ipAddress);
            return result ? ipAddress : null;
        }

        public static byte[] Object2Byte2(object obj)
        {
            int size = Marshal.SizeOf(obj);
            byte[] aData = new byte[size];
            IntPtr ptr = Marshal.AllocCoTaskMem(size);

            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, aData, 0, size);
            Marshal.FreeHGlobal(ptr);

            return aData;
        }

        public static byte[] Object2Byte(object obj)
        {
            byte[] ATS_BUFFER = new byte[Marshal.SizeOf(obj)];

            unsafe
            {
                fixed (byte* fixed_buffer = ATS_BUFFER)
                {
                    Marshal.StructureToPtr(obj, (IntPtr)fixed_buffer, false);
                }
            }

            return ATS_BUFFER;
        }

        public static byte[] AddStruct2Byte(object obj, List<object> listObj)
        {
            int nSize = Marshal.SizeOf(obj);
            foreach (object data in listObj)
            {
                nSize += Marshal.SizeOf(data);
            }

            byte[] aData = new byte[nSize];
            IntPtr ptr = Marshal.AllocCoTaskMem(nSize);

            int nCount = 0;
            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, aData, nCount, Marshal.SizeOf(obj));
            nCount += Marshal.SizeOf(obj);

            foreach (object data in listObj)
            {
                Marshal.StructureToPtr(data, ptr, false);
                Marshal.Copy(ptr, aData, nCount, Marshal.SizeOf(data));
                nCount += Marshal.SizeOf(data);
            }

            Marshal.FreeHGlobal(ptr);

            return aData;
        }

        public static T Byte2Struct<T>(byte[] aData)    //where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));

            if (size > aData.Length)
                throw new Exception();

            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(size);
                size = size > aData.Length ? aData.Length : size;
                Marshal.Copy(aData, 0, ptr, size);
                T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
                Marshal.FreeHGlobal(ptr);
                return obj;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }

        public static T_Object Array2Struct<T_Array, T_Object>(T_Array[] aData)    //where T : struct
        {
            int nObjectSize = Marshal.SizeOf(typeof(T_Object));
            int nArrayDataTypeSize = Marshal.SizeOf(typeof(T_Array));
            int nTotalBufferSize = aData.Length * nArrayDataTypeSize;
            if (nObjectSize > nTotalBufferSize)
                throw new Exception();
            try
            {
                GCHandle handle = GCHandle.Alloc(aData, GCHandleType.Pinned);
                IntPtr ptr = handle.AddrOfPinnedObject();
                T_Object obj = (T_Object)Marshal.PtrToStructure(ptr, typeof(T_Object));
                handle.Free();
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T_Object);
            }
        }

        public static T IntPtr2Struct<T>(IntPtr pData, bool isMemoryFree = true)    //where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));

            if (IntPtr.Zero == pData)
                throw new Exception();

            try
            {
                T obj = (T)Marshal.PtrToStructure(pData, typeof(T));
                if (isMemoryFree)
                    Marshal.FreeHGlobal(pData);
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }

        public static bool ParseByte2Struct<T1, T2>(byte[] aData, out T1 obj, out List<T2> listObj) //where T1 : struct
                                                                                                    //where T2 : struct
        {
            obj = default(T1);
            listObj = new List<T2>();

            int nSizeObj = Marshal.SizeOf(typeof(T1));
            int nSizeListObj = Marshal.SizeOf(typeof(T2));

            try
            {
                IntPtr ptrObj = Marshal.AllocHGlobal(nSizeObj);
                IntPtr ptrListObj = Marshal.AllocHGlobal(nSizeListObj);

                Marshal.Copy(aData, 0, ptrObj, nSizeObj);
                obj = (T1)Marshal.PtrToStructure(ptrObj, typeof(T1));
                Marshal.FreeHGlobal(ptrObj);

                int nCount = aData.Count();
                for (int i = nSizeObj; i < nCount; i += nSizeListObj)
                {
                    Marshal.Copy(aData, i, ptrListObj, nSizeListObj);
                    T2 objListResult = (T2)Marshal.PtrToStructure(ptrListObj, typeof(T2));
                    listObj.Add(objListResult);
                    Marshal.FreeHGlobal(ptrListObj);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        public static byte[] HexString2ByteArray(string strHex)
        {
            return Enumerable.Range(0, strHex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(strHex.Substring(x, 2), 16))
                .ToArray();
        }

        public static string ByteArray2String(byte[] aData)
        {
            StringBuilder sbHex = new StringBuilder(aData.Length * 2);
            for (int i = 0; i < aData.Length; i++)
                sbHex.Append(aData[i].ToString("X2"));
            return sbHex.ToString();
        }

        public static bool HexString2Number(string strHex, out UInt64 nResult)
        {
            StringBuilder sb = new StringBuilder();
            nResult = 0;
            foreach (char c in strHex)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'))
                    sb.Append(c);
                else
                    break;
            }

            if (UInt64.TryParse(sb.ToString(), System.Globalization.NumberStyles.HexNumber, null, out nResult))
                return true;
            else
                return false;
        }

        public static string Hex2Ascii(string strHexString)
        {

            StringBuilder str = new StringBuilder();
            int nParse = 0;
            char cConvert = ' ';
            string strConvert = string.Empty;
            for (int i = 0; i <= strHexString.Length - 2; i += 2)
            {
                nParse = Int32.Parse(strHexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                cConvert = Convert.ToChar(nParse);
                strConvert = Convert.ToString(cConvert);
                str.Append(strConvert);
            }

            return str.ToString();
        }

        public static byte[] String2ByteArray(string str, eEncoding encoding = eEncoding.Default)
        {
            switch(encoding)
            {
                case eEncoding.ASCII:               return Encoding.ASCII.GetBytes(str);
                case eEncoding.Default:             return Encoding.Default.GetBytes(str);
                case eEncoding.BigEndianUnicode:    return Encoding.BigEndianUnicode.GetBytes(str);
                case eEncoding.Unicode:             return Encoding.Unicode.GetBytes(str);
                case eEncoding.UTF32:               return Encoding.UTF32.GetBytes(str);
                case eEncoding.UTF7:                return Encoding.UTF7.GetBytes(str);
                case eEncoding.UTF8:                return Encoding.UTF8.GetBytes(str);
                default:                            return Encoding.Default.GetBytes(str);
            }
        }

        //public static string ByteArray2HexString(byte[] aData)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (byte data in aData)
        //    {
        //        sb.Append("")
        //    }
        //}
    }
}
