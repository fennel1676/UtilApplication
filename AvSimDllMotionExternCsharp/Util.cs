using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AvSimDllMotionExternCsharp
{
    public class Util
    {
        private static IntPtr g_ptr = IntPtr.Zero;
        private static int g_nPtrSize = 0;
        private static byte[] g_aData = null;
        private static int g_nDataSize = 0;

        public static void Free()
        {
            if (IntPtr.Zero != g_ptr)
            {
                Marshal.FreeHGlobal(g_ptr);
                g_nPtrSize = 0;
            }
        }

        public static byte[] Struct2Byte(object obj)
        {
            int size = Marshal.SizeOf(obj);
            byte[] aData = new byte[size];
            IntPtr ptr = Marshal.AllocCoTaskMem(size);

            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, aData, 0, size);
            Marshal.FreeHGlobal(ptr);

            return aData;
        }

        public static byte[] Struct2Byte2(object obj)
        {
            int size = Marshal.SizeOf(obj);

            if (null == g_aData)
            {
                g_aData = new byte[size];
                g_nDataSize = size;
            }
            else
            {
                if (g_nDataSize < size)
                {
                    Array.Resize<byte>(ref g_aData, size);
                    g_nDataSize = size;
                }
            }

            if (IntPtr.Zero == g_ptr)
            {
                g_ptr = Marshal.AllocHGlobal(size);
                g_nPtrSize = size;
            }
            else
            {
                if (g_nPtrSize < size)
                {
                    Marshal.FreeHGlobal(g_ptr);
                    g_nPtrSize = 0;

                    g_ptr = Marshal.AllocHGlobal(size);
                    g_nPtrSize = size;
                }
            }

            Marshal.StructureToPtr(obj, g_ptr, true);
            Marshal.Copy(g_ptr, g_aData, 0, size);
            //Marshal.FreeHGlobal(ptr);

            return g_aData;
        }

        public static T Byte2Struct<T>(byte[] aData)    //where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));

            if (size < aData.Length)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }

        public static T Byte2Struct2<T>(byte[] aData)
        {
            int size = Marshal.SizeOf(typeof(T));

            if (size < aData.Length)
                return default(T);

            try
            {
                if (IntPtr.Zero == g_ptr)
                {
                    g_ptr = Marshal.AllocHGlobal(size);
                    g_nPtrSize = size;
                }
                else
                {
                    if (g_nPtrSize < size)
                    {
                        Marshal.FreeHGlobal(g_ptr);
                        g_nPtrSize = 0;

                        g_ptr = Marshal.AllocHGlobal(size);
                        g_nPtrSize = size;
                    }
                }

                size = size > aData.Length ? aData.Length : size;
                Marshal.Copy(aData, 0, g_ptr, size);
                T obj = (T)Marshal.PtrToStructure(g_ptr, typeof(T));
                //Marshal.FreeHGlobal(ptr);
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }

        public static T Int2Struct<T>(int[] aData)    //where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            int nArrayDataTypeSize = Marshal.SizeOf(typeof(int));
            int nTotalSize = aData.Length + nArrayDataTypeSize;
            if (size > aData.Length * nArrayDataTypeSize)
                return default(T);

            try
            {
                GCHandle handle = GCHandle.Alloc(aData, GCHandleType.Pinned);
                IntPtr ptr = handle.AddrOfPinnedObject();
                T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
                handle.Free();
                return obj;
            }
            catch (Exception ex)
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

        public static int Load<T>(out T tObject, string strFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            if (null == serializer)
            {
                tObject = default(T);
                return 1;
            }
            XmlReader reader = XmlReader.Create(strFilePath);
            if (null == reader)
            {
                tObject = default(T);
                return 2;
            }
            tObject = (T)serializer.Deserialize(reader);
            if (null == tObject)
            {
                tObject = default(T);
                return 3;
            }
            reader.Close();
            return 0;
        }

        public static int LoadXml<T>(out T tObject, string strXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            if (null == serializer)
            {
                tObject = default(T);
                return 1;
            }

            XmlReader reader = XmlReader.Create(new StringReader(strXml));
            if (null == reader)
            {
                tObject = default(T);
                return 2;
            }
            tObject = (T)serializer.Deserialize(reader);
            if (null == tObject)
            {
                tObject = default(T);
                return 3;
            }
            reader.Close();
            return 0;
        }

        public static int Save<T>(T tObject, string strFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            if (null == serializer)
            {
                tObject = default(T);
                return 1;
            }
            StringBuilder resutl = new StringBuilder();
            if (null == resutl)
            {
                tObject = default(T);
                return 2;
            }
            XmlWriter writer = XmlWriter.Create(strFilePath);
            if (null == writer)
            {
                tObject = default(T);
                return 3;
            }
            serializer.Serialize(writer, tObject);
            writer.Close();
            return 0;
        }

        public static int SaveXml<T>(T tObject, out string strOutXml)
        {
            XmlSerializer serializer = null;
            if (typeof(T) == tObject.GetType())
                serializer = new XmlSerializer(typeof(T));
            else
                serializer = new XmlSerializer(tObject.GetType());

            strOutXml = string.Empty;

            if (null == serializer)
            {
                tObject = default(T);
                return 1;
            }

            StringBuilder resutl = new StringBuilder();
            if (null == resutl)
            {
                tObject = default(T);
                return 2;
            }
            StringBuilder strbXml = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(strbXml);
            if (null == writer)
            {
                tObject = default(T);
                return 3;
            }
            serializer.Serialize(writer, tObject);
            writer.Close();
            strOutXml = strbXml.ToString();
            return 0;
        }


    }
}
