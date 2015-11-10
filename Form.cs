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


        /// <summary>
        /// Функция, выполняемая при загрузке формы
        /// </summary>
        private void Form_Load(object sender, EventArgs e)
        {
            GC = new GameController(this);
            GC.Initialize();
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

            //сосданние массива строк результатов гонки
            GC.ResultStrings = new string[GC.Gamers.Count];
        }



        public void FormInitialize()
        {
            lbCash.Text = "";
            combMizgir.SelectedIndex = 0;
            btStart.Enabled = false;
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
        }


        public void ToStart()
        {
            GC.betsCount = 0;
            GC.sumBet = 0;
            GC.winBet = 0;
            for(int i=0; i<GC.Gamers.Count; i++)
            {
                GC.ResultStrings[i] = "";
            }

            btStart.Enabled = false;

            foreach (Bug mizgir in GC.Mizgirs)
            {
                mizgir.IsWinner = false;
                mizgir.Position = 0;
                mizgir.Bets.Clear();

                switch (mizgir.Number)
                {
                    case 1:
                        mizgirRoad1.Controls.Add(mizgir.Picture, 0, 0);
                        mizgirRoad1.Refresh();
                        break;
                    case 2:
                        mizgirRoad2.Controls.Add(mizgir.Picture, 0, 0);
                        mizgirRoad2.Refresh();
                        break;
                    case 3:
                        mizgirRoad3.Controls.Add(mizgir.Picture, 0, 0);
                        mizgirRoad3.Refresh();
                        break;
                    case 4:
                        mizgirRoad4.Controls.Add(mizgir.Picture, 0, 0);
                        mizgirRoad4.Refresh();
                        break;

                }
            }
        }


        public void Move(Bug mizgir, int step)
        {
            mizgir.Position += step;
            switch(mizgir.Number)
            {
                case 1:
                    mizgirRoad1.Controls.Add(mizgir.Picture, mizgir.Position, 0);
                    mizgirRoad1.Refresh();
                    break;
                case 2:
                    mizgirRoad2.Controls.Add(mizgir.Picture, mizgir.Position, 0);
                    mizgirRoad2.Refresh();
                    break;
                case 3:
                    mizgirRoad3.Controls.Add(mizgir.Picture, mizgir.Position, 0);
                    mizgirRoad3.Refresh();
                    break;
                case 4:
                    mizgirRoad4.Controls.Add(mizgir.Picture, mizgir.Position, 0);
                    mizgirRoad4.Refresh();
                    break;
            }
        }

        private void combGamer_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbCash.Text = GC.Gamers[combGamer.SelectedIndex].Cash.ToString();

            if (GC.Gamers[combGamer.SelectedIndex].HasBet)
            {
                combMizgir.Enabled = false;
                tbBetSize.Enabled = false;
                btPut.Enabled = false;
            }
            else
            {
                combMizgir.Enabled = true;
                tbBetSize.Enabled = true;
                btPut.Enabled = true;
            }
        }


        public void PutBet(int gamerNum, int mizgirNum, int bet)
        {
            Bet Bet = new Bet(GC.Gamers[gamerNum], bet);

            GC.Mizgirs[mizgirNum].Bets.Add(Bet);

            GC.Gamers[gamerNum].Cash -= bet;
            lbCash.Text = GC.Gamers[gamerNum].Cash.ToString();
            GC.Gamers[gamerNum].HasBet = true;

            switch (GC.Gamers[gamerNum].Number)
            {
                case 1:
                    label1.Text = "Участник1 поставил " + bet + " на Мизгирь" + (mizgirNum+1);
                    break;
                case 2:
                    label2.Text = "Участник2 поставил " + bet + " на Мизгирь" + (mizgirNum + 1);
                    break;
                case 3:
                    label3.Text = "Участник3 поставил " + bet + " на Мизгирь" + (mizgirNum + 1);
                    break;
            }


            GC.betsCount++;
            GC.sumBet += bet;
            

            if (GC.betsCount == GC.Gamers.Count)
                btStart.Enabled = true;
        }


        public void GetBet(int gamerInd, int bet)
        {
            int win = (int)((double)bet / GC.winBet * GC.sumBet);
            GC.Gamers[gamerInd].Cash += win+5;
            GC.Gamers[gamerInd].HasBet = false;
            GC.ResultStrings[gamerInd] = GC.Gamers[gamerInd].Name + " выиграл " + (win - bet) + "\n";
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

                    

                    btPut.Enabled = false;

                    combMizgir.Enabled = false;
                    tbBetSize.Enabled = false;

                    if (GC.betsCount == 3)
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
            GC.StartRun();


            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            btStart.Enabled = false;
        }
    }
}
