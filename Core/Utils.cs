using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tLaText.Core
{
    internal static class Utils
    {
        /// <summary>
        /// Checks if the Range overlaps with the other Range.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static bool Overlap(this Range r, Range range)
        {
            if (r.Start.Value <= range.Start.Value)
            {
                if (r.Start.Value >= range.Start.Value)
                {
                    return true;
                }
            }
            if (r.Start.Value > range.Start.Value)
            {
                if (r.Start.Value <= range.End.Value)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Merges the Range with the other Range.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="range"></param>
        public static Range MergeWith(this Range r, Range range)
        {
            return new Range(Math.Min(r.Start.Value, range.Start.Value), Math.Max(r.End.Value, range.End.Value));
        }
    }
}
