using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _22_24.Classes
{
    public class Common
    {
        public bool IsNumber(string str)
        {
            Match m = Regex.Match(str, @"\+79[0-9]{9}$");
            return m.Success;
        }

        public bool IsFio(string str)
        {
            Match m = Regex.Match(str, @"^[аА-яЯёЁ]{3,15} [аА-яЯ-ёЁ]{3,15} [аА-яЯёЁ]{3,15}$");
            return m.Success;
        }
    }
}
