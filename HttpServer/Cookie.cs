using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer.HTTP
{
    public class Cookie
    {
        public Cookie()
        {
                
        }
        public string Key { get; set; }
        public string Value { get; set; }

        public static Cookie Parse(string cookieAsString) 
        {
            var cookieKeyValuePair = cookieAsString.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
            return new Cookie()
            {
                Key = cookieKeyValuePair[0],
                Value = cookieKeyValuePair[1]
            };
        }
    }
}
