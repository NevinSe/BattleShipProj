using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class Destroyer : Ships
    {
        public Destroyer()
        {
            name = "Destroyer";
            length = 2;
            abbreviation = "[D]";
            hitsOnShip = 0;
        }
    }
}
