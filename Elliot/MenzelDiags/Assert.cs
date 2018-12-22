using System;
using System.Collections.Generic;
using System.Text;

namespace Blackmitten.Menzel
{
    public class Assert
    {
        public static void AreEqual(object o1, object o2)
        {
            if (!o1.Equals(o2))
            {
                throw new AssertionFailedException();
            }
        }

        public static void AreNotEqual(object o1, object o2)
        {
            if (o1.Equals(o2))
            {
                throw new AssertionFailedException();
            }
        }

        public static void AreNotSame(object o1, object o2)
        {
            if (ReferenceEquals(o1, o2))
            {
                throw new AssertionFailedException();
            }
        }

        public static void IsTrue(bool b)
        {
            if (!b)
            {
                throw new AssertionFailedException();
            }
        }


    }
}
