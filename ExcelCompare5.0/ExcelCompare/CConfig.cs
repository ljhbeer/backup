using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelCompare
{
    class CConfig
    {
        public CConfig()
        {
            XlsName = "";
        }
        public string XlsName { get; set; }
        public string ShortName
        {
            get
            {
                if (XlsName ==  "") return "";
                return XlsName.Substring( XlsName.LastIndexOf("\\")+1);
            }
        }
    }
}
