﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tLaText.Core
{
    internal struct LaText
    {
        // private static Dictionary<string, LaTextOp> ops = new Dictionary<string, LaTextOp>();

        private string[] cache;
        private int cIndex;

        public int CIndex { get { return Math.Clamp(cIndex, 0, Math.Max(cache.Length - 1, 0)); } set { cIndex = Math.Clamp(value, 0, Math.Max(cache.Length - 1, 0)); } }
        public LaCursor Cursor { get; private set; }

        public string Text { get => cache[CIndex] == null ? string.Empty : cache[CIndex]; }
        public string[] Words { get => Text.Split(' '); }
        public string[] Lines { get => Text.Split('\n'); } //TODO properly split

        public LaText()
        {
            cache = new string[9];
            CIndex = 0;
            Cursor = new LaCursor();
        }
        public LaText(int cacheLength = 9, Color cursorColor = default, Color selectionColor = default)
        {
            cursorColor = cursorColor == default ? Color.White : cursorColor;
            selectionColor = selectionColor == default ? Color.Blue : selectionColor;
            cache = new string[cacheLength];
            CIndex = 0;
            Cursor = new LaCursor(new List<ACursor>(), cursorColor, selectionColor);
        }

        #region Cache
        /// <summary>
        /// Move all elements in cache by step, exceeding element will be default
        /// </summary>
        /// <param name="step">Can be negative</param>
        private void MoveCache(int step = 1)
        {
            if (cache.Length <= 0 || step == 0)
            {
                return;
            }
            if (Math.Abs(step) >= cache.Length)
            {
                for (int i = 0; i < cache.Length; i++)
                {
                    cache[i] = default;
                }
                return;
            }
            if (step < 0)
            {
                for (int i = 0; i < cache.Length + step; i++)
                {
                    cache[i] = cache[i - step];
                }
                for (int i = -1; i >= step; i--)
                {
                    cache[cache.Length + i] = default;
                }
            }
            else
            {
                for (int i = cache.Length - 1; i > step - 1; i--)
                {
                    cache[i] = cache[i - step];
                }
                for (int i = 0; i < step - 1; i++)
                {
                    cache[i] = default;
                }
            }
        }
        #endregion

        #region Undo & redo
        /// <summary>
        /// Tries to move cIndex along cache by <paramref name="length"/> then clear Cursor.
        /// </summary>
        /// <param name="length">Can be negative (redo).</param>
        /// <returns>If the movement is successful.</returns>
        public bool UndoBy(int length)
        {
            if (!(CIndex + length).Within(0, cache.Length - 1))
            {
                return false;
            }
            CIndex += length;
            Cursor.ClearCursors();
            return true;
        }

        /// <summary>
        /// Tries to undo and clear Cursor.
        /// </summary>
        /// <returns>If the undo is successful.</returns>
        public bool Undo()
        {
            return UndoBy(1);
        }
        /// <summary>
        /// Tries to redo and clear Cursor.
        /// </summary>
        /// <returns>If the redo is successful</returns>
        public bool Redo()
        {
            return UndoBy(-1);
        }
        #endregion
    }
}
