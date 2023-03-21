using Microsoft.Xna.Framework;
using rail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tLaText.Core
{
    internal struct LaCursor
    {
        private Point domain;
        /// <summary>
        /// All cursors.
        /// </summary>
        public List<ACursor> Cursors { get; private set; }
        /// <summary>
        /// Range of the cursors, cursors can only fall into this range.
        /// </summary>
        public Point Domain { get => domain; private set { domain.X = value.X; domain.Y = Math.Max(domain.X, value.Y); } }
        /// <summary>
        /// Color of the cursors.
        /// </summary>
        public Color CursorColor;
        /// <summary>
        /// Color of selected areas.
        /// </summary>
        public Color SelectionColor;

        #region Constructors
        public LaCursor()
        {
            Cursors = new List<ACursor>();
            Domain = new Point(); 
            CursorColor = Color.White;
            SelectionColor = Color.Blue;
        }
        public LaCursor(List<ACursor> cursors, Color cursorColor, Color selectionColor)
        {
            Cursors = cursors;
            CursorColor = cursorColor;
            SelectionColor = selectionColor;
        }
        #endregion

        #region Private functions
        /// <summary>
        /// Removes cursors out of Domain and moves alternate cursors into the Domain.
        /// </summary>
        private void ClampCursors()
        {
            int min = Domain.X;
            int max = Domain.Y;
            for (int i = 0; i < Cursors.Count; i++)
            {
                if (!Cursors[i].Cursor.Within(min, max))
                {
                    Cursors.RemoveAt(i);
                    continue;
                }
                if (Cursors[i].Alt < min)
                {
                    Cursors[i].SetCursor(alt: min);
                    continue;
                }
                if (Cursors[i].Alt > max)
                {
                    Cursors[i].SetCursor(alt: max);
                    continue;
                }
            }
        }
        /// <summary>
        /// Order the cursors in ascending order.
        /// </summary>
        private void OrderCursor()
        {
            Cursors = Cursors.OrderBy(c => c.Cursor).ToList();
        }
        /// <summary>
        /// Checks if cursors at c1 and c2 overlap.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        private bool CursorOverlap(int c1, int c2)
        {
            return Cursors[c1].Selection.Overlap(Cursors[c2].Selection);
        }
        /// <summary>
        /// Tries to find where should cursor of the merged cursor be.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="newCursor"></param>
        /// <param name="newAlt"></param>
        /// <returns></returns>
        private bool TryGetMergedCursor(int c1, int c2, out int newCursor, out int newAlt)
        {
            newCursor = 0;
            newAlt = 0;
            if (!CursorOverlap(c1, c2))
            {
                return false;
            }
            Point selection = Cursors[c1].Selection.MergeWith(Cursors[c2].Selection);
            if (Cursors[c1].Cursor == selection.X || Cursors[c2].Cursor == selection.X)
            {
                newCursor = selection.X;
                newAlt = selection.Y;
                return true;
            }
            if (Cursors[c1].Cursor == selection.Y || Cursors[c2].Cursor == selection.Y)
            {
                newCursor = selection.Y;
                newAlt = selection.X;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Merges all overlaping cursors by modifying the former one and deleting the latter one.
        /// </summary>
        private void MergeOverlaps()
        {
            for (int i = 0; i < Cursors.Count - 1; i++)
            {
                for (int j = i + 1; j < Cursors.Count; j++)
                {
                    if (!TryGetMergedCursor(i, j, out int c, out int a))
                    {
                        continue;
                    }
                    Cursors[i].SetCursor(c, a);
                    Cursors.RemoveAt(j);
                }
            }
        }
        /// <summary>
        /// Run all the cursor-cleaning functions.
        /// </summary>
        /// <param name="clamp"></param>
        private void CleanCursors(bool clamp = false)
        {
            if (clamp)
            {
                ClampCursors();
            }
            OrderCursor();
            MergeOverlaps();
        }
        /// <summary>
        /// Cancel selections of all cursors.
        /// </summary>
        private void CancelSelections()
        {
            for (int i = 0; i < Cursors.Count; i++)
            {
                Cursors[i].CancelSelection();
            }
        }
        #endregion

        #region Public functions
        /// <summary>
        /// Set Domain to to given value.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void SetDomain(int min, int max)
        {
            Domain = new Point(min, max);
            CleanCursors(true);
        }
        /// <summary>
        /// Set Domain to to given value.
        /// </summary>
        /// <param name="newDomain"></param>
        public void SetDomain(Point newDomain)
        {
            Domain = newDomain;
            CleanCursors(true);
        }
        /// <summary>
        /// Add a new cursor.
        /// </summary>
        /// <param name="cursor"></param>
        private void NewCursor(int cursor)
        {
            cursor = Math.Clamp(cursor, Domain.X, Domain.Y);
            Cursors.Add(new ACursor(cursor));
            CleanCursors();
        }
        /// <summary>
        /// Clear all cursors then add a new one.
        /// </summary>
        /// <param name="cursor"></param>
        public void RenewCursor(int cursor)
        {
            Cursors.Clear();
            cursor = Math.Clamp(cursor, Domain.X, Domain.Y);
            Cursors.Add(new ACursor(cursor));
        }
        #endregion
    }
}
