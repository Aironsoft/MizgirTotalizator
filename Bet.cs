using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MizgirTotalizator
{
    public class Bet
    {
        public Gambler Gamer;
        public int Cost;

        public Bet(Gambler gamer, int cost)
        {
            Gamer = gamer;
            Cost = cost;
        }


    }
}
