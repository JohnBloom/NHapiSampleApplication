using System.Collections.Generic;
using System.Linq;

namespace NHapiSampleApplication.Tcp
{
    public class TcpBuffer
    {
        public string Buffer { get; set; }
        
        public void Add(byte[] bytes, int length)
        {
            string result = System.Text.Encoding.ASCII.GetString(bytes, 0, length);
            Buffer += result;
        }

        public IEnumerable<string> GetMessages(char start, char stop)
        {
            var result = new List<string>();
            var endIndex = Buffer.IndexOf(stop);

            while (endIndex > 0)
            {
                var startIndex = Buffer.IndexOf(start);
                if (startIndex < endIndex)
                {
                    result.Add(Buffer.Substring(startIndex, endIndex - startIndex));
                }

                Buffer = Buffer.Substring(endIndex, Buffer.Length - endIndex);

                endIndex = Buffer.IndexOf(stop);
            }

            if (Buffer.Any((x => x == start)) == false)
            {
                Buffer = "";
            }

            return result;
        }
    }
}
