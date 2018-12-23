using System;
using System.Collections.Generic;
using System.Text;

namespace Blackmitten.Menzel
{
    public class Diags
    {
        public static void Assert(bool assertion)
        {
            if (!assertion)
            {
                throw new AssertionFailedException();
            }
        }

        public enum PlatformType
        {
            Windows,
            Linux
        }

        public static PlatformType Platform
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                if (p == 4 || p == 6 || p == 128)
                {
                    return PlatformType.Linux;
                }
                else
                {
                    return PlatformType.Windows;
                }
            }
        }

    }
}
