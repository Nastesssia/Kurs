using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Kurs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CheckField Pole;

        Rand Rand;

        public MainWindow()
        {
            InitializeComponent();

            Pole = new CheckField(CheckPlace);  //Создание поля и шашек

            Rand = new Rand(64);
        }

        private void cmClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        bool IsW = true;

        private void cmRun(object sender, RoutedEventArgs e)
        {
            if (IsW)
            {
                CheckRun(Pole.CW);
                Thread.Sleep(1000);
            }
            else
            {
                CheckRun(Pole.CB);
                Thread.Sleep(1000); //задержка для отслеживания
            }

            IsW = !IsW;
        }

        void CheckRun(Checks C)
        {
            CheckRun Runs;
            CheckRun RunsKill;

            Rand.Clear();

            for (int n = 0; n < C.Count; n++)
            {
                RunsKill = C[n].GetRunsKill(Pole);

                if (RunsKill.Count > 0)
                {
                    Rand.Add(n);
                }
            }

            if (Rand.Count > 0)
            {
                C[Rand.Get].Run(Pole, null);
                return;
            }

            Rand.Clear();

            for (int n = 0; n < C.Count; n++)
            {
                Runs = C[n].GetRuns(Pole);

                if (Runs.Count > 0)
                {
                    Rand.Add(n);
                }
            }

            if (Rand.Count > 0)
            {
                C[Rand.Get].Run(Pole, null);
            }
            else
            {
                IsGame = false;

                Color Cx;

                if (IsW)
                {
                    Cx = Color.White;
                }
                else
                {
                    Cx = Color.Black;
                }

                Pole.GameOver(Cx);

                Pole = new CheckField(CheckPlace);
            }

        }

        bool IsGame;

        DispatcherTimer Timer;

        private void cmAutoRun(object sender, RoutedEventArgs e)
        {
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(onTick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            IsGame = true;

            Timer.Start();
        }

        void onTick(object sender, EventArgs e)
        {
            if (IsGame)
            {
                cmRun(null, null);
            }
            else
            {
                Timer.Stop();
            }

        }

        private void cmPlay(object sender, RoutedEventArgs e)
        {

        }

        FigureCheck CheckFor = null;

        private void cmClick(object sender, MouseButtonEventArgs e)
        {
            Point XY = e.GetPosition(CheckPlace);


            TCell Z = Pole.GetCell(XY);

            if (CheckFor == null)
            {
                Pole.Who(Z, out CheckFor);

                if (CheckFor == null)
                {
                    return;
                }

                if (CheckFor.C != Color.White)
                {
                    CheckFor = null;
                    return;
                }
            }
            else
            {
                ArrayList arr = new ArrayList();

                Move Run = new Move(CheckFor, Z);

                CheckRun Runs;
                CheckRun RunsKill;

                for (int n = 0; n < Pole.CW.Count; n++)
                {
                    RunsKill = Pole.CW[n].GetRunsKill(Pole);

                    for (int k = 0; k < RunsKill.Count; k++)
                    {
                        arr.Add(RunsKill[k]);
                    }
                }
                if (arr.Count > 0)
                {
                    for (int k = 0; k < arr.Count; k++)
                    {
                        Move R = (Move)arr[k];

                        if ((Run.Check.Pos.Eq(R.Check.Pos)) && (Run.PosTo.Eq(R.PosTo)))
                        {
                            R.Check.Run(Pole, R);

                            IsW = !IsW;
                            cmRun(null, null);
                            CheckFor = null;
                            return;
                        }
                    }

                    CheckFor = null;
                    return;
                }
                else
                {
                    arr.Clear();

                    for (int n = 0; n < Pole.CW.Count; n++)
                    {
                        Runs = Pole.CW[n].GetRuns(Pole);

                        for (int k = 0; k < Runs.Count; k++)
                        {
                            arr.Add(Runs[k]);
                        }
                    }

                    if (arr.Count > 0)
                    {
                        for (int k = 0; k < arr.Count; k++)
                        {
                            Move R = (Move)arr[k];

                            if ((Run.Check.Pos.Eq(R.Check.Pos)) && (Run.PosTo.Eq(R.PosTo)))
                            {
                                R.Check.Run(Pole, R);

                                IsW = !IsW;
                                cmRun(null, null);
                                CheckFor = null;
                                return;
                            }
                        }

                        CheckFor = null;
                        return;
                    }
                    else
                    {
                        IsGame = false; //конец игры

                        Color Cx; //определение победителя

                        if (IsW)
                        {
                            Cx = Color.White;
                        }
                        else
                        {
                            Cx = Color.Black;
                        }

                        Pole.GameOver(Cx);

                        Pole = new CheckField(CheckPlace); //создание нового поля после игры
                    }
                }
            }
        }
    }
}