using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class DataFile
    {
        public string FileName { set; get; }
        public Encoding EncodingSet { set; get; }
        TSVFile Header { set; get; }
        TSVFile Lines { set; get; }
    }
}
