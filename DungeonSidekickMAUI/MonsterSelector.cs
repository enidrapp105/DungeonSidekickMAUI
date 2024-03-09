using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSidekickMAUI
{
    /*
     * Class: MonsterSelector
     * Author: Brendon Williams
     * Purpose: This has a list of all the monsters the user has selected
     * Last Modified: 3/9/2024 12:33pm by Author
     */
    class MonsterSelector
    {
        private List<Monster> m_monsters { get; set; }

        private static MonsterSelector instance;

        private MonsterSelector()
        {
            m_monsters = new List<Monster>();
        }

        public void Purge()
        {
            m_monsters.Clear();
        }

        public static MonsterSelector Instance
        {
            get
            {
                // Lazy initialization: create the instance if it doesn't exist
                // Not thread safe, but I think that is a bit overkill
                if (instance == null)
                {
                    instance = new MonsterSelector();
                }
                return instance;
            }
        }


        /*
         * Class: RemoveMonster
         * Author: Brendon Williams
         * Purpose: Public Call to the pRemoveMonster function
         * Last Modified: 3/9/2024 1:05pm by Author
         */
        public void RemoveMonster(string mName)
        {
            if (instance != null)
                p_RemoveMonster(FindMonster(mName));
        }

        /*
         * Class: pRemoveMonster
         * Author: Brendon Williams
         * Purpose: Takes a monster and removes it from the list. More general option to the damage function
         * Last Modified: 3/9/2024 1:05pm by Author
         */
        private void p_RemoveMonster(Monster monster)
        {
            if (monster.AC == 0)
                return;
            m_monsters.Remove(monster);
        }

        /*
         * Class: findMonster
         * Author: Brendon Williams
         * Purpose: Takes a string and finds the monster in the list
         * Last Modified: 3/9/2024 1:05pm by Author
         */
        private Monster FindMonster(string mName)
        {
            Monster monster = new Monster();

            foreach (Monster creature in m_monsters)
            {
                if (creature.Name == mName)
                    return monster = creature;
            }

            return monster;
        }

        /*
         * Class: DamageMonster
         * Author: Brendon Williams
         * Purpose: Takes a string and a damage number. Gets the monster node in the list and then accurately applies damage and checks if the monster is dead
         * Last Modified: 3/9/2024 1:14pm by Author
         */
        public void DamageMonster(string mName, int dmg)
        {
            Monster monster = FindMonster(mName);
            /*monster.HP -= dmg;
              if(monster.HP <= 0)
                  p_RemoveMonster(monster);
            */
        }

        /*
         * Class: AddMonster
         * Author: Brendon Williams
         * Purpose: Adds Monster Node to the list
         * Last Modified: 3/9/2024 1:14pm by Author
         */
        public void AddMonster(Monster mName)
        {
            m_monsters.Add(mName);
        }
    }
}
