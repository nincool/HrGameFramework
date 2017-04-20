using System;

namespace StaticData
{
    class Program
    {
        static void Main(string[] args)
        {
            Excel2Bytes excel = new Excel2Bytes();
            string result = string.Empty;
            bool success = excel.Handle(ref result);

            if(!success)
            {
                Console.WriteLine(result);
                Console.ReadKey();
            }
        }
    }
}
