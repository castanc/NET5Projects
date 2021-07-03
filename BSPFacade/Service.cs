using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ExcelHelper;
using Utils;

namespace BSPFacade
{
    public class Service
    {
        public async Task<List<string>> ConvertFolderToTSV(string fileName)
        {
            var res = new List<string>();
            var files = Directory.GetFiles(Path.GetDirectoryName(fileName), "*.csv");

            foreach (var f in files)
            {
                var resultFile = await f.CSVFileToTSV(Encoding.UTF7);
                res.Add(resultFile);
            }
            return res;
        }

        public async Task<string> ConvertToTSV(string fileName, Encoding enc)
        {
            string fName = fileName.GetNewName("", ".tsv");
            return await fileName.CSVFileToTSV(enc);
        }
        public async Task<string> ConvertExcelToTSV(string fileName)
        {
            var fName = fileName;
            if ( Path.GetExtension(fileName).ToLower()==".csv")
                fName = await fileName.UTF7ToUTF8();

            fName = await fName.ExcelToTSV();
            return fName;
        }
    }
}
