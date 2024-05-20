using Microsoft.Maui.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * Class: DiceRoll
 * Author: Kenny Rapp
 * Purpose: Because almost all of our data in our dat base being strings
 * we need a function to handle data that is stored like "1d20" and add modifiers to it like "+2"
 * deliminated by a ","
 * Last Modified: 2/7/2024
 */
namespace DungeonSidekickMAUI
{
    class DiceRoll
    {
        Random random;

        public DiceRoll()
        {
            long ticks = DateTime.Now.Ticks;
            this.random = new Random((int)ticks);
        }

        public int Roll(string input)
        {
            long ticks = DateTime.Now.Ticks;
            this.random = new Random((int)ticks);
            int sum = ParseAndRoll(input);
            return sum;
        }

        /*
         * Function: RollDice
         * Author: Kenny Rapp
         * Purpose: to roll dice and return the rolled sum
         * last Modified: 2/4/2024 6:21pm By Kenny Rapp
         */
        private int RollDice(int numberOfDice, int numberOfFaces)
        {
            int totalSum = 0;
            if (numberOfFaces > 0)
            {
                for (int i = 0; i < numberOfDice; i++)
                {
                    int rollResult = random.Next(1, numberOfFaces+1);
                    totalSum += rollResult;
                }
            }
            return totalSum;
        }

        /*
         * Function: ParseAndRoll
         * Author: Kenny Rapp
         * Purpose: to parse the input string and return the rolled sum
         * last Modified: 2/4/2024 6:21pm By Kenny Rapp
         */
        private int ParseAndRoll(string input)
        {
            if(input == null)
            {
                return 0;
            }
            string[] parts = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int totalResult = 0;

            foreach (var part in parts)
            {
                if (part.Contains("d"))
                {
                    // This part contains a dice roll
                    string[] dicePart = part.Split('d');
                    char firstChar = dicePart[0][0];
                    int startIndex = Char.IsDigit(firstChar) ? 0 : 1;
                    int numberOfDice = int.Parse(dicePart[0].Substring(startIndex));
                    int numberOfFaces = int.Parse(dicePart[1]);

                    int rollResult = RollDice(numberOfDice, numberOfFaces);
                    totalResult += rollResult;
                }
                else if (part.Contains("-") || part.Contains("+"))
                {
                    // This part is a modifier
                    char firstChar = part[0];
                    int startIndex = Char.IsDigit(firstChar) ? 0 : 1;
                    int customValue = int.Parse(part.Substring(startIndex));
                    if (firstChar == '+')
                        totalResult += customValue;
                    else if (firstChar == '-')
                        totalResult -= customValue;
                }
                else
                {
                    int customValue = int.Parse(part);
                    totalResult -= customValue;
                }
            }

            return totalResult;
        }


    }

}
