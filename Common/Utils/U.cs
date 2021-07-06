using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class U
    {

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        public static string ChangeExtension(this string fileName, string ext)
        {
            return $"{Path.GetDirectoryName(fileName)}\\{Path.GetFileNameWithoutExtension(fileName)}{ext}";
        }

        public static bool ContainsAny(this string text, params char[] chrs)
        {
            if (chrs == null)
                throw new ArgumentNullException("items");
            BlockingCollection<string> resultList = new BlockingCollection<string>();

            Parallel.ForEach(chrs, ch =>
            {
                if (text.Contains(ch))
                    resultList.TryAdd("Y");
                else
                    resultList.TryAdd("N");
            }
            );
            var res2 = string.Join("", resultList);
            return res2.Contains("Y");
        }

        public static async Task<string> UTF7ToUTF8(this string fileName)
        {
            string fName = "";
            if ( File.Exists(fileName))
            {
                fName = fileName.GetNewName("_UTF8");
                string text = await File.ReadAllTextAsync(fileName, Encoding.UTF7);
                await File.WriteAllTextAsync(fName, text, Encoding.UTF8);
            }
            return fName;
        }


        public static bool In<T>(this T item, params T[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            return items.Contains(item);
        }

        public static bool InRange<T>(this T item, params T[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            return items.Contains(item);
        }


        public static bool TryDelete(this string fileName)
        {
            bool result = false;
            if (File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                    result = true;
                }
                catch(Exception ex)
                {
                    result = false;
                }
            }
            return result;
        }
                
        public static string GetNewName(this string fileName, string postFix,
            string ext = "")
        {
            if (ext.IsEmpty())
                ext = Path.GetExtension(fileName);

            return $"{Path.GetDirectoryName(fileName)}\\{Path.GetFileNameWithoutExtension(fileName)}{postFix}{ext}";
        }
        public static string RemoveExcessBlanks(this string text)
        {
            while (text.Contains("  "))
                text = text.Replace("  ", " ");
            return text;
        }

        public static bool IsEmpty(this string text)
        {
            return text?.Trim() == string.Empty;
        }

        public static async Task<string> CSVFileToTSV(this string fileName, 
            Encoding enc,
            string extension = ".tsv", string encloser = "\"", 
           string colSeparator = "\t", string origColSep = ",")
        {
            var lines = await File.ReadAllLinesAsync(fileName, enc);

            for (int i = 0; i < lines.Length; i++)
                lines[i] = lines[i].CSVToTSV(encloser, colSeparator, origColSep);

            if (extension.IsEmpty())
                extension = ".tsv";

            var fName = fileName.GetNewName("", extension);
            await File.WriteAllLinesAsync(fName, lines, Encoding.UTF8);
            return fName;
        }

        //public static string CSVToTSV(this string line, char encloser = '"',
        //    char colSeparator = '\t', char origColSep = ',')
        //{
        //    if (line.Contains($"{origColSep}{encloser}") ||
        //         line.Contains($"{encloser}{origColSep}"))
        //    {
        //        line = line.Replace($"{encloser}{origColSep}{encloser}", $"{colSeparator}");
        //        line = line.Replace($"{encloser}{origColSep}", $"{colSeparator}");
        //        line = line.Replace($"{origColSep}{encloser}", $"{colSeparator}");
        //    }
        //    else
        //    {
        //        if (line.Contains($"{encloser}"))
        //            line = line.Replace($"{encloser}", "'");
        //        line = line.Replace($"{origColSep}", $"{colSeparator}");
        //    }
        //    return line;

        //}

        public static string CSVToTSV(this string line, string encloser = "\"",
            string colSeparator = "\t", string origColSep = ",")
        {
            bool withinSep = false;
            var cha = line.Trim().ToArray();
            for (int i = 0; i < cha.Length; i++)
            {
                if (cha[i] == encloser[0])
                    withinSep = !withinSep;
                else
                {
                    if (cha[i] == origColSep[0] && withinSep)
                        cha[i] = '`';
                }
            }
            line = string.Join("", cha);

            line = line.Replace($"{encloser}{origColSep}{encloser}", colSeparator);
            line = line.Replace($"{encloser}{origColSep}", colSeparator);
            line = line.Replace($"{origColSep}{encloser}", $"{colSeparator}");
            line = line.Replace(encloser, string.Empty);

            line = line.Replace($"{origColSep}", $"{colSeparator}");
            line = line.Replace("`", origColSep);
            return line;

        }


        public static string[] GetStringArray(this 
            BlockingCollection<string[]> collection, 
            char colSeparator = '\t' )
        {
            BlockingCollection<string> lines = new BlockingCollection<string>();
            Parallel.ForEach(collection, line =>
            {
                lines.TryAdd(string.Join($"{colSeparator}", line));
            });
            return lines.ToArray();
        }

        public static string RemoveSpaceSeparators(this string text, 
            string value = "   ", string replace = "  ", string separator = "\t")
        {
            while (text.Contains(value))
                text = text.Replace(value, replace);
            text = text.Replace(replace, separator);
            return text;
        }

        public static string GetStandardName(this string text)
        {
            text = text.Trim().Replace(" ", "_");
            var chars = text.ToCharArray();
            chars = chars.Where(x => (x >= '0' && x <= '9') ||
            (x >= 'a' && x <= 'z') ||
            (x >= 'A' && x <= 'Z') ||
            x == '-' || x == '_' ).ToArray();

            text = string.Join("", chars);

            return text;
        }

    }
}
