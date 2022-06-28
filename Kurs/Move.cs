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
    class Move
    {
        public FigureCheck Check;
        public TCell PosTo;
        public ArrayList Killed;

        public Move(FigureCheck Check, TCell PosTo)
        {
            this.Check = Check;
            this.PosTo = PosTo;
            Killed = new ArrayList();
        }
    }

    class CheckRun
    {
        ArrayList arr;
        Random rnd;

        public CheckRun()
        {
            arr = new ArrayList();
            rnd = new Random();
        }

        public Move GetR()
        {
            if (Count < 1)
            {
                return null;
            }
            else
            {
                return this[rnd.Next(Count)];
            }
        }

        public void Add(Move Run)
        {
            arr.Add(Run);
        }

        public int Count
        {
            get
            {
                return arr.Count;
            }
        }

        public Move this[int i]
        {
            get
            {
                return (Move)arr[i];
            }
            set
            {
                arr[i] = value;
            }
        }
    }
}