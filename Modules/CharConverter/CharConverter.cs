using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;
using Mp3.Infrastructure;

namespace Modules.CharConverter
{
    [Export(typeof(IStringConverter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CharConverter : IStringConverter
    {
        ILoggerFacade _logger;
        IUnityContainer _container;

        #region Conversion Tables
        readonly string[] _translit = 
        {
            "ё","yo","а","a","б","b","в","v","г","g","д","d","е","e","ж","zh","з","z","и","i","й","j","к","k","л","l","м","m","н","n","о","o","п","p","р","r","с","s","т","t","у","u","ф","f","х","h","ц","c","ч","ch","ш","sh","щ","sch","ъ","_","ы","y","ь","`","э","e`","ю","yu","я","ya","Ё","YO","А","A","Б","B","В","V","Г","G","Д","D","Е","E","Ж","Zh","З","Z","И","I","Й","J","К","K","Л","L","М","M","Н","N","О","O","П","P","Р","R","С","S","Т","T","У","U","Ф","F","Х","H","Ц","C","Ч","Ch","Ш","Sh","Щ","Sch","Ъ","_","Ы","Y","Ь","`","Э","E`","Ю","Yu","Я","Ya"
        };
        readonly char[] _iso_8859 = 
        {
            System.Convert.ToChar(161),'Ё', 
            System.Convert.ToChar(162),'Ђ', 
            System.Convert.ToChar(163),'Ѓ', 
            System.Convert.ToChar(164),'Є', 
            System.Convert.ToChar(165),'Ѕ', 
            System.Convert.ToChar(166),'І', 
            System.Convert.ToChar(167),'Ї', 
            System.Convert.ToChar(168),'Ј', 
            System.Convert.ToChar(169),'Љ', 
            System.Convert.ToChar(170),'Њ', 
            System.Convert.ToChar(171),'Ћ', 
            System.Convert.ToChar(172),'Ќ', 
            System.Convert.ToChar(173),'­', 
            System.Convert.ToChar(174),'Ў', 
            System.Convert.ToChar(175),'Џ', 
            System.Convert.ToChar(176),'А', 
            System.Convert.ToChar(177),'Б', 
            System.Convert.ToChar(178),'В', 
            System.Convert.ToChar(179),'Г', 
            System.Convert.ToChar(180),'Д', 
            System.Convert.ToChar(181),'Е', 
            System.Convert.ToChar(182),'Ж', 
            System.Convert.ToChar(183),'З', 
            System.Convert.ToChar(184),'И', 
            System.Convert.ToChar(185),'Й', 
            System.Convert.ToChar(186),'К', 
            System.Convert.ToChar(187),'Л', 
            System.Convert.ToChar(188),'М', 
            System.Convert.ToChar(189),'Н', 
            System.Convert.ToChar(190),'О', 
            System.Convert.ToChar(191),'П', 
            System.Convert.ToChar(192),'Р', 
            System.Convert.ToChar(193),'С', 
            System.Convert.ToChar(194),'Т', 
            System.Convert.ToChar(195),'У', 
            System.Convert.ToChar(196),'Ф', 
            System.Convert.ToChar(197),'Х', 
            System.Convert.ToChar(198),'Ц', 
            System.Convert.ToChar(199),'Ч', 
            System.Convert.ToChar(200),'Ш', 
            System.Convert.ToChar(201),'Щ', 
            System.Convert.ToChar(202),'Ъ', 
            System.Convert.ToChar(203),'Ы', 
            System.Convert.ToChar(204),'Ь', 
            System.Convert.ToChar(205),'Э', 
            System.Convert.ToChar(206),'Ю', 
            System.Convert.ToChar(207),'Я', 
            System.Convert.ToChar(208),'а', 
            System.Convert.ToChar(209),'б', 
            System.Convert.ToChar(210),'в', 
            System.Convert.ToChar(211),'г', 
            System.Convert.ToChar(212),'д', 
            System.Convert.ToChar(213),'е', 
            System.Convert.ToChar(214),'ж', 
            System.Convert.ToChar(215),'з', 
            System.Convert.ToChar(216),'и', 
            System.Convert.ToChar(217),'й', 
            System.Convert.ToChar(218),'к', 
            System.Convert.ToChar(219),'л', 
            System.Convert.ToChar(220),'м', 
            System.Convert.ToChar(221),'н', 
            System.Convert.ToChar(222),'о', 
            System.Convert.ToChar(223),'п', 
            System.Convert.ToChar(224),'р', 
            System.Convert.ToChar(225),'с', 
            System.Convert.ToChar(226),'т', 
            System.Convert.ToChar(227),'у', 
            System.Convert.ToChar(228),'ф', 
            System.Convert.ToChar(229),'х', 
            System.Convert.ToChar(230),'ц', 
            System.Convert.ToChar(231),'ч', 
            System.Convert.ToChar(232),'ш', 
            System.Convert.ToChar(233),'щ', 
            System.Convert.ToChar(234),'ъ', 
            System.Convert.ToChar(235),'ы', 
            System.Convert.ToChar(236),'ь', 
            System.Convert.ToChar(237),'э', 
            System.Convert.ToChar(238),'ю', 
            System.Convert.ToChar(239),'я', 
            System.Convert.ToChar(240),'№', 
            System.Convert.ToChar(241),'ё', 
            System.Convert.ToChar(242),'ђ', 
            System.Convert.ToChar(243),'ѓ', 
            System.Convert.ToChar(244),'є', 
            System.Convert.ToChar(245),'ѕ', 
            System.Convert.ToChar(246),'і', 
            System.Convert.ToChar(247),'ї', 
            System.Convert.ToChar(248),'ј', 
            System.Convert.ToChar(249),'љ', 
            System.Convert.ToChar(250),'њ', 
            System.Convert.ToChar(251),'ћ', 
            System.Convert.ToChar(252),'ќ', 
            System.Convert.ToChar(253),'§', 
            System.Convert.ToChar(254),'ў', 
            System.Convert.ToChar(255),'џ'
        };
        #endregion

        Dictionary<string, string> _ctoe = new Dictionary<string, string>();
        Dictionary<char, char> _iso = new Dictionary<char, char>();
        public CharConverter(ILoggerFacade logger, IUnityContainer container)
        {
            _logger = logger;
            _container = container;

            for (int i = 0; i < _translit.Length - 1; i += 2)
            {
                _ctoe.Add(_translit[i], _translit[i + 1]);
            }
            for (int i = 0; i < _iso_8859.Length - 1; i += 2)
            {
                _iso.Add(_iso_8859[i], _iso_8859[i + 1]);
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
                    if (_iso.ContainsKey(System.Convert.ToChar(source)))
                        return GetLatin(_iso[System.Convert.ToChar(source)].ToString());
                    else
                        return source;
        }

        public string Convert(string source)
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
