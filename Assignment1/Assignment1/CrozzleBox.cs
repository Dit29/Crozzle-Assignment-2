using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    /// <summary>
    /// CrozzleBox class contains mostly methods that controls the boolean for crozzle box and
    /// remove word group from crozzle duplicate.
    /// Crozzle boolean box is for checking the rows and columns of the crozzle box.
    /// While remove word group method is for remove a word group connection then determine
    /// whether the crozzle has the right word group connection or not.
    /// </summary>
    class CrozzleBox
    {
        // Create crozzle box with ',' separated value.
        private Boolean[,] box;

        /// <summary>
        /// Create a 2D array of boolean box.
        /// give the box the position.
        /// Then store every correct value with the same position of letters.
        /// </summary>
        /// <param name="crozzleRows">list of crozzle rows that contain the strings</param>
        /// <param name="crozzleColumns">list of crozzle columns that contain the strings</param>
        #region constructors
        public CrozzleBox(List<String[]> crozzleRows, List<String[]> crozzleColumns)
        {
            // Create 2D array of everytime the boolean return false
            int numberOfRows = crozzleRows.Count;
            int numberOfColumns = crozzleColumns.Count;
            box = new Boolean[numberOfRows, numberOfColumns];

            // Store every true value after the crozzle box being processed
            // with the same position of the letters.
            this.boxes(crozzleRows);
        }
        #endregion

        #region getters
        public Boolean ContainsWordGroup
        {
            get 
            { 
                Boolean exist = false;

                foreach (Boolean flag in box)
                {
                    if (flag)
                    {
                        exist = true;
                        break;
                    }
                }

                return (exist);
            }
        }
        #endregion

        /// <summary>
        /// Create the boolean crozzle box with the total rows and check the contained word one by one using loop.
        /// </summary>
        /// <param name="crozzleRows">list of crozzle rows that contain the strings</param>
        #region create boolean crozzle box
        private void boxes(List<String[]> crozzleRows)
        {
            int row;
            int column;

            row = 0;
            foreach (String[] words in crozzleRows)
            {
                row++;
                column = 0;

                foreach (String word in words)
                {
                    column++;
                    if (word.Length == 1)
                    {
                        box[row, column] = true;
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// The remove word group is a method to remove the word group from crozzle duplicate
        /// which if the crozzle only has one group, then the crozzle is valid.
        /// 
        /// It starts from position of the crozzle then continued with recursive calls 
        /// and leads that to remove word group.
        /// </summary>
        #region remove word group
        public void RemoveWordGroup()
        {
            // Remove a words group from the box.
            // If all words are connected in one group,
            // The box will containing only false values words group.

            // Set the start coordinate position.
            // Which can be any letter on the crozzle box.
            Position startPosition = this.FindCoordinate();

            // List of position or recursive calls
            List<Position> coordinate = new List<Position>();
            coordinate.Add(startPosition);

            // Remove word group.
            RemoveWordGroup(coordinate);
        }

        public Position FindCoordinate()
        {
            Position coordinate = new Position(-1, -1);

            for (int row = 0; row < box.GetLength(0); row++)
                for (int column = 0; column < box.GetLength(1); column++)
                    if (box[row, column])
                    {
                        coordinate.row = row;
                        coordinate.column = column;
                    }
            return (coordinate);
        }

        private void RemoveWordGroup(List<Position> coordinates)
        {
            // Remove a words group from the box.
            // If all words are connected in one group,
            // The box will containing only false values words group.

            foreach (Position coordinate in coordinates)
            {
                if (coordinate.Equals(null))
                {
                    // Remove the first letter of the word group.
                    box[coordinate.row, coordinate.column] = false;

                    // Get next coordinate of the rest of letters.
                    List<Position> nextCoordinate = GetNextCoordinate(coordinate);



                    // Remove the rest of word group.
                    RemoveWordGroup(nextCoordinate);

                }
            }
        }

        private List<Position> GetNextCoordinate(Position coordinate)
        {
            List<Position> nextCoordinate = new List<Position>();

            if (box[coordinate.row, coordinate.column - 1] == true)
            {
                Position position = new Position(coordinate.row, coordinate.column - 1);
                nextCoordinate.Add(position);
            }

            if (box[coordinate.row, coordinate.column + 1] == true)
            {
                Position position = new Position(coordinate.row, coordinate.column + 1);
                nextCoordinate.Add(position);
            }

            if (box[coordinate.row - 1, coordinate.column] == true)
            {
                Position position = new Position(coordinate.row - 1, coordinate.column);
                nextCoordinate.Add(position);
            }

            if (box[coordinate.row + 1, coordinate.column] == true)
            {
                Position position = new Position(coordinate.row + 1, coordinate.column);
                nextCoordinate.Add(position);
            }

            return (nextCoordinate);
        }
        #endregion
    }
}