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
    struct CritValues
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


        bool IsRunning = false;
        List<Gamer> Gamers = new List<Gamer>();//список игроков
        List<Mizgir> Mizgirs = new List<Mizgir>();//список участников гонок
        int betsCount = 0;


        /// <summary>
        /// Функция, выполняемая при загрузке формы
        /// </summary>
        private void Form_Load(object sender, EventArgs e)
        {
            //цикл создания игроков
            for(int i=0; i<3; i++)
            {
                Gamer gamer = new Gamer(1 + i, "Участник"+(1+ i), 100);
                Gamers.Add(gamer);
            }

            //создание гонщиков
            Mizgir mizgir;
            mizgir = new Mizgir(1, picbMizgir1);
            Mizgirs.Add(mizgir);
            mizgir = new Mizgir(2, picbMizgir2);
            Mizgirs.Add(mizgir);
            mizgir = new Mizgir(3, picbMizgir3);
            Mizgirs.Add(mizgir);
            mizgir = new Mizgir(4, picbMizgir4);
            Mizgirs.Add(mizgir);
            for (int i = 0; i < 4; i++)
            {
                Mizgirs[i].move = Move;
                Mizgirs[i].toStart = ToStart;
            }

            //combGamer.SelectedIndex = 0;
            lbCash.Text = "";
            combMizgir.SelectedIndex = 0;
            btStart.Enabled = false;
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
        }

        public void ToStart(Mizgir mizgir)
        {
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

        public void Move(Mizgir mizgir, int step)
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
            lbCash.Text = Gamers[combGamer.SelectedIndex].Cash.ToString();

            if (Gamers[combGamer.SelectedIndex].HasBet)
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

                if(bet > Gamers[combGamer.SelectedIndex].Cash)
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
                    Bet Bet = new Bet(Mizgirs[combMizgir.SelectedIndex], bet);
                    Gamers[combGamer.SelectedIndex].Bet = Bet;
                    Gamers[combGamer.SelectedIndex].Cash -= bet;
                    lbCash.Text = Gamers[combGamer.SelectedIndex].Cash.ToString();
                    Gamers[combGamer.SelectedIndex].HasBet = true;
                    
                    betsCount++;

                    switch(Gamers[combGamer.SelectedIndex].Number)
                    {
                        case 1:
                            label1.Text = "Участник1 поставил " + bet + " на Мизгирь" + Mizgirs[combMizgir.SelectedIndex].Number;
                            break;
                        case 2:
                            label2.Text = "Участник2 поставил " + bet + " на Мизгирь" + Mizgirs[combMizgir.SelectedIndex].Number;
                            break;
                        case 3:
                            label3.Text = "Участник3 поставил " + bet + " на Мизгирь" + Mizgirs[combMizgir.SelectedIndex].Number;
                            break;
                    }

                    btPut.Enabled = false;

                    combMizgir.Enabled = false;
                    tbBetSize.Enabled = false;

                    if (betsCount == 3)
                        btStart.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Ошибка: пустая величина ставки");
            }
        }

        CritValues Max(List<double> Ratios)
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

        private void btStart_Click(object sender, EventArgs e)
        {
            Random r = new Random();

            List<int> Steps = new List<int>();
            List<int> Remains = new List<int>();
            List<double> Ratios = new List<double>();
            for (int i = 0; i < 4; i++)
            {
                Steps.Add(new Int32());
                Remains.Add(new Int32());
                Ratios.Add(new Double());
            }

            IsRunning = true;

            while(IsRunning)
            {
                for (int i = 0; i < 4; i++)
                {
                    Steps[i]= r.Next(5) + 1;
                    Remains[i]= 24 - Mizgirs[i].Position;
                    Ratios[i]= (double)Steps[i]/ Remains[i];
                }

                CritValues max = Max(Ratios);
                CritValues min;
                min.CritValue = 24;
                min.CritVars = new List<int>();

                while(max.CritValue > 0)
                {
                    foreach(int i in max.CritVars)
                    {
                        Mizgirs[i].move(Mizgirs[i], 1);
                        Remains[i]--;
                        Steps[i]--;
                        Ratios[i] = (double)Steps[i] / Remains[i];
                    }

                    System.Threading.Thread.Sleep(500);

                    foreach (int i in max.CritVars)
                    {
                        if (Remains[i] < min.CritValue)
                        {
                            min.CritValue = Remains[i];
                            min.CritVars.Clear();
                            min.CritVars.Add(i);
                        }
                        else if (Remains[i] == min.CritValue)
                        {
                            min.CritVars.Add(i);
                        }
                    }

                    if(min.CritValue==0)
                    {
                        if(min.CritVars.Count==1)
                        {
                            MessageBox.Show("Победитель: Мизгирь" + Mizgirs[min.CritVars[0]].Number);
                        }
                        else
                        {
                            string winners = "Победители: Мизгирь" + Mizgirs[min.CritVars[0]].Number;
                            for(int i=1; i< min.CritVars.Count; i++)
                            {
                                winners += ", Мизгирь" + Mizgirs[min.CritVars[i]].Number;
                            }
                            MessageBox.Show(winners);
                        }

                        foreach(int i in min.CritVars)
                        {
                            Mizgirs[i].IsWinner = true;
                        }

                        int sumBet=0, winBet=0;
                        List<Gamer> Winners = new List<Gamer>();

                        foreach (Gamer gamer in Gamers)
                        {
                            sumBet += gamer.Bet.Cost;
                            if (gamer.Bet.Mizgir.IsWinner)
                            {
                                winBet += gamer.Bet.Cost;
                                Winners.Add(gamer);
                            }
                        }

                        string results = "";
                        foreach (Gamer gamer in Gamers)
                        {
                            if (gamer.Bet.Mizgir.IsWinner)
                            {
                                int win= (int)(0.9 * gamer.Bet.Cost / winBet * sumBet);
                                gamer.Cash += win;
                                results += gamer.Name + " выиграл " + (win-gamer.Bet.Cost) + "\n";
                            }
                            else
                            {
                                results += gamer.Name + " проиграл " + gamer.Bet.Cost.ToString() + "\n";
                            }

                            gamer.Cash += 5;
                            gamer.Bet = null;
                            gamer.HasBet = false;
                        }
                        rtbResults.Text = results + "\n"+rtbResults.Text;


                        foreach (Mizgir mizgir in Mizgirs)
                        {
                            mizgir.IsWinner = false;
                            mizgir.Position = 0;
                            mizgir.toStart(mizgir);
                        }

                        label1.Text = "";
                        label2.Text = "";
                        label3.Text = "";

                        betsCount = 0;
                        btStart.Enabled = false;

                        IsRunning = false;
                        return;
                    }

                    max = Max(Ratios);
                }

            }
        }
    }
}
