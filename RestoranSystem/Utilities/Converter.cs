using RestoranSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RestoranSystem.Utilities
{
    public static class Converter
    {
        public static string ConvertDecimalToReal(decimal num)
        {
            return Convert.ToString(num).Replace(",", ".").Replace("m", "");
        }

        public static object[] ConvertListStringToListInt(List<string> strLst)
        {
            List<int> MenuOrder = new List<int>();
            List<string> FailedInputs = new List<string>();
            foreach(string str in strLst)
            {
                IEnumerable<string> strArr = str.Split(",").Select(s => s.Trim());
                foreach(string str2 in strArr)
                {
                    int MenuID;
                    bool success = int.TryParse(str2, out MenuID);
                    if(success && MenuID <= 21) // TODO: Čia 21 turėtų būti pakeistas į eilučių skaičių esančių meniu.
                    {
                        MenuOrder.Add(MenuID);
                    }
                    else
                    {
                        FailedInputs.Add(str2);
                    }
                }
            }
            object[] ConvertResults = new object[2] { MenuOrder , FailedInputs };
            return ConvertResults;
        }

        public static bool ConvertToBool(int answer, int valForTrue)
        {
            return answer == valForTrue;
        }
    }
}
