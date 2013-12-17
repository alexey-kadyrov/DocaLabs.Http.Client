using System;
using System.Threading;
using DocaLabs.Test.Utils.DotNet;

namespace DocaLabs.Test.Services.Proxy
{
    class Program
    {
        static void Main()
        {
            try
            {
                using (new WcfServerHost<TestServicesProxy>())
                {
                    Console.WriteLine();
                    Console.WriteLine(" -- press enter key to quit.");
                    Console.WriteLine();

                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
