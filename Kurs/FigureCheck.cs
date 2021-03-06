using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;

namespace Kurs
{
    enum Color { White, Black };//

    class FigureCheck
    {
        CheckField Pole;

        public Color C; // цвет шашки

        public TCell Pos; // позиция шашки

        public bool Dama = false;

        Canvas g;

        public Ellipse[] O;
        //создает объект шашка на заданной позиции
        public FigureCheck(Canvas g, Color C, TCell Pos)
        {
            this.g = g;
            this.C = C;
            this.Pos = Pos;
            O = new Ellipse[8]; //поле 8*8
            O[0] = null;
            O[1] = null;
        }


        public void SetPos(TCell Pos)
        {
            double L = g.Width; //сохранить размер игрового поля
            double L8 = L / 8.0;
            double kappa = 0.8;

            this.Pos.Set(Pos);

            if (((Pos.n == 1) && (C == Color.Black)) || ((Pos.n == 8) && (C == Color.White)))
            {
                Dama = true; //назначение шашки дамкой

                O[1] = new Ellipse();
                O[1].Stroke = Brushes.Red;
                O[1].StrokeThickness = 3;
                O[1].Width = kappa * L8 / 2;
                O[1].Height = kappa * L8 / 2;
                g.Children.Add(O[1]);
            }

            O[0].Margin = new Thickness(Pos.i * L8 + L8 * (1 - kappa) / 2, Pos.j * L8 + L8 * (1 - kappa) / 2, 0, 0);

            if (O[1] != null)
            {
                O[1].Margin = new Thickness(Pos.i * L8 + (L8 - O[1].Width) / 2, Pos.j * L8 + (L8 - O[1].Height) / 2, 0, 0);
            }
        }

        public void Delete()
        {
            g.Children.Remove(O[0]);
            g.Children.Remove(O[1]);
        }

        void AddRun(CheckRun Runs, int dir)
        {
            TCell Near;
            FigureCheck Who;
            FigureCheck W2;
            Move R2;

            Near = Pos.Near(dir);
            if (Pole.Who(Near, out Who))
            {
                if (Who == null)
                {
                    Runs.Add(new Move(this, Near));
                }
                else
                {
                    if (Who.C != C)
                    {
                        Near = Who.Pos.Near(dir);
                        if (Pole.Who(Near, out W2))
                        {
                            if (W2 == null)
                            {
                                R2 = new Move(this, Near);
                                R2.Killed.Add(Who);
                                Runs.Add(R2);
                            }
                        }
                    }
                }
            }

        }

        void AddRunDama(CheckRun Runs, int dir)
        {
            TCell[] Nears = Pos.NearsDamka(dir, Pole);

            for (int i = 1; i < Nears.Count(); i++)
            {
                Runs.Add(new Move(this, Nears[i]));
            }
        }

        void AddRunKillDama(TCell StartPos, int dir, TNode Parent)
        {
            ArrayList arr = new ArrayList();

            TCell Near;

            if (StartPos == null)
            {
                Near = Pos.NearDamka(dir, Pole);
            }
            else
            {
                Near = StartPos.NearDamka(dir, Pole);
            }

            FigureCheck Who;

            Pole.Who(Near, out Who);

            if (Who == null)
            {
                return;
            }

            if (Who.C == C)
            {
                return;
            }

            Near = Near.Near(dir);

            FigureCheck W2;

            if (Pole.Who(Near, out W2))
            {
                if (W2 == null)
                {
                    TNode Cur = new TNode(Parent, Near, Who, dir);

                    if (Tree == null)
                    {
                        Tree = new Tree(Cur, Pole);
                    }
                    Tree.Count++;

                    int Count = Tree.Count;

                    TCell[] Nears;

                    for (int i = 1; i <= 4; i++)
                    {

                        if (((dir == 1) && (i == 4)) || ((dir == 2) && (i == 3)) ||
                            ((dir == 3) && (i == 2)) || ((dir == 4) && (i == 1)))
                        {
                            continue;
                        }

                        Nears = Near.NearsDamka(dir, Pole);

                        for (int k = 0; k < Nears.Count(); k++)
                        {
                            AddRunKillDama(Nears[k], i, Cur);
                        }

                    }

                    if (Count == Tree.Count)
                    {
                        Tree.Lists.Add(Cur);
                    }
                }
            }
        }

        void AddRunKill(TCell StartPos, int dir, TNode Parent)
        {
            TCell Near;
            FigureCheck Who;
            FigureCheck W2;

            if (StartPos == null)
            {
                Near = Pos.Near(dir);
            }
            else
            {
                Near = StartPos.Near(dir);
            }


            if (Pole.Who(Near, out Who))
            {
                if (Who == null)
                {
                }
                else
                {
                    if (Who.C != C)
                    {
                        Near = Who.Pos.Near(dir);

                        if (Pole.Who(Near, out W2))
                        {
                            if (W2 == null)
                            {
                                TNode Cur = new TNode(Parent, Near, Who, dir);

                                if (Tree == null)
                                {
                                    Tree = new Tree(Cur, Pole);
                                }
                                Tree.Count++;

                                int Count = Tree.Count;

                                for (int i = 1; i <= 4; i++)
                                {

                                    if (((dir == 1) && (i == 4)) || ((dir == 2) && (i == 3)) ||
                                        ((dir == 3) && (i == 2)) || ((dir == 4) && (i == 1)))
                                    {
                                        continue;
                                    }

                                    AddRunKill(Near, i, Cur);
                                }

                                if (Count == Tree.Count)
                                {
                                    Tree.Lists.Add(Cur);
                                }
                            }
                        }
                    }
                }
            }

        }

        public CheckRun GetRuns(CheckField Pole)
        {
            this.Pole = Pole;

            CheckRun Res = new CheckRun();

            if (Dama)
            {
                AddRunDama(Res, 1);

                AddRunDama(Res, 2);

                AddRunDama(Res, 3);

                AddRunDama(Res, 4);
            }
            else
            {
                if (C == Color.White)
                {
                    AddRun(Res, 1);

                    AddRun(Res, 2);
                }
                else
                {
                    AddRun(Res, 3);

                    AddRun(Res, 4);
                }
            }


            return Res;
        }

        Tree Tree;

        public CheckRun GetRunsKill(CheckField Pole)
        {
            this.Pole = Pole;

            CheckRun Res = new CheckRun();

            if (Dama)
            {
                Tree = null;
                AddRunKillDama(null, 1, null);
                if (Tree != null)
                {
                    Tree.AddRuns(this, ref Res);
                    Tree = null;
                }


                AddRunKillDama(null, 2, null);
                if (Tree != null)
                {
                    Tree.AddRuns(this, ref Res);
                    Tree = null;
                }

                AddRunKillDama(null, 3, null);
                if (Tree != null)
                {
                    Tree.AddRuns(this, ref Res);
                    Tree = null;
                }

                AddRunKillDama(null, 4, null);
                if (Tree != null)
                {
                    Tree.AddRuns(this, ref Res);
                    Tree = null;
                }

            }
            else
            {
                Tree = null;
                AddRunKill(null, 1, null);
                if (Tree != null)
                {
                    Tree.AddRuns(this, ref Res);
                    Tree = null;
                }


                AddRunKill(null, 2, null);
                if (Tree != null)
                {
                    Tree.AddRuns(this, ref Res);
                    Tree = null;
                }

                AddRunKill(null, 3, null);
                if (Tree != null)
                {
                    Tree.AddRuns(this, ref Res);
                    Tree = null;
                }

                AddRunKill(null, 4, null);
                if (Tree != null)
                {
                    Tree.AddRuns(this, ref Res);
                    Tree = null;
                }
            }

            return Res;
        }


        public bool Run(CheckField Pole, Move R)
        {
            CheckRun RunsKill = GetRunsKill(Pole);

            if (RunsKill.Count > 0)
            {
                Move Run;
                if (R == null)
                {
                    Run = RunsKill.GetR();
                }
                else
                {
                    Run = R;
                }

                SetPos(Run.PosTo);

                for (int i = 0; i < Run.Killed.Count; i++)
                {
                    FigureCheck K = (FigureCheck)Run.Killed[i];
                    g.Children.Remove(K.O[0]);
                    g.Children.Remove(K.O[1]);
                    if (K.C == Color.White)
                    {
                        Pole.CW.Remove(K);
                    }
                    else
                    {
                        Pole.CB.Remove(K);
                    }
                }

                return true;
            }

            CheckRun Runs = GetRuns(Pole);

            if (Runs.Count > 0)
            {
                Move Run;
                if (R == null)
                {
                    Run = Runs.GetR();
                }
                else
                {
                    Run = R;
                }

                SetPos(Run.PosTo);
                return true;
            }

            return false;
        }

    }

    class Checks
    {
        ArrayList arr;

        public Checks(Color C, Canvas g)
        {
            arr = new ArrayList(12);

            if (C == Color.White)
            {
                arr.Add(new FigureCheck(g, C, new TCell(TNote.A, 1)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.A, 3)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.B, 2)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.C, 1)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.C, 3)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.D, 2)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.E, 1)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.E, 3)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.F, 2)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.G, 1)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.G, 3)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.H, 2)));
            }
            else
            {
                arr.Add(new FigureCheck(g, C, new TCell(TNote.A, 7)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.B, 6)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.B, 8)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.C, 7)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.D, 6)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.D, 8)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.E, 7)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.F, 6)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.F, 8)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.G, 7)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.H, 6)));
                arr.Add(new FigureCheck(g, C, new TCell(TNote.H, 8)));
            }

        }

        public FigureCheck this[int n]
        {
            get
            {
                return (FigureCheck)arr[n];
            }
        }

        public int Count
        {
            get
            {
                return arr.Count;
            }
        }

        public void RemoveAt(int n)
        {
            arr.RemoveAt(n);
        }

        public void Remove(FigureCheck Check)
        {
            Check.Delete();

            arr.Remove(Check);
        }

        public void Move(int n, TCell Pos)
        {
            FigureCheck Check = this[n];
            Check.SetPos(Pos);
        }
    }
}
