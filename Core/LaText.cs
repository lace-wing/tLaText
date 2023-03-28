using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        #region Contructors
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
        #endregion

        #region Cursor management
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
        /// Update Domain using <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void UpdateDomain(int min, int max)
        {
            Cursor.SetDomain(min, max);
        }
        /// <summary>
        /// Move Cursor by <paramref name="step"/> in direction <paramref name="dir"/>.
        /// </summary>
        /// <param name="step"></param>
        /// <param name="dir"></param>
        public void MoveCursorBy(int step = 1, int dir = 1)
        {
            step = Math.Abs(step);
            dir = Math.Sign(dir);
            Cursor.MoveAllCursors(step * dir);
        }
        /// <summary>
        /// Move Cursor by <paramref name="step"/> in direction <paramref name="dir"/>, but does not move alternate cursors.
        /// </summary>
        /// <param name="step"></param>
        /// <param name="dir"></param>
        public void SelectBy(int step = 1, int dir = 1)
        {
            step = Math.Abs(step);
            dir = Math.Sign(dir);
            Cursor.MoveAllCursors(step * dir, true);
        }
        /// <summary>
        /// Clears all cursors, then adds a new one, selects from Domain.X to Domain.Y.
        /// </summary>
        public void SelectAll()
        {
            RenewCursorAt(Cursor.Domain.X);
            SelectBy(Cursor.Domain.Sub());
        }
        /// <summary>
        /// Clear Cursor then place a cursor at <paramref name="position"/>.
        /// </summary>
        /// <param name="position"></param>
        public void RenewCursorAt(int position)
        {
            position = Math.Clamp(position, Cursor.Domain.X, Cursor.Domain.Y);
            Cursor.RenewCursor(position);
        }
        #endregion

        #region Cursor-text integration
        /// <summary>
        /// Get indexes of chars corresponding to main cursor and alternate cursor of Cursor.Cursors[<paramref name="index"/>].
        /// </summary>
        /// <param name="index"></param>
        /// <returns>A new Point(index of char at the main cursor, index of char at the alternate cursor).</returns>
        private Point GetCharIndex(int index)
        {
            if (index < 0 || index > Cursor.Count)
            {
                return new Point(-1, -1);
            }
            return Cursor[index].Selection.Clamp(Cursor.Domain.Modify(0, -1));
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
            MoveCache(-CIndex + 1);
            cache[0] = text;
            CIndex = 0;
            UpdateDomain(0, Text.Length);
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
            if (!(CIndex + length).Within(0, cache.Length - 1) || cache[CIndex + length] == default)
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

        #region Text editing
        /// <summary>
        /// Insert <paramref name="text"/> at Cursors[<paramref name="index"/>], will replace selected text.
        /// <br>Cached.</br>
        /// </summary>
        /// <param name="text"></param>
        public void InsertAt(string text, int index) //TODO test
        {
            string nt = Text;
            Point range = GetCharIndex(index);
            nt = nt.Remove(range.X, range.Sub());
            nt = nt.Insert(range.X, text);
            CacheText(nt);
            Cursor.MoveThisAndLaterCursors(index, text.Length - GetCharIndex(index).Sub(), Cursor[index].Selecting);
        }
        /// <summary>
        /// Insert <paramref name="text"/> at all cursors, will replace selected text.
        /// </summary>
        /// <param name="text"></param>
        public void Insert(string text) //TODO test
        {
            string nt = Text;
            for (int i = 0; i < Cursor.Count; i++)
            {
                Point range = GetCharIndex(i);
                nt = nt.Remove(range.X, range.Sub());
                nt = nt.Insert(range.X, text);
                UpdateDomain(0, nt.Length);
                Cursor.MoveThisAndLaterCursors(i, text.Length - range.Sub(), Cursor[i].Selecting);
            }
            CacheText(nt);
        }
        /// <summary>
        /// Deletes text at Cursor[<paramref name="index"/>].
        /// <br>If not selecting, deletes char at its left side.</br>
        /// <br>If selecting, deletes text in the selection.</br>
        /// </summary>
        /// <param name="index"></param>
        public void DeleteAt(int index)
        {
            string nt = Text;
            Point range = GetCharIndex(index);
            int count = range.Sub() == 0 ? 1 : range.Sub();
            nt = nt.Remove(range.Y - count, count);
            Cursor.MoveThisAndLaterCursors(index, -count);
            CacheText(nt);
        }
        /// <summary>
        /// Deletes text at Cursor.
        /// <br>If not selecting, deletes char at its left side.</br>
        /// <br>If selecting, deletes text in the selection.</br>
        /// </summary>
        public void Delete()
        {
            string nt = Text;
            for (int i = 0; i < Cursor.Count; i++)
            {
                Point range = GetCharIndex(i);
                int count = range.Sub() == 0 ? 1 : range.Sub();
                nt = nt.Remove(range.Y - count, count);
                Cursor.MoveThisAndLaterCursors(i, -count);
            }
            CacheText(nt);
        }
        #endregion
    }
}
