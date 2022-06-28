using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs
{
    class Tree
    {
        TNode Root;

        CheckField Pole;

        public ArrayList Lists;

        public int Count;

        public Tree(TNode Root, CheckField Pole)
        {
            this.Root = Root;
            this.Pole = Pole;

            Lists = new ArrayList(12);

            Count = 0;
        }

        public ArrayList Up(TNode List)
        {
            ArrayList Res = new ArrayList(6);

            TNode Cur = List;

            Res.Add(Cur);

            while (Cur.Parent != null)
            {
                Cur = Cur.Parent;
                Res.Add(Cur);
            }

            return Res;
        }

        public void AddRuns(FigureCheck Check, ref CheckRun Runs)
        {
            for (int i = 0; i < Lists.Count; i++)
            {
                TNode Node = (TNode)Lists[i];

                Move Run = new Move(Check, Node.Pos);

                ArrayList arr = Up(Node);

                for (int j = 0; j < arr.Count; j++)
                {
                    Run.Killed.Add(((TNode)arr[j]).Killed);
                }

                Runs.Add(Run);

                TCell[] Addition = Node.Pos.NearsDamka(Node.dir, Pole);

                for (int k = 1; k < Addition.Count(); k++)
                {
                    Move ARun = new Move(Check, Addition[k]);

                    for (int j = 0; j < Run.Killed.Count; j++)
                    {
                        ARun.Killed.Add(Run.Killed[j]);
                    }

                    Runs.Add(ARun);
                }
            }

        }
    }

    class TNode
    {
        public TNode Parent;
        public TCell Pos;
        public FigureCheck Killed;
        public int dir;

        public TNode(TNode Parent, TCell Pos, FigureCheck Killed, int dir)
        {
            this.Parent = Parent;
            this.Pos = Pos;
            this.Killed = Killed;
            this.dir = dir;
        }
    }
}