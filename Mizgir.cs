using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;//для PictureBox

namespace MizgirTotalizator
{
    public delegate void Move(Mizgir mizgir, int step);
    public delegate void ToStart(Mizgir mizgir);

    public class Mizgir
    {
        public int Number;
        public PictureBox Picture = new PictureBox();
        public int Position;
        public bool IsWinner;
        public Move move;
        public ToStart toStart;

        public Mizgir(int num, PictureBox pb)
        {
            Number = num;
            Picture = pb;
            Position = 0;
            IsWinner = false;
        }
    }
}
