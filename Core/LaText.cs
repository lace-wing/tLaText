using Microsoft.Xna.Framework;
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
            cache = new string[12];
            CIndex = 0;
            Cursor = new LaCursor();
        }
        public LaText(int cacheLength = 12, Color cursorColor = default, Color selectionColor = default)
        {
            cacheLength = Math.Max(1, cacheLength);
            cursorColor = cursorColor == default ? Color.White : cursorColor;
            selectionColor = selectionColor == default ? Color.Blue : selectionColor;
            cache = new string[cacheLength];
            CIndex = 0;
            Cursor = new LaCursor(new List<ACursor>(), cursorColor, selectionColor);
        }

        #region Cursor management
        /// <summary>
        /// Update Domain using <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        private void UpdateDomain(int min, int max)
        {
            Cursor.SetDomain(min, max);
        }
        /// <summary>
        /// Refresh Cursor status.
        /// </summary>
        private void ResetCursor()
        {
            Cursor.ClearCursors();
            Cursor.SetDomain(0, Text.Length);
        }
        /// <summary>
        /// Refresh Cursor status, set Domain using <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        private void ResetCursor(int min, int max)
        {
            Cursor.ClearCursors();
            Cursor.SetDomain(min, max);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        /// <param name="dir"></param>
        public void MoveCursorBy(int step = 1, int dir = 1)
        {
            step = Math.Abs(step);
            dir = Math.Sign(dir);
            Cursor.MoveCursors(step * dir);
        }
        public void MoveCursorTo(int position)
        {
            position = Math.Clamp(position, Cursor.Domain.X, Cursor.Domain.Y);
            Cursor.RenewCursor(position);
        }
        #endregion

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
        /// <summary>
        /// Caches <paramref name="text"/> into cache[0] with refreshing.
        /// </summary>
        /// <param name="text"></param>
        private void CacheText(string text)
        {
            MoveCache();
            cache[0] = text;
            CIndex = 0;
            ResetCursor();
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
            ResetCursor();
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
