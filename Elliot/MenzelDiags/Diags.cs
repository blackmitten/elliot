using System;
using System.Collections.Generic;
using System.Text;

namespace Blackmitten.Menzel
{
    public class CrossPlatform
    {
        public enum PlatformType
        {
            Windows,
            Linux
        }

        static PlatformType? _platformType;
        static object _lock = new object();

        public static bool IsWindows => Platform == PlatformType.Windows;

        public static PlatformType Platform
        {
            get
            {
                lock (_lock)
                {
                    if (!_platformType.HasValue)
                    {
                        int p = (int)Environment.OSVersion.Platform;
                        if (p == 4 || p == 6 || p == 128)
                        {
                            _platformType = PlatformType.Linux;
                        }
                        else
                        {
                            _platformType = PlatformType.Windows;
                        }
                    }
                }
                return _platformType.Value;
            }
        }

    }
}
