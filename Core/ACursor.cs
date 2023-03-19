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
        public Point Selection
        {
            get
            {
                if (Alt > Cursor)
                {
                    return new Point(Cursor, Alt);
                }
                return new Point(Alt, Cursor);
            }
        }
        /// <summary>
        /// Color of the main cursor.
        /// </summary>
        public Color CursorColor;
        /// <summary>
        /// Color of selected area.
        /// </summary>
        public Color SelectionColor;

        public ACursor()
        {
            Cursor = 0;
            Alt = 0;
            CursorColor = Color.White;
            SelectionColor = Color.Blue;
        }
        public ACursor(int main, Color cursorColor = default, Color selectionColor = default)
        {
            this.Cursor = main;
            Alt = main;
            if (cursorColor == default)
            {
                cursorColor = Color.White;
            }
            else
            {
                CursorColor = cursorColor;
            }
            if (selectionColor == default)
            {
                selectionColor = Color.Blue;
            }
            else
            {
                SelectionColor = selectionColor;
            }
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
