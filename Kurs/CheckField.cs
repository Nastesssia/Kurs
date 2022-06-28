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

    class CheckField
    {
        Canvas g;

        public Checks CW, CB;

        public CheckField(Canvas g)
        {
            this.g = g;

            DrawCell(); //поле

            CW = new Checks(Color.White, g);
            CB = new Checks(Color.Black, g);

            DrawCheckers();
        }

        public TCell GetCell(Point XY)
        {
            double L = g.Width; //сохранение размера игрового поля
            double L8 = L / 8.0;

            double x = XY.X;
            double y = XY.Y;

            if ((x < 0) || (x > L) || (y < 0) || (y > L))
            {

                return null;
            }

            int a = Convert.ToInt16(Math.Ceiling(x / L8));

            TNote Z = (TNote)(a - 1);

            a = Convert.ToInt16(Math.Ceiling(y / L8));
            int n = 9 - a;

            return new TCell(Z, n);
        }

        public void GameOver(Color C) //вывод победителя
        {
            string s = "";

            if (C == Color.White)
            {
                s = "Чёрные шашки";
            }
            else
            {
                s = "Белые шашки";
            }

            MessageBox.Show(s + " победили!");
        }

        void DrawCheckers()
        {
            for (int k = 0; k < CW.Count; k++)
            {
                CW[k].O[0] = DrawCheck(CW[k].Pos, Color.White);

            }

            for (int k = 0; k < CB.Count; k++)
            {
                CB[k].O[0] = DrawCheck(CB[k].Pos, Color.Black);
            }
        }

        Ellipse DrawCheck(TCell Pos, Color C)
        {
            double L = g.Width; // сохранение размера игрового поля
            double L8 = L / 8.0;
            double koaf = 0.8;  // диаметр шашки

            Ellipse O = new Ellipse();

            if (C == Color.White)
            {
                O.Fill = Brushes.White;
            }
            else
            {
                O.Fill = Brushes.Black;
            }
            O.Width = koaf * L8;
            O.Height = koaf * L8;
            O.Margin = new Thickness(Pos.i * L8 + L8 * (1 - koaf) / 2, Pos.j * L8 + L8 * (1 - koaf) / 2, 0, 0);
            g.Children.Add(O);
            return O;
        }

        // создание игрового поля
        void DrawCell()
        {
            double L = g.Width; // сохранение размера игрового поля
            double L8 = L / 8.0;      //фиксация ширины одной клетки

            bool White = false;

            for (int i = 0; i < 8; i++)
            {
                White = !White;

                for (int j = 0; j < 8; j++)
                {
                    Rectangle Cell = new Rectangle(); //создание основы игрового поля - прямоугольника

                    if (White)
                    {
                        Cell.Fill = Brushes.Bisque;  //создание светлых клеток на игровом поле
                        White = false;
                    }
                    else
                    {
                        Cell.Fill = Brushes.Brown; //создание тёмных клеток на игровом поле
                        White = true;
                    }

                    Cell.Width = L8;
                    Cell.Height = L8;
                    Cell.Margin = new Thickness(i * L8, j * L8, 0, 0);   //отступление от края доски
                    g.Children.Add(Cell);
                }
            }

            Rectangle R = new Rectangle();
            R.Width = L;
            R.Height = L;
            R.Stroke = Brushes.Black;
            R.Margin = new Thickness(0);
            g.Children.Add(R);

        }

        public bool Who(TCell Pos, out FigureCheck who)
        {
            who = null;

            if (Pos == null)
            {
                return false;
            }

            for (int k = 0; k < CW.Count; k++)
            {
                if (CW[k].Pos.Eq(Pos))
                {
                    who = CW[k];
                    return true;
                }
            }

            for (int k = 0; k < CB.Count; k++)
            {
                if (CB[k].Pos.Eq(Pos))
                {
                    who = CB[k];
                    return true;
                }
            }

            return true;
        }

    }

    enum TNote { A, B, C, D, E, F, G, H };

    class TCell
    {
        public int i, j; // координаты клетки на поле
        public TNote B; //буква
        public int n;

        public TCell(int i, int j)
        {
            this.i = i;
            this.j = j;

            n = i + 1;
            B = (TNote)j;
        }

        public TCell(TNote B, int n)
        {
            this.B = B;
            this.n = n;

            i = (int)B;

            j = 8 - n;
        }

        public void Set(TCell Pos)
        {
            this.i = Pos.i;
            this.j = Pos.j;
            this.B = Pos.B;
            this.n = Pos.n;
        }

        public bool Eq(TCell Pos)
        {
            if ((B == Pos.B) && (n == Pos.n))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public TCell[] NearsDamka(int dir, CheckField Pole)
        {
            ArrayList arr = new ArrayList(8);

            TNote B_ = B;
            int n_ = n;

            arr.Add(new TCell(B_, n));

            FigureCheck Who;

            if (dir == 1)
            {
                while ((B_ != TNote.A) && (n_ != 8))
                {
                    B_ = B_ - 1;
                    n_ = n_ + 1;

                    Pole.Who(new TCell(B_, n_), out Who);
                    if (Who == null)
                    {
                        arr.Add(new TCell(B_, n_));
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (dir == 2)
            {
                while ((B_ != TNote.H) && (n_ != 8))
                {
                    B_ = B_ + 1;
                    n_ = n_ + 1;
                    Pole.Who(new TCell(B_, n_), out Who);
                    if (Who == null)
                    {
                        arr.Add(new TCell(B_, n_));
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (dir == 3)
            {
                while ((B_ != TNote.A) && (n_ != 1))
                {
                    B_ = B_ - 1;
                    n_ = n_ - 1;
                    Pole.Who(new TCell(B_, n_), out Who);
                    if (Who == null)
                    {
                        arr.Add(new TCell(B_, n_));
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (dir == 4)
            {
                while ((B_ != TNote.H) && (n_ != 1))
                {
                    B_ = B_ + 1;
                    n_ = n_ - 1;
                    Pole.Who(new TCell(B_, n_), out Who);
                    if (Who == null)
                    {
                        arr.Add(new TCell(B_, n_));
                    }
                    else
                    {
                        break;
                    }
                }
            }

            TCell[] Res = null;

            if (arr.Count > 0)
            {
                Res = new TCell[arr.Count];

                for (int i = 0; i < arr.Count; i++)
                {
                    Res[i] = (TCell)arr[i];
                }
            }

            return Res;
        }

        public TCell NearDamka(int dir, CheckField Pole)
        {
            TNote B_ = B;
            int n_ = n;
            FigureCheck Who;

            if (dir == 1)
            {
                while ((B_ != TNote.A) && (n_ != 8))
                {
                    B_ = B_ - 1;
                    n_ = n_ + 1;
                    Pole.Who(new TCell(B_, n_), out Who);
                    if (Who != null)
                    {
                        return new TCell(B_, n_);
                    }
                }
            }

            if (dir == 2)
            {
                while ((B_ != TNote.H) && (n_ != 8))
                {
                    B_ = B_ + 1;
                    n_ = n_ + 1;
                    Pole.Who(new TCell(B_, n_), out Who);
                    if (Who != null)
                    {
                        return new TCell(B_, n_);
                    }
                }
            }

            if (dir == 3)
            {
                while ((B_ != TNote.A) && (n_ != 1))
                {
                    B_ = B_ - 1;
                    n_ = n_ - 1;
                    Pole.Who(new TCell(B_, n_), out Who);
                    if (Who != null)
                    {
                        return new TCell(B_, n_);
                    }
                }
            }

            if (dir == 4)
            {
                while ((B_ != TNote.H) && (n_ != 1))
                {
                    B_ = B_ + 1;
                    n_ = n_ - 1;
                    Pole.Who(new TCell(B_, n_), out Who);
                    if (Who != null)
                    {
                        return new TCell(B_, n_);
                    }
                }
            }

            return null;
        }

        public TCell Near(int dir)
        {
            if (dir == 1)
            {
                if ((B == TNote.A) || (n == 8))
                {
                    return null;
                }

                return new TCell((TNote)B - 1, n + 1);
            }

            if (dir == 2)
            {
                if ((B == TNote.H) || (n == 8))
                {
                    return null;
                }

                return new TCell((TNote)B + 1, n + 1);
            }

            if (dir == 3)
            {
                if ((B == TNote.A) || (n == 1))
                {
                    return null;
                }

                return new TCell((TNote)B - 1, n - 1);
            }

            if (dir == 4)
            {
                if ((B == TNote.H) || (n == 1))
                {
                    return null;
                }

                return new TCell((TNote)B + 1, n - 1);
            }

            return null;
        }

        public string ToString()
        {
            return B.ToString() + n.ToString();
        }

    }
}
