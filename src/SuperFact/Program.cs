using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMath;

namespace SuperFact
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var i = 0; i < 100; i++)
            {
                Console.WriteLine(Decimal3.Parse(i));
            }
            /*
            var number = 533L;
            Console.WriteLine($"Gonna factor {number}");

            var factor = SuperMath.DecimalToArbitrarySystem(number, 3);
            Console.WriteLine($"Starting with {factor}");

            DoIt(factor);*/

            Console.ReadKey();
        }

        static void DoIt(string number)
        {
            /*
            var ret = string.Empty;
            var simpleNum = SuperMath.ArbitraryToDecimalSystem(number, 3);
            if (Factorable(number))
            {
                var tr = simpleNum/2.0M;
                if ((int) tr == tr)
                {
                    ret = SuperMath.DecimalToArbitrarySystem((long) tr, 3);
                }
                /*else if (tr > 3)
                {
                    Console.Write($"3. ");
                    tr -= 1.5M;
                    DoIt(SuperMath.DecimalToArbitrarySystem((long) tr, 3));
                    return;
                }
                else
                {
                    Console.WriteLine($"{tr*2}, 3 ({tr}, {number})");
                    var care = tr*2;
                    var wtf = care/3;
                    if ((long)wtf == 1)
                    {
                        Console.WriteLine($"{tr*2}#");
                        return;
                    }
                    DoIt(SuperMath.DecimalToArbitrarySystem((long) wtf, 3));
                    return;
                }

                if (tr == 1 || tr == 0)
                {
                    Console.WriteLine("2@");
                    return;
                }

                Console.WriteLine($"{tr}, 2");
            }
            else
            {
                Console.WriteLine($"er : {simpleNum}, {number}");
                DoIt(SuperMath.DecimalToArbitrarySystem(simpleNum / 5, 3));
                return;
            }

            DoIt(ret);*/
        }

        static bool Factorable(string me)
        {
            var last = me[me.Length - 1];
            if (last == '0' || last == '2')
            {
                return true;
            }

            return false;
        }
    }
}
