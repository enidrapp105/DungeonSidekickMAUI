using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSidekickMAUI
{
    public class CharacterSheet
    {
        // Private static instance variable
        private static CharacterSheet instance;

        // Private constructor to prevent instantiation
        private CharacterSheet() { }

        // Public static method to access the singleton instance
        public static CharacterSheet Instance
        {
            get
            {
                // Lazy initialization: create the instance if it doesn't exist
                // Not thread safe, but I think that is a bit overkill
                if (instance == null)
                {
                    instance = new CharacterSheet();
                    instance.Purge();
                }
                return instance;
            }
        }

        // Properties
        public string? charactername { get; set; }
        public string? race { get; set; }
        public string? characterclass { get; set; }
        public string? background { get; set; }
        public string? alignment { get; set; }
        public string? personalitytraits { get; set; }
        public string? ideals { get; set; }
        public string? bonds { get; set; }
        public string? flaws { get; set; }
        public string? featurestraits { get; set; }
        public string? proficiencies { get; set; }
        public string? attacks { get; set; }
        public int strength { get; set; }
        public int dexterity { get; set; }
        public int constitution { get; set; }
        public int intelligence { get; set; }
        public int wisdom { get; set; }
        public int charisma { get; set; }
        public bool exists { get; set; }
        /*
         * Function: Purge
         * Author: Brendon Williams
         * Purpose: Resets the instance back to nulls. This way, we can reset the instance easily
         * last Modified : 2/22/2024 8:15 pm
         */
        public void Purge()
        {
            charactername = null;
            race = null;
            characterclass = null;
            background = null;
            alignment = null;
            personalitytraits = null;
            ideals = null;
            bonds = null;
            flaws = null;
            featurestraits = null;
            proficiencies = null;
            attacks = null;
            strength = 0;
            dexterity = 0;
            constitution = 0;
            intelligence = 0;
            wisdom = 0;
            charisma = 0;
            exists = false;
        }
    }

}
