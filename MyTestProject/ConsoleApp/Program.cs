using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = { 3, 1, 2 };

            IOrderedEnumerable<int> aa = array.OrderBy(d => d);

            ArrayList arrayList = new ArrayList();

            for (int i = 0; i < 32; i++)
            {
                arrayList.Add(i);
                Console.WriteLine(arrayList.Capacity);
            }

            arrayList.Sort();

            Console.ReadLine();
        }
    }
}
