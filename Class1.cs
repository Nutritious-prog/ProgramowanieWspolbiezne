using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Class1
    {


        public int Fib(int x)
        {

            if(x == 0)
            {
                return 0;
            }

            else if(x == 1)
            {
                return 1;
            }

            else
            {
                return Fib(x - 2) + Fib(x - 1);
            }

        }



    }
}
