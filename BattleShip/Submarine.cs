using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class Submarine : Ships
    {
        public Submarine()
        {
            name = "Submarine";
            length = 3;
            abbreviation = "[S]";
            hitsOnShip = 0;
        }
    }
}
