using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace TextProfiler
{
    public enum ElementType {  Text, Date, Decimal, Number, Count, Code}
    public class Item
    {
        public int Col { set; get; }
        public string OriginalValue { set; get; }
        public string FinalValue { set; get; }
        public string Pattern { set; get; }
        public string WordPattern { set; get; }


        public void Process(string text = "", string dateFormat = "DMY",
            int timeZone =0 , char decSep = ',', char thousandSep = '.')
        {
            if (text.IsEmpty())
                text = OriginalValue;

            //character level"
            //A: ALPHA A..Z, a..z, SYMBOLS
            //N: NUMBERS 0..9, ., ,() $+-
            //S: SEPARATORS: -_=/.,
            //D: DATES  DDSDDSDDDD, DDDDSDDSDD, DDDDSDD, DDSDDDD
            //P: 0..9, ., %
            //global: group ()

            //WORDS: 
            //ALL ALPHA: A
            //ALL NUMBERS: #
            //DATES: D
            //PERCENTAGE: %
            //MONEY $ AND SEPARATORS
            //CODE # AND SEPARATORS
            //DECIMAL: NUMBERS, .,

            //bank document types
            //fileName
            //date format
            //decimal separator
            //thousand separator
            
            var words = text.Split(' ');
            List<string> pattern = new List<string>();
            List<string> value = new List<string>();
            char[] ptrn = Array.Empty<char>();
            foreach(var w in words )
            {
                ptrn = w.ToCharArray();
                for (int i = 0; i < ptrn.Length; i++)
                {
                    if (ptrn[i].InRange('0', '9'))
                        ptrn[i] = '#';
                    else if (!ptrn[i].In(',', '.','-','+', '(', ')','$','/'))
                        ptrn[i] = 'A';
                }
            }
            Pattern = string.Join("", ptrn);
            FinalValue = OriginalValue;
            if (Pattern.Contains("A"))
            {
                WordPattern = "A";
            }
            else if (Pattern.Contains("#") &&
                Pattern.ContainsAny(',', '.', '$', '(', ')'))
            {
                //it is a DECIMAL value. remove $, thouysand sep, () to negative
                FinalValue = OriginalValue.Replace("$", "");
                FinalValue = FinalValue.Replace($"{thousandSep}", "");
                if (FinalValue.Contains(decSep))
                    FinalValue = FinalValue.Replace(decSep, '.');
                if (FinalValue.ContainsAny('-', '(', ')'))
                {
                    FinalValue = FinalValue.Replace("-", "")
                        .Replace("(", "")
                        .Replace(")", "");
                    FinalValue = $"-{FinalValue}";
                }

            }
            else if (Pattern.Contains("#") &&
                Pattern.ContainsAny('/', '-', '.'))
            {
                //todo: check date pattern, if value doesnt match
                //numer of parts for a date ie ##/## or ##-##, it is a count
                WordPattern = "D";
            }
            else WordPattern = "#";


        }
    }
}
