using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tail
{
    class Program
    {
        static void Main(string[] args)
        {
            Help help = new Help();
            string strResult = help.AnalyzeArgument(args);
            if (null != strResult && string.Empty != strResult)
            {
                System.Console.WriteLine(strResult);
                return;
            }
        }
    }
}
