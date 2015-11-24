using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MizgirTotalizator
{
    public struct CritValues
    {
        public double CritValue;
        public List<int> CritVars; 
    }

    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }


        GameController GC;//класс управления игрой

        List<TableLayoutPanel> Roads;

        public delegate void fMove(Bug mizgir, int step);
        public fMove DelegateMove;
        public delegate void fPrintResults(string s);
        public fPrintResults DelegatePrintResults;
        public delegate void fToStart();
        public fToStart DelegateToStart;


        /// <summary>
        /// Функция, выполняемая при загрузке формы
        /// </summary>
        private void Form_Load(object sender, EventArgs e)
        {
            Roads = new List<TableLayoutPanel>();
            Roads.Add(mizgirRoad1);
            Roads.Add(mizgirRoad2);
            Roads.Add(mizgirRoad3);
            Roads.Add(mizgirRoad4);
            GC = new GameController(this, Roads);
            GC.Initialize();

            DelegateMove = new fMove(Move);
            DelegatePrintResults = new fPrintResults(ResultsPrint);
            DelegateToStart = new fToStart(ToStart);
        }


        public void GCDataInitialize()
        {
            //цикл создания игроков
            for (int i = 0; i < 3; i++)
            {
                Gambler gamer = new Gambler(1 + i, "Участник" + (1 + i), 100);
                gamer.putBet = PutBet;
                gamer.getBet = GetBet;
                GC.Gamers.Add(gamer);
            }

            //создание гонщиков
            Bug mizgir;
            mizgir = new Bug(1, picbMizgir1);
            GC.Mizgirs.Add(mizgir);
            mizgir = new Bug(2, picbMizgir2);
            GC.Mizgirs.Add(mizgir);
            mizgir = new Bug(3, picbMizgir3);
            GC.Mizgirs.Add(mizgir);
            mizgir = new Bug(4, picbMizgir4);
            GC.Mizgirs.Add(mizgir);
            for (int i = 0; i < 4; i++)
            {
                GC.Mizgirs[i].move = Move;
            }

            //создание массива суммарной ставки каждого игрока
            GC.SumBets = new int[GC.Gamers.Count];
            //созданние массива прибыли от ставок
            GC.Profits = new int[GC.Gamers.Count];

            GC.gameNumer = 0;
        }



        public void FormInitialize()
        {
            lbCash.Text = "";
            combMizgir.SelectedIndex = 0;
            btStart.Enabled = false;
        }


        public void ToStart()
        {
            GC.sumBet = 0;
            GC.winBet = 0;
            GC.gameNumer++;
            for(int i=0; i<GC.Gamers.Count; i++)
            {
                GC.Profits[i] = 0;
            }

            combGamer.Enabled = true;
            combMizgir.Enabled = true;
            tbBetSize.Enabled = true;
            rtbBets.Text = "";

            foreach (Bug mizgir in GC.Mizgirs)
            {
                mizgir.IsWinner = false;
                mizgir.Position = 0;
                mizgir.Bets.Clear();

                Roads[mizgir.Number-1].Controls.Add(mizgir.Picture, 0, 0);
                Roads[mizgir.Number - 1].Refresh();
            }
        }


        public void Move(Bug mizgir, int step)
        {
            mizgir.Position += step;
            Roads[mizgir.Number-1].Controls.Add(mizgir.Picture, mizgir.Position, 0);
            Roads[mizgir.Number - 1].Refresh();
        }


        private void combGamer_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbCash.Text = GC.Gamers[combGamer.SelectedIndex].Cash.ToString();

            if(GC.Gamers[combGamer.SelectedIndex].Cash>0)//если у игрока есть деньги
            {
                combMizgir.Enabled = true;
                tbBetSize.Enabled = true;
                btPut.Enabled = true;
            }
            else
            {
                combMizgir.Enabled = false;
                tbBetSize.Enabled = false;
                btPut.Enabled = false;
            }

            //if (GC.Gamers[combGamer.SelectedIndex].HasBet)
            //{
            //    combMizgir.Enabled = false;
            //    tbBetSize.Enabled = false;
            //    btPut.Enabled = false;
            //}
            //else
            //{
            //    combMizgir.Enabled = true;
            //    tbBetSize.Enabled = true;
            //    btPut.Enabled = true;
            //}
        }


        public void PutBet(int gamerNum, int mizgirNum, int bet)
        {
            Bet Bet = new Bet(GC.Gamers[gamerNum], bet);

            GC.Mizgirs[mizgirNum].Bets.Add(Bet);

            GC.Gamers[gamerNum].Cash -= bet;
            lbCash.Text = GC.Gamers[gamerNum].Cash.ToString();
            GC.Gamers[gamerNum].HasBet = true;
            GC.SumBets[gamerNum] += bet;

            rtbBets.Text+= "Участник" + (gamerNum+1) + " поставил " + bet + " на Мизгирь" + (mizgirNum + 1)+"\n";


            GC.sumBet += bet;
        }


        public void GetBet(int gamerInd, int bet)
        {
            int win = (int)((double)bet / GC.winBet * GC.sumBet);
            GC.Gamers[gamerInd].Cash += win+5;
            GC.Gamers[gamerInd].HasBet = false;
            GC.Profits[gamerInd] += win;
        }


        private void btPut_Click(object sender, EventArgs e)
        {
            if(tbBetSize.Text.Length>0)
            {
                int bet=0;
                try
                {
                    bet = Convert.ToInt32(tbBetSize.Text);
                }
                catch
                {
                    MessageBox.Show("Ошибка: неправильное значение ставки");
                    return;
                }

                if(bet > GC.Gamers[combGamer.SelectedIndex].Cash)
                {
                    MessageBox.Show("Ошибка: у игрока недостаточно средств для такой ставки");
                    return;
                }
                else if(bet==0)
                {
                    MessageBox.Show("Ошибка: ставка не должна быть нулевой");
                    return;
                }
                else
                {
                    GC.Gamers[combGamer.SelectedIndex].putBet(combGamer.SelectedIndex, combMizgir.SelectedIndex, bet);

                    if (GC.Gamers[0].HasBet && GC.Gamers[1].HasBet && GC.Gamers[2].HasBet)
                        btStart.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Ошибка: пустая величина ставки");
            }
        }

        public CritValues Max(List<double> Ratios)
        {
            CritValues max;
            max.CritValue = 0;
            max.CritVars = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                if(Ratios[i] > max.CritValue)
                {
                    max.CritValue = Ratios[i];
                    max.CritVars.Clear();
                    max.CritVars.Add(i);
                }
                else if (Ratios[i] == max.CritValue)
                {
                    max.CritVars.Add(i);
                }
            }

            return max;
        }


        public void ResultsPrint(string str)
        {
            rtbResults.Text = str + "\n" + rtbResults.Text;
        }


        private void btStart_Click(object sender, EventArgs e)
        {
            combGamer.Enabled = false;
            combMizgir.Enabled = false;
            tbBetSize.Enabled = false;
            btPut.Enabled = false;
            btStart.Enabled = false;

            GC.Start();

            btStart.Enabled = false;
        }
    }
}
