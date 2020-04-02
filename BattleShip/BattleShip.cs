using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class BattleShip : Ships
    {
        public BattleShip()
        {
            name = "Battle Ship";
            length = 4;
            abbreviation = "[B]";
            hitsOnShip = 0;
        }
    }
}

//comment