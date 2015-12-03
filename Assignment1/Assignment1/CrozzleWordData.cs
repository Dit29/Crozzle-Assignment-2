using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Assignment1
{
    /// <summary>
    /// This class contains a collection of methods:
    /// to scan words (scan and check every word), 
    /// scan missing words (scan missing word),
    /// scan the word direction (to make sure everything not inverted in the first place),
    /// intersecting letter scanner (later to determine the score),
    /// intersection scanner (for checking the word intersections, which will determine the total intersection should be less than 3),
    /// check word group connection (to ensure every word is connected to each other),
    /// word gap (for easy game, make sure the gap is provided).
    /// </summary>
    class CrozzleWordData
    {
        private List<WordData> wordData;
        private List<WordData> rowWordData;
        private List<WordData> columnWordData;
        private List<String> errors;

        #region constructors
        public CrozzleWordData(List<String[]> crozzleRow, List<String[]> crozzleColumn)
        {
            wordData = new List<WordData>();
            rowWordData = new List<WordData>();
            columnWordData = new List<WordData>();
            errors = new List<string>();

            this.addRowWord(crozzleRow);
            this.addColumnWord(crozzleColumn);
        }
        #endregion

        #region getters
        public int Count { get { return (rowWordData.Count + columnWordData.Count); } }
        public Boolean ErrorDetector { get { return (errors.Count > 0); } }
        public List<String> Errors { get { return (errors); } }
        #endregion

        /// <summary>
        /// Scan word is to scan and save every word in the crozzle to a big list of strings, which could be
        /// compared with the wordlist later.
        /// 
        /// First thing is to save every words in row part then the column part.
        /// </summary>
        /// <param name="crozzleScanRow">Get the crozzle word scanner with this parameter.</param>
        #region scan words
        private void addRowWord(List<String[]> crozzleScanRow)
        {
            int rowNumber = 0;
            int columnIndex;
            String rowContainer;

            foreach (String[] crozzleRow in crozzleScanRow)
            {
                rowNumber++;
                columnIndex = 0;

                // Combine all scanned letters into one string
                rowContainer = "";
                
                foreach (String letter in crozzleRow)
                {
                    if (Regex.IsMatch(letter, @"^[\s]"))
                        rowContainer += " ";
                    else
                        rowContainer += letter;
                }

                // Split and collect all letters by using char separators
                char[] separators = { ' ' };
                String[] letterSeq = rowContainer.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                // Store the letter sequence data (letterSeq) with requirement data length > 1
                // Why length > 1 ? Because a sequence of one letter is not a word
                foreach (String seq in letterSeq)
                {
                    if (seq.Length > 1)
                    {
                        // Scan if there's a duplicate word
                        if (this.Has(seq))
                          errors.Add("Error: \"" + seq + "\"already exists in the crozzle");
                        
                        // Update index for next sequence scan
                        WordData word = new WordData(seq, rowNumber, rowContainer.IndexOf(seq, columnIndex) + 1, true);
                        columnIndex = word.location.column - 1 + seq.Length;

                        // Store the word data
                        wordData.Add(word);
                        rowWordData.Add(word);
                    }
                }
            }
        }

        private void addColumnWord(List<String[]> crozzleScanColumn)
        {
            int columnNumber = 0;
            int rowIndex;
            String columnContainer;

            foreach (String[] crozzleColumn in crozzleScanColumn)
            {
                columnNumber++;
                rowIndex = 0;

                // Combine all scanned letters into one string
                columnContainer = "";
                
                foreach (String letter in crozzleColumn)
                {
                    if (Regex.IsMatch(letter, @"^[\s]"))
                        columnContainer += " ";
                    else
                        columnContainer += letter;
                }

                // Split and collect all letters by using char separators
                char[] separators = { ' ' };
                String[] letterSeq = columnContainer.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                // Store the letter sequence data (letterSeq) with requirement data length > 1
                // Why length > 1 ? Because a sequence of one letter is not a word
                foreach (String seq in letterSeq)
                {
                    if (seq.Length > 1)
                    {
                        // Scan if there's a duplicate word
                        if (this.Has(seq))
                          errors.Add("Error: \"" + seq + "\"already exists in the crozzle");

                        // Update index for next sequence scan
                        WordData word = new WordData(seq, columnNumber, columnContainer.IndexOf(seq, rowIndex) + 1, true);
                        rowIndex = word.location.row - 1 + seq.Length;

                        // Store the word data
                        wordData.Add(word);
                        columnWordData.Add(word);
                    }
                }
            }
        }

        private Boolean Has(String seq)
        {
            Boolean itHas = false;

            foreach (WordData word in wordData)
            {
                if (word.letters.Equals(seq))
                {
                    itHas = true;
                    break;
                }
            }

            return (itHas);
        }
        #endregion

        /// <summary>
        /// Scan the words that are not recoreded in the wordlist and avoid the program give more scores.
        /// </summary>
        /// <param name="wordList">the wordlist will be processed here then checks it one by one from crozzle.</param>
        #region scan missing words
        public void MissingWordsScanner(List<String> wordList)
        {
            foreach (WordData word in wordData)
                if (!wordList.Contains(word.letters))
                    errors.Add("Error: \"" + 
                        word.letters + "\" at (" + 
                        word.location.row.ToString() + ", " + 
                        word.location.column.ToString() + 
                        ") does not exist in the word list");
        }
        #endregion

        /// <summary>
        /// This method is for scanning the direction.
        /// The only allowed directions are "left-to-right" and "top-to-bottom"
        /// which makes "right-to-left" and "bottom-to-top" are not allowed.
        /// Which also means that there's no allowed inverted word.
        /// 
        /// Get all word in wordData, check the inverted word list if it exists, 
        /// then ignore the inverted word and give an error message and log.
        /// </summary>
        /// <param name="wordList">wordlist will be used as parameter.</param>
        #region direction scanner
        // Check whether the word direction is right or inverted
        // JILL or LLIJ
        public void DirectionScanner(List<String> wordList)
        {
            foreach (WordData word in wordData)
            {
                String invertedWord = new String(word.letters.Reverse().ToArray());

                // Ignore inverted words
                if (invertedWord.Equals(word.letters))
                    continue;

                if (!wordList.Contains(word.letters) && wordList.Contains(invertedWord))
                    errors.Add("Error: \"" + 
                        word.letters + "\" at (" + 
                        word.location.row.ToString() + ", " + 
                        word.location.column.ToString() + 
                        ") exists in the word list but the word is in backwards");
            }
        }
        #endregion

        #region intersecting letter scanner
        public List<Char> GetIntersectingLetters()
        {
            List<Char> intersectingLetters = new List<Char>();

            foreach (WordData rowWord in rowWordData)
            {
                intersectingLetters.AddRange(GetIntersectingLetters(rowWord));
            }

            return (intersectingLetters);
        }

        private List<Char> GetIntersectingLetters(WordData rowWord)
        {
            List<Char> intersectingLetters = new List<Char>();

            foreach (WordData columnWord in columnWordData)
            {
                if (columnWord.location.row == rowWord.location.row)
                {
                    if (columnWord.location.column >= rowWord.location.column && columnWord.location.column < rowWord.location.column + rowWord.letters.Length)
                        intersectingLetters.Add(columnWord.letters[0]);
                    
                }

                else if (columnWord.location.row < rowWord.location.row)
                {
                    if (columnWord.location.column >= rowWord.location.column && columnWord.location.column < rowWord.location.column + rowWord.letters.Length)
                        if (columnWord.location.row + columnWord.letters.Length > rowWord.location.row)
                            intersectingLetters.Add(columnWord.letters[rowWord.location.row - columnWord.location.row]);
                }
            }

            return (intersectingLetters);
        }
        #endregion

        /// <summary>
        /// Intersection scanner is a scanner for checking the intersecting words,
        /// the words should intersect at least one and maximum intersection is two.
        /// It will give a warning if the word does not intersect each other or intersects more than 3 times.
        /// </summary>
        #region intersections scanner
        /*
        public void IntersectionsScanner()
        {
            foreach (WordData word in wordData)
            {
                if (word.horizontalDirection)
                {
                    if (GetColumnIntersectingWords(word).Count == 0)
                    {
                        errors.Add("Error: \"" +
                            word.letters + "\" at (" +
                            word.location.row.ToString() + ", " +
                            word.location.column.ToString() +
                            ") is row word but does not intersect a column word");
                    }
                    else if (GetColumnIntersectingWords(word).Count >= 3)
                    {
                        errors.Add("Error: \"" +
                            word.letters + "\" at (" +
                            word.location.row.ToString() + ", " +
                            word.location.column.ToString() +
                            ") is row word but does intersect more than 2 column words");
                    }
                }

                else
                {
                    if (GetRowIntersectingWords(word).Count == 0)
                    {
                        errors.Add("Error: \"" + 
                            word.letters + "\" at (" +
                            word.location.row.ToString() + ", " +
                            word.location.column.ToString() + 
                            ") is column word but does not intersect a row word");
                    }
                    else if (GetRowIntersectingWords(word).Count >= 3)
                    {
                        errors.Add("Error: \"" +
                            word.letters + "\" at (" +
                            word.location.row.ToString() + ", " +
                            word.location.column.ToString() +
                            ") is column word but does intersect more than 2 row words");
                    }
                }
            }       
        }
        */
        public void IntersectionsScanner(int low)
        {
            foreach (WordData word in wordData)
                if (word.horizontalDirection)
                {
                    if (GetColumnIntersectingWords(word).Count < low)
                    {
                        errors.Add("Error: \"" +
                            word.letters + "\" at (" +
                            word.location.row.ToString() + ", " +
                            word.location.column.ToString() +
                            ") is row word but does not intersect a correct number of a column word");
                    }
                }
                else
                {
                    if (GetRowIntersectingWords(word).Count < 0)
                    {
                        errors.Add("Error: \"" +
                            word.letters + "\" at (" +
                            word.location.row.ToString() + ", " +
                            word.location.column.ToString() +
                            ") is column word but does not intersect a correct number of a row word");
                    }
                }
        }

        public void IntersectionsScanner(int low, int high)
        {
            foreach (WordData word in wordData)
            {
                if (word.horizontalDirection)
                {
                    int columnIntersections = GetColumnIntersectingWords(word).Count;
                    if (columnIntersections < low || columnIntersections > high)
                    {
                        errors.Add("Error: \"" +
                            word.letters + "\" at (" +
                            word.location.row.ToString() + ", " +
                            word.location.column.ToString() +
                            ") is row word but does not intersect a correct number of a column word");
                    }
                }
                else
                {
                    int rowIntersections = GetRowIntersectingWords(word).Count;
                    if (rowIntersections < low || rowIntersections > high)
                    {
                        errors.Add("Error: \"" +
                            word.letters + "\" at (" +
                            word.location.row.ToString() + ", " +
                            word.location.column.ToString() +
                            ") is column word but does not intersect a correct number of a row word");
                    }
                }
            }
        }

        private List<WordData> GetColumnIntersectingWords(WordData rowWord)
        {
            List<WordData> columnWords = new List<WordData>();

            foreach (WordData columnWord in columnWordData)
            {
                if (columnWord.location.row == rowWord.location.row)
                {
                    if (columnWord.location.column >= rowWord.location.column && columnWord.location.column < rowWord.location.column + rowWord.letters.Length)
                        columnWords.Add(rowWord);
                }

                else if (columnWord.location.row < rowWord.location.row)
                {
                    if (columnWord.location.column >= rowWord.location.column && columnWord.location.column < rowWord.location.column + rowWord.letters.Length)
                        if (columnWord.location.row + columnWord.letters.Length > rowWord.location.row)
                            columnWords.Add(rowWord);
                }
            }

            return (columnWords);
        }

        private List<WordData> GetRowIntersectingWords(WordData columnWord)
        {
            List<WordData> rowWords = new List<WordData>();

            foreach (WordData rowWord in rowWordData)
            {
                if (rowWord.location.column == columnWord.location.column)
                {
                    if (rowWord.location.row >= columnWord.location.row && rowWord.location.row < columnWord.location.row + columnWord.letters.Length)
                        rowWords.Add(rowWord);
                }

                else if (rowWord.location.column < columnWord.location.column)
                {
                    if (rowWord.location.row >= columnWord.location.row && rowWord.location.row < columnWord.location.row + columnWord.letters.Length)
                        if (rowWord.location.column + rowWord.letters.Length > columnWord.location.column)
                            rowWords.Add(rowWord);
                }
            }

            return (rowWords);
        }
        #endregion

        /// <summary>
        /// Checking the word group connection, because it's in the constraint the crozzle words is into a one big group.
        /// It will make a copy of the crozzle, then iterate by removing them one-by-one, if the crozzle box contains
        /// one or more group, then give an error.
        /// </summary>
        /// <param name="crozzleRows">Identify the crozzle rows.</param>
        /// <param name="crozzleColumns">Identify the crozzle columns.</param>
        #region check word group connection
        public void CheckWordGroup(List<String[]> crozzleRows, List<String[]> crozzleColumns)
        {
            // Create a copy of crozzle box.
            CrozzleBox box = new CrozzleBox(crozzleRows, crozzleColumns);

            // Remove the the group of words from the box.
            box.RemoveWordGroup();

            // Check whether the box still has the word groups or not.
            if (box.ContainsWordGroup)
            {
                errors.Add("Error: this crozzle has more than 1 group of words");
            }
        }
        #endregion

        /// <summary>
        /// prbably still wrong, fix thisb part, probably can be deleted.
        /// </summary>
        /// <returns></returns>
        #region scan every letter in crozzle
        public List<Char> GetEveryLetter()
        {
            List<Char> everyLetter = new List<Char>();

            foreach (WordData word in wordData)
                if (Regex.IsMatch(word.letters, "^[a-zA-Z]+$"))
                    everyLetter.AddRange(GetEveryLetter(everyLetter));
                
            return (everyLetter);

            /*foreach (String letter in letters)
            {
                everyLetter.AddRange(GetEveryLetter(letter));
            } */
        }

        private List<Char> GetEveryLetter(List<Char> letters)
        {
            List<Char> everyLetter = new List<Char>();

            foreach (WordData word in wordData)
                everyLetter.Add(letters[0]);

            return (everyLetter);
        }

        /*
        private List<Char> GetEveryLetter(WordData letters)
        {
            List<Char> everyLetter = new List<Char>();

            foreach (String letter in letters)
        } */
        #endregion

        /*
        /// <summary>
        /// Check the word gap for easy difficulty
        /// It will check the word surrounding at least one box and 
        /// the word must not connect to the start or the end of the letter
        /// </summary>
        #region easy difficulty word gap
        public void WordGap()
        {
            foreach (WordData word in wordData)
            {
                // propbably still wrong
                if (word.horizontalDirection)
                {
                    if (GetColumnWordGap(word).Count <= 1)
                    {
                        errors.Add("Error: \"" +
                            word.location.ToString() +
                            " is violating the gap requirement.");
                    }

                    else if (GetRowWordGap(word).Count <= 1)
                    {
                        errors.Add("Error: \"" +
                            word.location.ToString() +
                            " is violating the gap requirement.");
                    }
                }

                else
                {
                    if (GetColumnWordGap(word).Count <= 1)
                    {
                        errors.Add("Error: \"" +
                            word.location.ToString() +
                            " is violating the gap requirement.");
                    }

                    else if (GetRowWordGap(word).Count <= 1)
                    {
                        errors.Add("Error: \"" +
                            word.location.ToString() +
                            " is violating the gap requirement.");
                    }
                }
            }
        }

        private List<WordData> GetColumnWordGap(WordData rowWord)
        {
            List<WordData> columnWords = new List<WordData>();

            foreach (WordData columnWord in columnWordData)
            {
                if (columnWord.location.column.Equals(columnWord.location.row + 1))
                {
                    errors.Add("Error: \"" + 
                        columnWord.location.column.ToString() + 
                        ", this word is violating gap of " +
                        (columnWord.location.row + 1).ToString() +
                        " word.");
                }

                else if (columnWord.location.column.Equals(columnWord.location.column + 1))
                {
                    errors.Add("Error: \"" +
                        columnWord.location.column.ToString() +
                        ", this word is violating gap of " +
                        (columnWord.location.column + 1).ToString() +
                        " word.");
                }

                else if (columnWord.location.row.Equals(columnWord.location.column + 1))
                {
                    errors.Add("Error: \"" +
                        columnWord.location.row.ToString() +
                        ", this word is violating gap of " +
                        (columnWord.location.column + 1).ToString() +
                        " word.");
                }

                else if (columnWord.location.row.Equals(columnWord.location.row + 1))
                {
                    errors.Add("Error: \"" +
                        columnWord.location.row.ToString() +
                        ", this word is violating gap of " +
                        (columnWord.location.row + 1).ToString() +
                        " word.");
                }
            }

            return (columnWords);
        }

        private List<WordData> GetRowWordGap(WordData columnWord)
        {
            List<WordData> rowWords = new List<WordData>();

            foreach (WordData rowWord in rowWordData)
            {
                if (rowWord.location.row.Equals(rowWord.location.row + 1))
                {
                    errors.Add("Error: \"" +
                        rowWord.location.row.ToString() +
                        ", this word is violating gap of " +
                        (rowWord.location.row + 1).ToString() +
                        " word.");
                }

                else if (rowWord.location.column.Equals(rowWord.location.row + 1))
                {
                    errors.Add("Error: \"" +
                        rowWord.location.column.ToString() +
                        ", this word is violating gap of " +
                        (rowWord.location.row + 1).ToString() +
                        " word.");
                }

                else if (rowWord.location.row.Equals(rowWord.location.column + 1))
                {
                    errors.Add("Error: \"" +
                        rowWord.location.row.ToString() +
                        ", this word is violating gap of " +
                        (rowWord.location.column + 1).ToString() +
                        " word.");
                }

                else if (rowWord.location.column.Equals(rowWord.location.column + 1))
                {
                    errors.Add("Error: \"" +
                        rowWord.location.column.ToString() +
                        ", this word is violating gap of " +
                        (rowWord.location.column + 1).ToString() +
                        " word.");
                }
            }

            return (rowWords);
        }
        #endregion*/
    }
}
