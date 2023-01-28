using System;
using System.ComponentModel;
using System.Net.Mail;

namespace LogAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (null == args || 1 > args.Length)
            {
                Console.WriteLine("Run the batch file");
                Console.ReadLine();
                return;
            }
            else
            {
                foreach (string arg in args)
                {
                    Console.WriteLine(arg);
                }
                Console.ReadLine();
            }
            
        }
    }
}
