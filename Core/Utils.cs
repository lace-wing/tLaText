using Microsoft.Xna.Framework;
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
        /// Checks if an int is within a range.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool Within(this int i, int min, int max)
        {
            return i >= min && i <= max;
        }
        /// <summary>
        /// Checks if the Point overlaps with the other Point when used as ranges.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static bool Overlap(this Point d, Point domain)
        {
            if (d.X <= domain.X)
            {
                if (d.X >= domain.X)
                {
                    return true;
                }
            }
            if (d.X > domain.X)
            {
                if (d.X <= domain.Y)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Merges the Point with the other point when used as ranges.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="domain"></param>
        public static Point MergeWith(this Point d, Point domain)
        {
            return new Point(Math.Min(d.X, domain.X), Math.Max(d.Y, domain.Y));
        }
    }
}
