using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondApp
{
    public static class Util
    {
        public static string ReadFile(string filename)
        {
            string content;
            using(StreamReader streamReader = new StreamReader(filename, Encoding.UTF8))
            {
                content = streamReader.ReadToEnd();
            }
            return content;
        }
    }
}
