using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Assignment1
{
    class WordData
    {
        public String letters;
        public Boolean horizontalDirection;
        public Position location;

        #region constructors
        public WordData(String sequence, int row, int column, Boolean direction)
        {
            letters = sequence;
            location = new Position(row, column);
            horizontalDirection = direction;
        }
        #endregion
    }
}