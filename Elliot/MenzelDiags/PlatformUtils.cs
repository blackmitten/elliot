using System;

namespace Blackmitten.Menzel
{
    public class PlatformUtils
    {
        public enum Platform
        {
            Windows,
            Linux
        }

        public static Platform GetPlatform()
        {
            int p = (int)Environment.OSVersion.Platform;
            if ( p == 4 || p == 6 || p == 128 )
            {
                return Platform.Linux;
            }
            else
            {
                return Platform.Windows;
            }
        }
    }
}
