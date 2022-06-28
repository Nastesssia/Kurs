using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs
{
    class Rand
    {
        int[] A;
        int count;
        Random rnd;

        public Rand(int Max)
        {
            rnd = new Random();
            A = new int[Max];
            count = 0;
        }

        public void Clear()
        {
            count = 0;
        }

        public int Get
        {
            get
            {
                if (count < 1)
                {
                    return -1;
                }

                return A[rnd.Next(count)];  //выбираем рандомно одно из диапазона от 0 до count
            }
        }

        public void Add(int a)
        {
            if (count == A.Count())
            {
                return;
            }

            A[count] = a;
            count++;
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        public int this[int ind]
        {
            get
            {
                return A[ind];
            }
        }
    }
}