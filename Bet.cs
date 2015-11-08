using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MizgirTotalizator
{
    class Bet
    {
        public Mizgir Mizgir;
        public int Cost;

        public Bet(Mizgir mizgir, int cost)
        {
            Mizgir = mizgir;
            Cost = cost;
        }
    }
}
