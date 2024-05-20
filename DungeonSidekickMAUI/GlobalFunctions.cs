using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSidekickMAUI
{
    public static class GlobalFunctions
    {
        public static bool entryCheck(List<String> list, int type)
        {
            switch (type)
            {
                case 0:
                    // String Case
                    foreach (string str in list)
                    {
                        
                    }
                    break;
                case 1:
                    // Int Case
                    foreach (string str in list)
                    {
                        if (!int.TryParse(str, out _))
                        {
                            return false; // Return false if parsing fails
                        }
                    }
                    break;
                case 2:
                    // Double Case
                    break;
                default:

                    return false;
            }
            return true;
        }
        public static bool CheckExtendedAscii(string str)
        {
            foreach (char c in str)
            {
                if (c > 127)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
