using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer.HTTP
{
    public class Header
    {
        public Header(string headerAsString)
        {
            var parsedLine = headerAsString.Split(": ", 2, StringSplitOptions.None);
            this.Key = parsedLine[0];
            this.Value = parsedLine[1];
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
