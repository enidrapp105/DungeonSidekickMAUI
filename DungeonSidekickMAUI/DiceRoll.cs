using Microsoft.Maui.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    int rollResult = random.Next(1, numberOfFaces + 1);
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
            string[] parts = input.Split(new char[] { '+', '-' }, StringSplitOptions.RemoveEmptyEntries);

            int totalResult = 0;

            foreach (var part in parts)
            {
                if (part.Contains("d"))
                {
                    string[] dicePart = part.Split('d');
                    int numberOfDice = int.Parse(dicePart[0]);
                    int numberOfFaces = int.Parse(dicePart[1]);

                    totalResult += RollDice(numberOfDice, numberOfFaces);
                }
                else
                {
                    int modifier = int.Parse(part);
                    totalResult += modifier;
                }
            }

            return totalResult;
        }
    }

}
