﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MMORL.Client.Extensions
{
    public static class NumberExtensions
    {
        public enum SizeUnits
        {
            Byte, KB, MB, GB, TB, PB, EB, ZB, YB
        }

        public static string ToSize(this Int64 value, SizeUnits unit)
        {
            return (value / (double)Math.Pow(1024, (Int64)unit)).ToString("0.00");
        }

        public static string ToSize(this Int32 value, SizeUnits unit)
        {
            return (value / (double)Math.Pow(1024, (Int32)unit)).ToString("0.00");
        }
    }
}
