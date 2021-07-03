using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace TextProfiler
{
    public class Line
    {
        public int Row { set; get; }
        public string Text { get; set; }
        public string Pattern
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var it in Items)
                    sb.Append(it.Pattern);
                return sb.ToString();

            }
        }


        public List<Item> Items { set; get; }

        public Line()
        {
            Row = 0;
            Text = "";
            Items = new List<Item>();
        }
        public void Process()
        {
        }
        public void AddLine(string text)
        {
            Text = text;
            var cols = text.Split('\t');
            for(int i=0; i< cols.Length; i++)
            {
                var it = new Item();
                it.Col = i;
                it.OriginalValue = cols[i].Trim();
            }
        }
    }
}
