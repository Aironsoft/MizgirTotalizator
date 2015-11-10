using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;//для PictureBox

namespace MizgirTotalizator
{
    public delegate void Move(Bug mizgir, int step);

    public class Bug
    {
        public int Number;
        public PictureBox Picture = new PictureBox();
        public int Position;
        public List<Bet> Bets;
        public bool IsWinner;
        public Move move;

        public Bug(int num, PictureBox pb)
        {
            Number = num;
            Picture = pb;
            Position = 0;
            Bets = new List<Bet>();
            IsWinner = false;
        }
    }
}
