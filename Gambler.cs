using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MizgirTotalizator
{
    public delegate void PutBet(int gamerNum, int mizgirNum, int bet);
    public delegate void GetBet(int gamerNum, int bet);

    public class Gambler
    {
        public int Number;
        public string Name;
        public int Cash;
        public bool HasBet;//сделал ли данный игрок ставку

        public PutBet putBet;
        public GetBet getBet;


        public Gambler(int num, string name, int cash)
        {
            Number = num;
            Name = name;
            Cash = cash;
            HasBet = false;
        }

    }
}
