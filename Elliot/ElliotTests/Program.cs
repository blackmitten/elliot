using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Blackmitten.Menzel;

namespace ElliotTests
{
    class Program
    {

        static void Main(string[] args)
        {
            DateTime t0 = DateTime.UtcNow;

            RunTests();

            DateTime t1 = DateTime.UtcNow;
            var ts = t1 - t0;

            Console.WriteLine("All tests passed in " + ts.TotalSeconds.ToString("0.0") + "s");
            Console.ReadKey();
        }

        private static void RunTests()
        {
            /*
            var methodInfo = SymbolExtensions.GetMethodInfo(() => A_QuickTests.TestGetPieceOnSquare());
            RunMethod(methodInfo);
            return;
            */

            DateTime t0 = DateTime.UtcNow;
            RunStaticMethodsInClass(typeof(A_QuickTests));
            RunStaticMethodsInClass(typeof(B_SlowTests));
            RunStaticMethodsInClass(typeof(C_SlowestTests));

            DateTime t1 = DateTime.UtcNow;
            var ts = t1 - t0;
        }

        private static void RunMethod(MethodInfo method)
        {
            DateTime t0 = DateTime.UtcNow;
            Console.WriteLine(method.DeclaringType.Name + "." + method.Name);
            method.Invoke(null, null);
            DateTime t1 = DateTime.UtcNow;
            var ts = t1 - t0;
            Console.WriteLine(" in " + ts.TotalSeconds.ToString("0.0") + "s");
        }

        private static void RunStaticMethodsInClass(Type type)
        {
            MethodInfo[] methodInfos = type.GetMethods();
            foreach (var method in methodInfos)
            {
                if (method.IsStatic)
                {
                    RunMethod(method);
                }
            }

        }
    }
}
