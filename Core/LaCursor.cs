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
    }
}
