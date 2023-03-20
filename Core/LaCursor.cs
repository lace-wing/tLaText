using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tLaText.Core
{
    internal struct LaCursor
    {
        /// <summary>
        /// All cursors.
        /// </summary>
        public List<ACursor> Cursors { get; private set; }
        /// <summary>
        /// Range of the cursors, cursors can only fall into this range.
        /// </summary>
        public Range Domain { get; private set; }
        /// <summary>
        /// Color of the cursors.
        /// </summary>
        public Color CursorColor;
        /// <summary>
        /// Color of selected areas.
        /// </summary>
        public Color SelectionColor;

        public LaCursor()
        {
            Cursors = new List<ACursor>();
            Domain = new Range(); 
            CursorColor = Color.White;
            SelectionColor = Color.Blue;
        }
        public LaCursor(List<ACursor> cursors, Color cursorColor, Color selectionColor)
        {
            Cursors = cursors;
            CursorColor = cursorColor;
            SelectionColor = selectionColor;
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
        /// Finds where should cursor of the merged cursor be.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        private int FindMergedCursor(int c1, int c2)
        {
            Range selection = Cursors[c1].Selection.MergeWith(Cursors[c2].Selection);
            if (Cursors[c1].Cursor == selection.Start.Value)
            {
                return selection.Start.Value;
            }
            if (Cursors[c1].Cursor == selection.End.Value)
            {
                return selection.Start.Value;
            }
            if (Cursors[c2].Cursor == selection.Start.Value)
            {
                return selection.Start.Value;
            }
            if (Cursors[c2].Cursor == selection.End.Value)
            {
                return selection.Start.Value;
            }
            return selection.Start.Value;
        }
        private void MergeOverlaps()
        {
            for (int i = 0; i < Cursors.Count - 1; i++)
            {
                for (int j = i + 1; j < Cursors.Count; j++)
                {
                    if (!CursorOverlap(i, j))
                    {
                        continue;
                    }
                    Cursors[i].
                }
            }
        }

        /// <summary>
        /// Add a cursor then reorder the cursors.
        /// </summary>
        /// <param name="cursor"></param>
        public void AddCursor(ACursor cursor)
        {
            Cursors.Add(cursor);
            OrderCursor();
        }
    }
}
