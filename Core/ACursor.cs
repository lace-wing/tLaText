﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tLaText.Core
{
    internal struct ACursor
    {
        private int cursor;
        private int alt;

        /// <summary>
        /// Position of the main cursor.
        /// </summary>
        public int Cursor { get => cursor; set => cursor = Math.Max(0, value); }
        /// <summary>
        /// Position of the alternate cursor.
        /// </summary>
        public int Alt { get => alt; set => alt = Math.Max(0, value); }
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

        public ACursor()
        {
            Cursor = 0;
            Alt = 0;
        }
        public ACursor(int cursor)
        {
            Cursor = cursor;
            Alt = cursor;
        }

        /// <summary>
        /// Set the main and alternate cursor to given values, only effective if value > 0.
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="alt"></param>
        public void SetCursor(int cursor = -1, int alt = -1)
        {
            if (cursor > 0)
            {
                Cursor = cursor;
            }
            if (alt > 0)
            {
                Alt = alt;
            }
        }
        /// <summary>
        /// Move the main cursor by <paramref name="cursor"/> and move the alternate cursor by <paramref name="alt"/>.
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="alt"></param>
        public void MoveCursor(int cursor = 0, int alt = 0)
        {
            Cursor += cursor;
            Alt += alt;
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
        /// <summary>
        /// Move the alternate cursor to the main cursor.
        /// </summary>
        public void CancelSelection()
        {
            Alt = Cursor;
        }
    }
}
