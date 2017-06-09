using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Utilities
{
    public static class StringsParser
    {
        public static Dictionary<string, string> ParseGroupedString(char groups_delimeter, char params_delimeter, string StringForParsing)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();

            string[] first_pass = StringForParsing.Split(groups_delimeter);
            string[] second_pass = null;
            if (first_pass.Length > 0)
            {
                for (int i = 0; i < first_pass.Length; i++)
                {
                    second_pass = first_pass[i].Split(params_delimeter);
                    if (second_pass.Length == 2)                    
                        res.Add(second_pass[0], second_pass[1]);                    
                }
            }

            return res;
        }
    }
}
