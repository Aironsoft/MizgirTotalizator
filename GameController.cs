using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MizgirTotalizator
{
    public class GameController
    {
        public bool IsRunning;
        public List<Gambler> Gamers;// игроков
        public List<Bug> Mizgirs;//список участников гонок
        public List<Bet> Bets;//список ставок
        public int betsCount;
        public int sumBet;
        public int winBet;
        private Form Form;
        public string[] ResultStrings;


        public GameController(Form form)
        {
            IsRunning = false;
            Gamers = new List<Gambler>();//список игроков
            Mizgirs = new List<Bug>();//список участников гонок
            betsCount = 0;
            sumBet = 0;
            winBet = 0;
            Form = form;
        }

        public void Initialize()
        {
            FormInitialize();
            
            Form.GCDataInitialize();

            ToStart();
        }


        public void FormInitialize()
        {
            Form.FormInitialize();
        }


        public void ToStart()
        {
            Form.ToStart();
        }


        public void StartRun()
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

            while (IsRunning)
            {
                for (int i = 0; i < 4; i++)
                {
                    Steps[i] = r.Next(5) + 1;
                    Remains[i] = 24 - Mizgirs[i].Position;
                    Ratios[i] = (double)Steps[i] / Remains[i];
                }

                CritValues max = Form.Max(Ratios);
                CritValues min;
                min.CritValue = 24;
                min.CritVars = new List<int>();

                while (max.CritValue > 0)
                {
                    foreach (int i in max.CritVars)
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

                    if (min.CritValue == 0)
                    {
                        if (min.CritVars.Count == 1)
                        {
                            MessageBox.Show("Победитель: Мизгирь" + Mizgirs[min.CritVars[0]].Number);
                        }
                        else
                        {
                            string winners = "Победители: Мизгирь" + Mizgirs[min.CritVars[0]].Number;
                            for (int i = 1; i < min.CritVars.Count; i++)
                            {
                                winners += ", Мизгирь" + Mizgirs[min.CritVars[i]].Number;
                            }
                            MessageBox.Show(winners);
                        }

                        foreach (int i in min.CritVars)
                        {
                            Mizgirs[i].IsWinner = true;
                        }


                        foreach(Bug mizgir in Mizgirs)
                        {
                            if(mizgir.IsWinner)
                            {
                                if(mizgir.Bets.Count!=0)//если на этого паука кто-то поставил
                                {
                                    foreach (Bet bet in mizgir.Bets)
                                    {
                                        winBet += bet.Cost;
                                    }

                                    foreach (Bet bet in mizgir.Bets)
                                    {
                                        bet.Gamer.getBet(bet.Gamer.Number-1, bet.Cost);
                                    }
                                }
                            }
                            else
                            {
                                if (mizgir.Bets.Count != 0)//если на этого паука кто-то поставил
                                {
                                    foreach (Bet bet in mizgir.Bets)
                                    {
                                        ResultStrings[bet.Gamer.Number-1]= bet.Gamer.Name + " проиграл " + bet.Cost + "\n";
                                        bet.Gamer.HasBet = false;
                                        bet.Gamer.Cash += 5;
                                    }
                                }
                            }
                        }


                        string results = "";
                        for (int n=0; n<ResultStrings.Length; n++)
                        {
                            results += ResultStrings[n];
                        }
                        Form.ResultsPrint(results);


                        ToStart();

                        betsCount = 0;
                        sumBet = 0;

                        IsRunning = false;
                        return;
                    }

                    max = Form.Max(Ratios);
                }

            }
        }

    }
}
