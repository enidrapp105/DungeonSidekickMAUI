using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSidekickMAUI
{

    /*
    THIS FUNCTION IS TO BE ABLE TO CHECK A LIST OF STRINGS IF THEY ARE PARSIBLE TO WHAT WE WANT OF THE SAME TYPE, AND ARE NOT BLANK
    TYPE 0: STRING CHECK FOR EXTENDED ASCII
    TYPE 1: INT CHECK
    TYPE 2: DOUBLE CHECK
    */
    public static class GlobalFunctions
    {
        public static bool entryCheck(List<String> list, int type)
        {
            // Error handling
            if (list.Count == 0)
                return false;
            foreach (string str in list)
            {
                if (string.IsNullOrEmpty(str))
                {
                    Application.Current.MainPage.DisplayAlert("Text Entry Failure", "Please ensure that all Textboxes are have text in them!", "OK");
                    return false;
                }
            }

            switch (type)
            {
                case 0:
                    // String Case
                    foreach (string str in list)
                    {
                        if (checkExtendedAscii(str) == false)
                        {
                            Application.Current.MainPage.DisplayAlert("Text Entry Failure", "Extended ASCII letters detected, please use standard characters!", "OK");
                            return false;
                        }
                    }
                    break;
                case 1:
                    // Int Case
                    foreach (string str in list)
                    {
                        if (!int.TryParse(str, out _))
                        {
                            Application.Current.MainPage.DisplayAlert("Text Entry Failure", "Please enter numbers in the entered fields!", "OK");
                            return false;
                        }
                    }
                    break;
                case 2:
                    // Double Case
                    foreach (string str in list)
                    {
                        if (!double.TryParse(str, out _))
                        {
                            Application.Current.MainPage.DisplayAlert("Text Entry Failure", "Please enter numbers in the entered fields!", "OK");
                            return false;
                        }
                    }
                    break;
                default:
                    // No defined type case
                    return false;
            }
            return true;
        }
        public static bool checkExtendedAscii(string str)
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
