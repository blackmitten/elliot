using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ElliotTests
{
    class Program
    {
        static void Main(string[] args)
        {
            RunStaticMethodsInClass(typeof(A_QuickTests));
            RunStaticMethodsInClass(typeof(B_SlowTests));
            RunStaticMethodsInClass(typeof(C_SlowestTests));

            Console.WriteLine("All tests passed, well done dude");
            Console.ReadKey();
        }

        private static void RunStaticMethodsInClass(Type type)
        {
//            MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
            MethodInfo[] methodInfos = type.GetMethods();
            foreach (var method in methodInfos)
            {
                if (method.IsStatic)
                {
                    Console.WriteLine(method.Name);
                    method.Invoke(null, null);
                }
            }

        }
    }
}
