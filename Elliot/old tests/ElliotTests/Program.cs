﻿using System;
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
            DateTime t0 = DateTime.UtcNow;
            RunStaticMethodsInClass(typeof(A_QuickTests));
            RunStaticMethodsInClass(typeof(B_SlowTests));
            RunStaticMethodsInClass(typeof(C_SlowestTests));

            DateTime t1 = DateTime.UtcNow;
            var ts = t1 - t0;
            Console.WriteLine("All tests passed in " + ts.TotalSeconds.ToString("0.0") + "s");
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
                    DateTime t0 = DateTime.UtcNow;
                    Console.Write(type.Name + "." + method.Name);
                    method.Invoke(null, null);
                    DateTime t1 = DateTime.UtcNow;
                    var ts = t1 - t0;
                    Console.WriteLine(" in " + ts.TotalSeconds.ToString("0.0") + "s");
                }
            }

        }
    }
}
