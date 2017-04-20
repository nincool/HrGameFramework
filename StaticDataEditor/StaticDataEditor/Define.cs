using System;

namespace StaticData
{
    class Define
    {
        public static string GetExcleFile()
        {
            return string.Format("{0}{1}", Environment.CurrentDirectory, "\\xls\\");
        }

        public static string GetByteFile(string fileName)
        {
            return string.Format("{0}{1}{2}{3}", Environment.CurrentDirectory, "\\data\\", fileName.ToLower(),".bytes");
        }
    }
}
