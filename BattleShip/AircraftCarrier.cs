using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class AircraftCarrier : Ships
    {
        public AircraftCarrier()
        {
            name = "Aircraft Carrier";
            length = 5;
            abbreviation = "[A]";
            hitsOnShip = 0;
        }
    }
}
