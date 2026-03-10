using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ClassConnection
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
