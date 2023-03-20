using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tLaText.Core
{
    internal struct ACursor
    {
        /// <summary>
        /// Position of the main cursor.
        /// </summary>
        public int Cursor;
        /// <summary>
        /// Position of the alternate cursor.
        /// </summary>
        public int Alt;
        /// <summary>
        /// Whether there is a selection.
        /// </summary>
        public bool Selecting => Alt != Cursor;
        /// <summary>
        /// Get range of selection.
        /// </summary>
        public Range Selection
        {
            get
            {
                if (Alt > Cursor)
                {
                    return new Range(Cursor, Alt);
                }
                return new Range(Alt, Cursor);
            }
        }

        public ACursor()
        {
            Cursor = 0;
            Alt = 0;
        }
        public ACursor(int main)
        {
            this.Cursor = main;
            Alt = main;
        }
        /// <summary>
        /// Move the selection by <paramref name="length"/>
        /// </summary>
        /// <param name="length"></param>
        public void MoveSelection(int length)
        {
            Cursor += length;
            Alt += length;
        }
    }
}
