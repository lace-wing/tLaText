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
        /// <summary>
        /// Clamps each value of the Point using <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Point Clamp(this Point p, int min, int max)
        {
            return new Point(Math.Clamp(p.X, min, max), Math.Clamp(p.Y, min, max));
        }
        /// <summary>
        /// Clamps each value of the Point using <paramref name="domain"/>.X as min and <paramref name="domain"/>.Y as max.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static Point Clamp(this Point p, Point domain)
        {
            return new Point(Math.Clamp(p.X, domain.X, domain.Y), Math.Clamp(p.Y, domain.X, domain.Y));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="addX"></param>
        /// <param name="addY"></param>
        /// <returns>A new Point(<paramref name="p"/>.X + <paramref name="addX"/>, <paramref name="p"/>.Y + <paramref name="addY"/>).</returns>
        public static Point Modify(this Point p, int addX, int addY)
        {
            return new Point(p.X + addX, p.Y + addY);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="inverse"></param>
        /// <returns>Point.X - Point.Y or Point.Y - Point.X if <paramref name="inverse"/></returns>
        public static int Sub(this Point p, bool inverse = false)
        {
            return inverse ? p.Y - p.X : p.X - p.Y;
        }
    }
}
