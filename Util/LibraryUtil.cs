using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AKI.Util
{

    public class LibraryUtil
    {
        public static bool Load<T>(string strDirectory, List<string> listExceptionFiles, ref List<T> listT)
        {
            if (null == listT)
                listT = new List<T>();
            else
                listT.Clear();
                        
            if (!Directory.Exists(strDirectory))
                return false;

            DirectoryInfo directoryInfo = new DirectoryInfo(strDirectory);
            FileInfo[] files = directoryInfo.GetFiles("*.dll");

            if (files.Length == 0)
                return false;

            foreach (FileInfo fileInfo in files)
            {
                if (listExceptionFiles.Contains(fileInfo.Name))
                    continue;

                try
                {
                    Assembly assy = Assembly.LoadFile(fileInfo.FullName);
                    Type[] types = assy.GetTypes();
                    T temp = default(T);
                    foreach (Type type in types)
                    {
                        try
                        {
                            if (type.GetInterface(typeof(T).Name) != null)
                            {
                                temp = (T)assy.CreateInstance(type.FullName);
                                listT.Add(temp);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("LibraryUtil.Load<T>() : Exception = " + ex.Message);
                            //return false;
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("LibraryUtil.Load<T>() : Exception = " + ex.Message);
                    //return false;
                    continue;
                }
            }

            return true;
        }

    }
}
