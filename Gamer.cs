using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MizgirTotalizator
{
    class Gamer
    {
        public int Number;
        public string Name;
        public int Cash;
        public Bet Bet;
        public bool HasBet;//сделал ли данный игрок ставку

        public Gamer(int num, string name, int cash)
        {
            Number = num;
            Name = name;
            Cash = cash;
            Bet = null;
            HasBet = false;
        }
    }
}
