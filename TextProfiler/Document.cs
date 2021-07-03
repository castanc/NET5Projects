using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProfiler
{
    public class Document
    {
        public string FileName { set; get; }
        public string DocCode { set; get; }
        public string DateFormat { set; get; }
        public string ThousandSeparator { set; get; }
        public string DecimalSeparator { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateProcessed { set; get; }

        public List<Line> Lines { set; get; }
    }
}
