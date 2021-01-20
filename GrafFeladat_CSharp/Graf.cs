using System;
using System.Collections.Generic;

namespace GrafFeladat_CSharp
{
    /// <summary>
    /// Irányítatlan, egyszeres gráf.
    /// </summary>
    class Graf
    {
        int csucsokSzama;
        /// <summary>
        /// A gráf élei.
        /// Ha a lista tartalmaz egy(A, B) élt, akkor tartalmaznia kell
        /// a(B, A) vissza irányú élt is.
        /// </summary>
        readonly List<El> elek = new List<El>();
        /// <summary>
        /// A gráf csúcsai.
        /// A gráf létrehozása után új csúcsot nem lehet felvenni.
        /// </summary>
        readonly List<Csucs> csucsok = new List<Csucs>();

        /// <summary>
        /// Létehoz egy úgy, N pontú gráfot, élek nélkül.
        /// </summary>
        /// <param name="csucsok">A gráf csúcsainak száma</param>
        public Graf(int csucsok)
        {
            this.csucsokSzama = csucsok;

            // Minden csúcsnak hozzunk létre egy új objektumot
            for (int i = 0; i < csucsok; i++)
            {
                this.csucsok.Add(new Csucs(i));
            }
        }

        /// <summary>
        /// Hozzáad egy új élt a gráfhoz.
        /// Mindkét csúcsnak érvényesnek kell lennie:
        /// 0 &lt;= cs &lt; csúcsok száma.
        /// </summary>
        /// <param name="cs1">Az él egyik pontja</param>
        /// <param name="cs2">Az él másik pontja</param>
        public void Hozzaad(int cs1, int cs2)
        {
            if (cs1 < 0 || cs1 >= csucsokSzama ||
                cs2 < 0 || cs2 >= csucsokSzama)
            {
                throw new ArgumentOutOfRangeException("Hibas csucs index");
            }

            // Ha már szerepel az él, akkor nem kell felvenni
            foreach (var el in elek)
            {
                if (el.Csucs1 == cs1 && el.Csucs2 == cs2)
                {
                    return;
                }
            }

            elek.Add(new El(cs1, cs2));
            elek.Add(new El(cs2, cs1));
        }

        public void torles(int csucs1, int csucs2)
        {
            List<Csucs> list1 = new List<Csucs>();
            foreach (var item in csucsok)
            {
                if (item.getId() == csucs1 || item.getId() == csucs2)
                {
                    list1.Add(item);
                    List<El> list2 = new List<El>();
                    foreach (var item2 in elek)
                    {
                        if (item2.Csucs1 == csucs1 || item2.Csucs2 == csucs1)
                        {
                            list2.Add(item2);
                        }
                        if (item2.Csucs1 == csucs2 || item2.Csucs2 == csucs2)
                        {
                            list2.Add(item2);
                        }
                    }
                    foreach (var item2 in list2)
                    {
                        elek.Remove(item2);
                    }
                }
            }
            foreach (var item in list1)
            {
                csucsok.Remove(item);
            }
        }

        public HashSet<int> szelessegBejar(int szb)
        {
            HashSet<int> order = new HashSet<int>();
            Queue<int> que = new Queue<int>();
            que.Enqueue(szb);
            order.Add(szb);

            do
            {
                Queue<int> bf = new Queue<int>();
                bf.Enqueue(que.Dequeue());

                foreach (var item in elek)
                {
                    int i = 0;
                    if (item.Csucs1.Equals(bf.Peek()) && !order.Contains(item.Csucs2))
                    {
                        que.Enqueue(item.Csucs2);
                        order.Add(item.Csucs2);
                        i++;
                    }
                }
            } while (que.Count != 0);

            return order;
        }


        public HashSet<int> melysegBejaras(int mb)
        {
            Stack<int> stack = new Stack<int>();
            HashSet<int> order = new HashSet<int>();
            stack.Push(mb);
            order.Add(mb);

            do
            {
                Stack<int> bf = new Stack<int>();
                bf.Push(stack.Pop());

                foreach (var item in elek)
                {
                    if (item.Csucs1.Equals(bf.Peek()) && !order.Contains(item.Csucs2))
                    {
                        stack.Push(item.Csucs2);
                        order.Add(item.Csucs2);
                    }
                }
            } while (stack.Count != 0);
            return order;
        }

        public Graf feszitofa()
        {
            Graf fa = new Graf(this.csucsokSzama);
            Queue<int> que = new Queue<int>();
            HashSet<int> order = new HashSet<int>();
           
            que.Enqueue(0);
            order.Add(0);
 
            do
            {
                Queue<int> bf = new Queue<int>();
                bf.Enqueue(que.Dequeue());

                foreach (var item in elek)
                {
                    if (item.Csucs1.Equals(bf.Peek()))
                    {
                        if (order.Contains(item.Csucs2))
                        {
                            order.Add(item.Csucs2);
                            que.Enqueue(item.Csucs2);
                            fa.Hozzaad(item.Csucs1, item.Csucs2);
                        }
                    }
                }
            } while (que.Count != 0);

            return fa;
        }

        public bool bejarhatoE(int bj)
        {

            Stack<int> stack = new Stack<int>();
            HashSet<int> order = new HashSet<int>();
            stack.Push(bj);
            order.Add(bj);

            do
            {
                Stack<int> bf = new Stack<int>();
                bf.Push(stack.Pop());

                foreach (var item in elek)
                {
                    if (item.Csucs1.Equals(bf.Peek()) && !order.Contains(item.Csucs2))
                    {
                        stack.Push(item.Csucs2);
                        order.Add(item.Csucs2);
                    }
                }
            } while (stack.Count != 0);

            if (order.Count == csucsok.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            string str = "Csucsok:\n";
            foreach (var cs in csucsok)
            {

                str += cs + "\n";
            }
            str += "Elek:\n";
            foreach (var el in elek)
            {
                str += el + "\n";
            }
            return str;
        }
    }
}