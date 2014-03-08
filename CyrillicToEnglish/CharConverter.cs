using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MP3TagUpdater
{
    public class CharConverter
    {
        readonly string[] _translit = 
        {
            "ё","yo","а","a","б","b","в","v","г","g","д","d","е","e","ж","zh","з","z","и","i","й","j","к","k","л","l","м","m","н","n","о","o","п","p","р","r","с","s","т","t","у","u","ф","f","х","h","ц","c","ч","ch","ш","sh","щ","sch","ъ","_","ы","y","ь","`","э","e`","ю","yu","я","ya","Ё","YO","А","A","Б","B","В","V","Г","G","Д","D","Е","E","Ж","Zh","З","Z","И","I","Й","J","К","K","Л","L","М","M","Н","N","О","O","П","P","Р","R","С","S","Т","T","У","U","Ф","F","Х","H","Ц","C","Ч","Ch","Ш","Sh","Щ","Sch","Ъ","_","Ы","Y","Ь","`","Э","E`","Ю","Yu","Я","Ya"
        };
        Dictionary<string, string> _ctoe = new Dictionary<string, string>();
        public CharConverter()
        {
            for (int i = 0; i < _translit.Length - 1; i+=2)
            {
                _ctoe.Add(_translit[i], _translit[i + 1]);
            }
        }

        private string GetLatin(string source)
        {
            if (_ctoe.ContainsKey(source))
                return _ctoe[source];
            else
                if (source.ToCharArray()[0] == 1110)
                    return "i";
                else
                    return source;
        }

        public string ConvertString(string source)
        {
            StringBuilder result = new StringBuilder();
            foreach (char item in source.ToCharArray())
            {
                result.Append(GetLatin(item.ToString()));
            }
            return result.ToString();
        }
    }
}
