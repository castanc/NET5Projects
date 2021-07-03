using IronXL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ExcelHelper
{
    public static class Excel
    {
        public static async Task<string> ExcelToTSV(this string fileName)
        {
            string fName = "";
            if (File.Exists(fileName))
            {
                fName = fileName.GetNewName("", ".tsv");
                WorkBook workbook = WorkBook.Load(fileName);
                WorkSheet sheet = workbook.WorkSheets.First();
                List<string> result = new List<string>();

                foreach(var row in sheet.Rows)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var col in row.Columns)
                    {
                        sb.Append(col.StringValue.Replace("\n"," ").Trim());
                        sb.Append('\t');
                    }
                    result.Add(sb.ToString());
                }

                await File.WriteAllLinesAsync(fName, result);
            }
            return fName;
        }
    }
}
