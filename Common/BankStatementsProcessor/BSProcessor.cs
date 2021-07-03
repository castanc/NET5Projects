using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace BankStatementsProcessor
{
    public class BSProcessor
    {
        /*
         * header layouts:
         * line labels
         * line value
         * 
         * line label/value ...
         */ 
        public string FileName { set; get; }
        public string BodyTop { set; get; }
        public string BodyBotom { set; get; }
        public ConcurrentDictionary<string, string> Values = new ConcurrentDictionary<string, string>();
        public BlockingCollection<string[]> Lines { set; get; }

        public string TableStart { set; get; }
        public int ShiftStart { set; get; }
        public string TableEnd { set; get; }

        public virtual void AnalizeTop()
        {

        }

        public virtual void AnalizeBottom()
        {

        }

        public virtual void AnalizeLines()
        {

        }

        public virtual async Task<string> Process(string fileName, Encoding enc)
        {
            this.FileName = fileName;
            var lines = await File.ReadAllLinesAsync(fileName, enc);
            Lines = new BlockingCollection<string[]>();
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].RemoveExcessBlanks();
                lines[i] = lines[i].CSVToTSV();
            }
            return "";
        }

        public async void SaveAsTSV(string fileName, Encoding enc)
        {
            this.FileName = fileName;
            var lines = await File.ReadAllLinesAsync(fileName, enc);
            Lines = new BlockingCollection<string[]>();
            for(int i=0; i< lines.Length; i++)
            {
                lines[i] = lines[i].RemoveExcessBlanks();
                lines[i] = lines[i].CSVToTSV();
            }

            var fName = fileName.GetNewName("_CONVERT",".tsv");
            fName.TryDelete();
            await File.WriteAllLinesAsync(fName, lines);
        }
    }
}
