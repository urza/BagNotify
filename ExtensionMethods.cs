using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagNotify
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Try to parse string to float with either "." or "," as decimal separator. Must not contain both and must be only once in input string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float FloatParseCzEn(this string value)
        {
            float result = 0;
            string cislo = value;

            bool proslo = float.TryParse(cislo, out result);

            if (proslo)
                return result;
            else
            {
                if (value.Contains(','))
                    cislo = cislo.Replace(',', '.');
                else if (value.Contains('.'))
                    cislo = cislo.Replace('.', ',');

                proslo = float.TryParse(cislo, out result);

                if (proslo)
                    return result;
                else
                    throw new ArgumentException();
            }
        }
    }
}
