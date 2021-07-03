using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class TSVFile
    {
        public string FileName { set; get; }
        public Dictionary<string,ColumnInfo> ColIndex { set; get; }
        public BlockingCollection<string[]> Lines { set; get; }
        private string headerLine = "";
        public string HeaderLine
        {
            set { headerLine = value; }
            get { return headerLine;  }
        }

        public void SetColIndex(string hLine, string dLine = "")
        {
            if (dLine.Trim() == string.Empty)
                dLine = hLine;

            ColIndex = new Dictionary<string, ColumnInfo>();
            var cols = hLine.Split(ColSeparator);
            var dCols = dLine.Split(ColSeparator);

            for(int i=0; i<cols.Length; i++)
            {
                ColumnInfo ci = new ColumnInfo()
                {
                    DisplayText = dCols[i],
                    InternalName = cols[i],
                    Index = i
                };
                ColIndex.Add(cols[i], ci);
                
            }

        }

        public string DisplayLine { set; get; }
        public int HeaderRow { set; get; }
        public int DisplayRow { set; get; }
        public string ColSeparator { set; get; }

        public int TotalRows { get { return (int)Lines?.Count; } }
        public Encoding EncodingSet { set; get; }


        public async Task<TSVFile> LoadTSV(string fileName, 
            Encoding enc, char colSeparator = '\t', int headerRow = 0, 
            int displayRow = -1)
        {
            this.FileName = fileName;
            this.EncodingSet = enc;
            this.ColSeparator = ColSeparator;
            HeaderRow = headerRow;
            DisplayRow = displayRow;


            if (File.Exists(fileName))
            {
                var lines = await File.ReadAllLinesAsync(fileName, EncodingSet);
                if (lines.Length > 0)
                {

                    if (HeaderRow >= 0)
                        HeaderLine = lines[HeaderRow];


                    if (DisplayRow >= 0)
                    {
                        DisplayLine = lines[displayRow];
                    }
                    else DisplayLine = HeaderLine;

                    if (HeaderLine.IsEmpty())
                    {
                        headerRow = 1;
                        HeaderLine = DisplayLine.GetStandardName();
                    }


                    lines = lines.Skip(Math.Max(DisplayRow,HeaderRow)).ToArray();

                    Parallel.ForEach(lines, line => {
                        Lines.TryAdd(line.Split(ColSeparator));
                    });
                }
            }
            return this;
        }


        public async Task<TSVFile> LoadCSV(string fileName,
            Encoding enc, string colSeparator = "\t", 
            string origColSeparator = ",",
            string encloser="\"", int headerRow = 0,
            int displayRow = -1)
        {
            this.FileName = fileName;
            this.EncodingSet = enc;
            this.ColSeparator = ColSeparator;
            HeaderRow = headerRow;
            DisplayRow = displayRow;


            if (File.Exists(fileName))
            {
                var lines = await File.ReadAllLinesAsync(fileName, EncodingSet);
                if (lines.Length > 0)
                {

                    if (HeaderRow >= 0)
                        HeaderLine = lines[HeaderRow].CSVToTSV(encloser,
                            ColSeparator,origColSeparator);


                    if (DisplayRow >= 0)
                        DisplayLine = lines[displayRow].CSVToTSV(encloser, ColSeparator, origColSeparator); 
                    else DisplayLine = HeaderLine;


                    lines = lines.Skip(Math.Max(DisplayRow, HeaderRow)).ToArray();

                    Parallel.ForEach(lines, line => {
                        
                        Lines.TryAdd(line.CSVToTSV(encloser, 
                            ColSeparator, origColSeparator)
                            .Split(ColSeparator));
                    });
                }
            }
            return this;
        }

        public async Task<string> Save(Encoding enc, string view = "", string ext = ".tsv")
        {
            string fName = FileName.GetNewName(view,ext);
            await File.WriteAllTextAsync(fName, "", enc);

            if (!DisplayLine.IsEmpty())
                await File.AppendAllTextAsync(fName, $"{DisplayLine}\r\n", enc);

            if ( !HeaderLine.IsEmpty())
                await File.AppendAllTextAsync(fName, $"{HeaderLine}\r\n", enc);

            if (Lines.Count > 0)
                await File.AppendAllLinesAsync(fName, Lines.GetStringArray(), enc);


            return fName;
        }

    }
}
