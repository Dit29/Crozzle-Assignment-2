using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Assignment1
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// If you want to try to open the crozzle file
        /// First thing you need to do is to open the suitable wordlist
        /// then open the crozzle which has the same difficulty level
        /// Because my program will read, validate and give the score
        /// pretty much immediately by opening crozzle file.
        /// (for example, open Ass2 - Test 1 - wordlist EASY.csv
        /// then open EasyCrozzle.txt).
        /// </summary>

        public int totalWords;     // Total words.
        public int totalRows;      // Total rows.
        public int totalColumns;   // Total columns.
        public int tempSc = 0;  // Score
        public string logWriter;       // For writing log.
        public StreamWriter writer;    // Writer
        public bool[,] rSc;     // Row checker pointer.
        public bool[,] cSc;  // Column checker pointer.
        public bool[,] rThere;       // Check whether the row word is exist.
        public string[,] Crz1;      // The crozzle.
        public string[,] Crz2;  // tempCrozzle is for make a temporary crozzle for finding highest score.
        public List<WordData> crzR;     // Crozzle row wordlist.
        public List<WordData> crzC;  // Crozzle column wordlist
        public List<Crozzle> crzLs;         // Wordlist.
        public List<Crozzle> crzLsT;   // Wordlist for temporary crozzle.
        public List<Position> ij;     // List of positions.
        public EventHandler<LongProcessEventArgs> computeCrozzle;   // Handler for computing the crozzle.
        List<Position> ijT;  // List of position for temporary crozzle.
        public string crzValids;     // Checking crozzle validity.
        public string scr;               // Score.
        public string disCrz;         // Show Crozzle.
        public string disWL;        // Show Wordlist.
        public bool isTrue;
        public string fCroz;
        private CrozzleProcess cProc;      // Crozzle process handler.
        private CrozzleDifficulty cd;       // Difficulty checker.
        private ObservableCollection<string> wLcol;    // Wordlist Collection.
        private ObservableCollection<RowList> crzlRow;           // Crozzle row for crozzle processing.
        private DataTable crozzleBox;
        private Crozzle crozzleGame;
        private int timeLeft = (5 * 60);
        public static Random random = new Random(); // Initiating random.

        #region constructors
        public Form1()
        {
            InitializeComponent();

            this.IsCorrect = false;
            this.processStart = CrozzleProcess.Creating;
            scoreContainer.Text = "NO SCORE";
            validContainer.Text = "VALIDATOR";
            timeContainer.Text = "5";

            openCrozzleToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            playToolStripMenuItem.Enabled = false;
            //dataGridView1.ColumnHeadersVisible = false;
        }
        #endregion

        #region properties collection
        public string CrozzleValids
        {
            get
            {
                return crzValids;
            }
            set
            {
                if (value == crzValids)
                    return;
                crzValids = value;
            }
        }

        public string theScore
        {
            get
            {
                return scr;
            }
            set
            {
                if (value == scr)
                    return;
                scr = value;
            }
        }

        public string wlFile
        {
            get
            {
                return disWL;
            }
            set
            {
                if (value == disWL)
                    return;
                disWL = value;
            }
        }

        public string CrozzleFile
        {
            get
            {
                return fCroz;
            }
            set
            {
                if (value == fCroz)
                    return;
                fCroz = value;
            }
        }

        private void RunProgram()
        {
            readCrozzle();
        }

        public CrozzleProcess processStart
        {
            get
            {
                return cProc;
            }
            set
            {
                if (value == cProc)
                    return;
                cProc = value;
            }
        }
        public CrozzleDifficulty crozzleDifficulty
        {
            get
            {
                return cd;
            }
            set
            {
                if (value == cd)
                    return;
                cd = value;
            }
        }

        public ObservableCollection<string> wordList
        {
            get
            {
                return wLcol;
            }
            set
            {
                if (value == wLcol)
                    return;
                wLcol = value;
            }
        }

        public ObservableCollection<RowList> Cols
        {
            get
            {
                return crzlRow;
            }
            set
            {
                if (value == crzlRow)
                    return;
                crzlRow = value;
            }
        }

        public DataTable CrozzleBoxArea
        {
            get
            {
                return crozzleBox;
            }
            set
            {
                if (value == crozzleBox)
                    return;
                crozzleBox = value;
            }
        }

        public bool IsCorrect
        {
            get
            {
                return isTrue;
            }
            set
            {
                if (value == isTrue)
                    return;
                isTrue = value;
            }
        }

        public string showTheCrozzle
        {
            get
            {
                return disCrz;
            }
            set
            {
                if (value == disCrz)
                    return;
                disCrz = value;
            }
        }

        // Row List
        public class RowList
        {

            private ObservableCollection<string> Rows;
            public ObservableCollection<string> Row
            {
                get
                {
                    return Rows;
                }
                set
                {
                    if (value == Rows)
                        return;
                    Rows = value;
                }
            }

            public RowList()
            {
                Row = new ObservableCollection<string>();
            }

        }

        // Position
        public class Position
        {
            public string Letter
            {
                get;
                set;
            }
            public int row
            {
                get;
                set;
            }
            public int column
            {
                get;
                set;
            }

            public Position()
            {

            }
        }

        // WordData
        public class WordData
        {
            public List<Position> wordPos { get; set; }
            public bool isAccurate { get; set; }
            public bool isLinked { get; set; }
            public WordData()
            {
                wordPos = new List<Position>();
            }

            public string PickWord()
            {
                string result = string.Empty;

                foreach (var letter in wordPos)
                {
                    result += letter.Letter;
                }

                return result;
            }
        }

        public class Crozzle
        {
            public string Word { get; set; }
            public bool IsExist { get; set; }
            public int totalIntersections { get; set; }
            public List<Position> blockFull { get; set; }
            public Crozzle()
            {
                blockFull = new List<Position>();
            }
        }
        #endregion
        
        /// <summary>
        /// This class contains every difficulty levels and
        /// each constraints for every class.
        /// It each class also validate the crozzle which suitable
        /// with the wordlist itself.
        /// </summary>
        #region check and validate difficulty level
        public void CheckDifficultyLevel()
        {
            // Check difficulty levels constraint.
            // Easy crozzle checker.
            if (crozzleDifficulty == CrozzleDifficulty.EASY)
            {
                // Scan word.
                var wordScanner = WordScanner();
                if (!wordScanner)
                {
                    MessageBox.Show("Wordlist is Invalid");
                    Console.WriteLine("Wordlist is Invalid");
                    LogFile("Wordlist is Invalid");

                    scoreContainer.Text = "0 - INVALID";
                    validContainer.Text = "INVALID";
                    return;
                }

                // Check letters intersection.
                var checkIntersection = IntersectingWordsScanner();
                if (!checkIntersection)
                {
                    MessageBox.Show("Wordlist is Invalid");
                    Console.WriteLine("Wordlist is Invalid");
                    LogFile("Wordlist is Invalid");

                    scoreContainer.Text = "0 - INVALID";
                    validContainer.Text = "INVALID";
                    return;
                }

                // Check word position constraint.
                var wordLoc = WordLocator();
                if (!wordLoc)
                {
                    MessageBox.Show("Wordlist is Invalid");
                    Console.WriteLine("Wordlist is Invalid");
                    LogFile("Wordlist is Invalid");

                    scoreContainer.Text = "0 - INVALID";
                    validContainer.Text = "INVALID";
                    return;
                }

                // If every constraint has been passed, compute the crozzle score.
                scoreContainer.Text = scoringSystem().ToString();
                validContainer.Text = "VALID";
            }

            // Medium checker.
            else if (crozzleDifficulty == CrozzleDifficulty.MEDIUM)
            {
                // Scan word.
                var wordScanner = WordScanner();
                if (!wordScanner)
                {
                    MessageBox.Show("Wordlist is Invalid");
                    Console.WriteLine("Wordlist is Invalid");
                    LogFile("Wordlist is Invalid");

                    scoreContainer.Text = "0 - INVALID";
                    validContainer.Text = "INVALID";
                    return;
                }

                // Check letter intersection.
                var intersectingLettersScanner = IntersectingWordsScanner();
                if (!intersectingLettersScanner)
                {
                    MessageBox.Show("Wordlist is Invalid");
                    Console.WriteLine("Wordlist is Invalid");
                    LogFile("Wordlist is Invalid");

                    scoreContainer.Text = "0 - INVALID";
                    validContainer.Text = "INVALID";
                    return;
                }

                // If every constraint has been passed, compute the crozzle score.
                scoreContainer.Text = scoringSystem().ToString();
                validContainer.Text = "VALID";
            }

            // Hard checker.
            else if (crozzleDifficulty == CrozzleDifficulty.HARD)
            {
                // Scan word.
                var wordScanner = WordScanner();
                if (!wordScanner)
                {
                    MessageBox.Show("Wordlist is Invalid");
                    Console.WriteLine("Wordlist is Invalid");
                    LogFile("Wordlist is Invalid");

                    scoreContainer.Text = "0 - INVALID";
                    validContainer.Text = "INVALID";
                    return;
                }

                // Hard intersection scanner.
                var hardIntersection = IntersectingScanner();
                if (!hardIntersection)
                {
                    MessageBox.Show("Wordlist is Invalid");
                    Console.WriteLine("Wordlist is Invalid");
                    LogFile("Wordlist is Invalid");

                    scoreContainer.Text = "0 - INVALID";
                    validContainer.Text = "INVALID";
                    return;
                }

                // If every constraint has been passed, compute the crozzle score.
                scoreContainer.Text = scoringSystem().ToString();
                validContainer.Text = "VALID";
            }

            // Extreme checker.
            else
            {
                // Scan words
                var wordScanner = WordScanner();
                if (!wordScanner)
                {
                    MessageBox.Show("Wordlist is Invalid");
                    Console.WriteLine("Wordlist is Invalid");
                    LogFile("Wordlist is Invalid");

                    scoreContainer.Text = "0 - INVALID";
                    validContainer.Text = "INVALID";
                    return;
                }

                // Extreme intersection scanner.
                var extremeIntersection = IntersectingScanner();
                if (!extremeIntersection)
                {
                    MessageBox.Show("Crozzle is Invalid");
                    Console.WriteLine("Crozzle is Invalid");
                    LogFile("Crozzle is Invalid");

                    scoreContainer.Text = "0 - INVALID";
                    validContainer.Text = "INVALID";
                    return;
                }

                // Word-Group connectivity scanner.
                var connectivity = WordGroupScanner();
                if (!connectivity)
                {
                    MessageBox.Show("Word-Group connectivity constraint breached.");
                    Console.WriteLine("Word-Group connectivity constraint breached.");
                    LogFile("Word-Group connectivity constraint breached.");

                    scoreContainer.Text = "0 - INVALID";
                    validContainer.Text = "INVALID";
                    return;
                }

                // If every constraint has been passed, compute the crozzle score.
                scoreContainer.Text = scoringSystem().ToString();
                validContainer.Text = "VALID";
            }
        }
        #endregion
        
        /// <summary>
        /// Get every word in the crozzle, 
        /// so the program can process it letter.
        /// It will always check if the block is empty so it can skip it
        /// and if it has a word on it, it will be saved inside the list.
        /// </summary>
        #region get crozzle words
        public void GetCrozzleWords()
        {
            // Scan and take word from the start to finish from the rows of crozzle.
            for (int a = 0; a < totalRows; a++)
            {
                bool go = false;
                WordData wordData = null;
                for (int b = 0; b < totalColumns; b++)
                {
                    if (b < totalColumns - 1)
                    {
                        if (!string.IsNullOrEmpty(Crz1[a, b]) && go == false)
                        {
                            go = true;
                            wordData = new WordData();
                            wordData.wordPos.Add(new Position() { Letter = Crz1[a, b], row = a, column = b });
                        }
                        else if (!string.IsNullOrEmpty(Crz1[a, b]) && go == true)
                        {
                            wordData.wordPos.Add(new Position() { Letter = Crz1[a, b], row = a, column = b });
                        }
                        else if (string.IsNullOrEmpty(Crz1[a, b]) && go == true)
                        {
                            go = false;
                            crzR.Add(wordData);
                            wordData = null;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Crz1[a, b]) && go == false)
                        {
                            go = true;
                            wordData = new WordData();
                            wordData.wordPos.Add(new Position() { Letter = Crz1[a, b], row = a, column = b });
                            crzR.Add(wordData);
                        }
                        else if (!string.IsNullOrEmpty(Crz1[a, b]) && go == true)
                        {
                            wordData.wordPos.Add(new Position() { Letter = Crz1[a, b], row = a, column = b });
                            crzR.Add(wordData);
                        }
                        else if (string.IsNullOrEmpty(Crz1[a, b]) && go == true)
                        {
                            go = false;
                            crzR.Add(wordData);
                            wordData = null;
                        }
                    }
                }
            }

            // Scan and take word from the start to finish from columns of crozzle.
            for (int b = 0; b < totalColumns; b++)
            {
                bool go = false;
                WordData wordData = null;
                for (int a = 0; a < totalRows; a++)
                {
                    if (a < totalRows - 1)
                    {
                        if (!string.IsNullOrEmpty(Crz1[a, b]) && go == false)
                        {
                            go = true;
                            wordData = new WordData();
                            wordData.wordPos.Add(new Position() { Letter = Crz1[a, b], row = a, column = b });
                        }
                        else if (!string.IsNullOrEmpty(Crz1[a, b]) && go == true)
                        {
                            wordData.wordPos.Add(new Position() { Letter = Crz1[a, b], row = a, column = b });
                        }
                        else if (string.IsNullOrEmpty(Crz1[a, b]) && go == true)
                        {
                            go = false;
                            crzC.Add(wordData);
                            wordData = null;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Crz1[a, b]) && go == false)
                        {
                            go = true;
                            wordData = new WordData();
                            wordData.wordPos.Add(new Position() { Letter = Crz1[a, b], row = a, column = b });
                            crzC.Add(wordData);
                        }
                        else if (!string.IsNullOrEmpty(Crz1[a, b]) && go == true)
                        {
                            wordData.wordPos.Add(new Position() { Letter = Crz1[a, b], row = a, column = b });
                            crzC.Add(wordData);
                        }
                        else if (string.IsNullOrEmpty(Crz1[a, b]) && go == true)
                        {
                            go = false;
                            crzC.Add(wordData);
                            wordData = null;
                        }
                    }
                }
            }
        }
        #endregion
        
        /// <summary>
        /// This region contains some validator.
        /// The most top of this region is a word intersection scanner
        /// for HARD & EXTREME.
        /// Intersection conditions should be min 1. 
        /// and no max.
        /// </summary>
        /// <returns>intersection validity</returns>
        #region validators for crozzle
        public bool IntersectingScanner()
        {
            //horizontal and vertical for hard and extreme
            foreach (var rowWords in crzR.Where(a => a.isAccurate == true).ToList())
            {
                int res = 0;

                foreach (var word in rowWords.wordPos)
                {
                    if (cSc[word.row, word.column])
                        res++;
                }

                if (res > 1)
                {
                    return true;
                }
            }

            foreach (var columnWords in crzC.Where(a => a.isAccurate == true).ToList())
            {
                int res = 0;

                foreach (var word in columnWords.wordPos)
                {
                    if (rSc[word.row, word.column])
                        res++;
                }

                if (res > 1)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Intersecting words scanner for EASY & MEDIUM.
        /// Intersection conditions should be min 1 and max 2.
        /// </summary>
        /// <returns>Easy & Medium validity</returns>
        public bool IntersectingWordsScanner()
        {
            foreach (var rowWords in crzR.Where(a => a.isAccurate == true).ToList())
            {
                int res = 0;
                foreach (var word in rowWords.wordPos)
                {
                    if (cSc[word.row, word.column])
                        res++;
                }

                if (res > 2)
                {
                    return false;
                }
            }

            foreach (var columnWords in crzC.Where(a => a.isAccurate == true).ToList())
            {
                int res = 0;
                foreach (var word in columnWords.wordPos)
                {
                    if (rSc[word.row, word.column])
                        res++;
                }

                if (res > 2)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check Extreme Word-Group connectivity.
        /// This class only specific for EXTREME difficulty.
        /// </summary>
        /// <param name="wordData">the wordlist data</param>
        /// <param name="connected">check the connection</param>
        public void WordGroupConnection(WordData wordData, bool connected)
        {
            if (connected)
            {
                wordData.isLinked = true;
                var newList = new List<WordData>();
                foreach (var group in wordData.wordPos)
                {
                    foreach (var colGroup in crzC.Where(a => a.isAccurate && !a.isLinked).ToList())
                    {
                        var check = colGroup.wordPos.Where(a => a.column == group.column && a.row == group.row).FirstOrDefault();
                        if (check != null)
                            newList.Add(colGroup);
                    }
                }

                foreach (var word in newList)
                    WordGroupConnection(word, false);
            }
            else
            {
                wordData.isLinked = true;
                var newList = new List<WordData>();
                foreach (var hc in wordData.wordPos)
                {
                    foreach (var rowGroup in crzR.Where(a => a.isAccurate && !a.isLinked).ToList())
                    {
                        var check = rowGroup.wordPos.Where(a => a.column == hc.column && a.row == hc.row).FirstOrDefault();
                        if (check != null)
                            newList.Add(rowGroup);
                    }
                }

                foreach (var list in newList)
                    WordGroupConnection(list, true);
            }
        }

        /// <summary>
        /// Check Word-Group connectivity.
        /// if there's one or more group in this
        /// the boolean will return invalid.
        /// </summary>
        /// <returns>word group validity for crozzle</returns>
        public bool WordGroupScanner()
        {
            // Check crozzle and wordlist connection.
            var croz = crzR[0];
            WordGroupConnection(croz, true);

            int validity = crzR.Where(a => a.isAccurate).Count() + crzC.Where(a => a.isAccurate).Count();
            int totalConnection = crzR.Where(a => a.isAccurate && a.isLinked).Count() + crzC.Where(a => a.isAccurate && a.isLinked).Count();

            if (totalConnection == validity)
                return true;
            else
                return false;
        }

        /// <summary>
        /// class will validate the word in the crozzle.
        /// it will get every letter both on rows and columns,
        /// and check the validation.
        /// </summary>
        /// <returns>word scanner validity for crozzle</returns>
        public bool WordScanner()
        {
            //set boolean variable for validating
            rSc = new bool[totalRows, totalColumns];
            cSc = new bool[totalRows, totalColumns];

            foreach (var row in crzR)
            {
                if (this.wordList.Contains(row.PickWord()))
                {
                    foreach (var letter in row.wordPos)
                    {
                        rSc[letter.row, letter.column] = true;
                    }
                    row.isAccurate = true;
                }
                else
                {
                    foreach (var letter in row.wordPos)
                    {
                        rSc[letter.row, letter.column] = false;
                    }
                    row.isAccurate = false;
                }
            }

            foreach (var columns in crzC)
            {
                if (this.wordList.Contains(columns.PickWord()))
                {
                    foreach (var letter in columns.wordPos)
                    {
                        cSc[letter.row, letter.column] = true;
                    }
                    columns.isAccurate = true;
                }
                else
                {
                    foreach (var letter in columns.wordPos)
                    {
                        cSc[letter.row, letter.column] = false;
                    }
                    columns.isAccurate = false;
                }
            }

            // Scan the letter once again and check the validity.
            int totalErrors = 0;
            for (int a = 0; a < totalRows; a++)
                for (int b = 0; b < totalColumns; b++)
                    if (!string.IsNullOrEmpty(Crz1[a, b]))
                        if (!(cSc[a, b] == true || rSc[a, b] == true))
                            totalErrors++;

            if (totalErrors > 0)
                return false;
            else
                return true;
        }
        #endregion
        
        /// <summary>
        /// The scoring system for crozzle
        /// which has different scoring system for each difficulty level.
        /// </summary>
        /// <returns>the score which going to be showed on interface</returns>
        #region scoring system
        public int scoringSystem()
        {
            int result = 0;

            // Get the proper score from certain difficulty.
            if (crozzleDifficulty == CrozzleDifficulty.EASY)
                result = ComputeEasy();
            else if (crozzleDifficulty == CrozzleDifficulty.MEDIUM)
                result = ComputeMedium();
            else if (crozzleDifficulty == CrozzleDifficulty.HARD)
                result = ComputeHard();
            else if (crozzleDifficulty == CrozzleDifficulty.EXTREME)
                result = ComputeExtreme();

            return result;
        }

        public int ComputeEasy()
        {
            int score = 0;
            for (int a = 0; a < totalRows; a++)
                for (int b = 0; b < totalColumns; b++)
                    if (!string.IsNullOrEmpty(Crz1[a, b]))
                        score++;

            return score;
        }

        public int ComputeMedium()
        {
            int score = 0;
            string scoreList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int a = 0; a < totalRows; a++)
                for (int b = 0; b < totalColumns; b++)
                    if (!string.IsNullOrEmpty(Crz1[a, b]))
                    {
                        int system = scoreList.IndexOf(Crz1[a, b][0]) + 1;
                        score += system;
                    }

            return score;
        }

        public int ComputeHard()
        {
            int score = 0;
            string scoreList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int a = 0; a < totalRows; a++)
                for (int b = 0; b < totalColumns; b++)
                    if (!string.IsNullOrEmpty(Crz1[a, b]))
                    {
                        int system = scoreList.IndexOf(Crz1[a, b][0]) + 1;
                        score += system;
                    }

            score += 10 * crzR.Count();
            score += 10 * crzC.Count();

            return score;
        }

        public int ComputeExtreme()
        {
            int score = 0;
            for (int a = 0; a < totalRows; a++)
                for (int b = 0; b < totalColumns; b++)

                    if (!string.IsNullOrEmpty(Crz1[a, b]))
                    {
                        if (Crz1[a, b] == "A" || Crz1[a, b] == "E" 
                            || Crz1[a, b] == "I" || Crz1[a, b] == "O" 
                            || Crz1[a, b] == "U")
                            score += 1;
                        else if (Crz1[a, b] == "B" || Crz1[a, b] == "C" 
                            || Crz1[a, b] == "D" 
                            || Crz1[a, b] == "F" || Crz1[a, b] == "G")
                            score += 2;
                        else if (Crz1[a, b] == "H" || Crz1[a, b] == "J" 
                            || Crz1[a, b] == "K" || Crz1[a, b] == "L" 
                            || Crz1[a, b] == "M")
                            score += 4;
                        else if (Crz1[a, b] == "N" || Crz1[a, b] == "P" 
                            || Crz1[a, b] == "Q" || Crz1[a, b] == "R")
                            score += 8;
                        else if (Crz1[a, b] == "S" || Crz1[a, b] == "T" 
                            || Crz1[a, b] == "V")
                            score += 16;
                        else if (Crz1[a, b] == "W" || Crz1[a, b] == "X" 
                            || Crz1[a, b] == "Y")
                            score += 32;
                        else if (Crz1[a, b] == "Z")
                            score += 64;
                    }

            score += 10 * crzR.Count();
            score += 10 * crzC.Count();

            return score;
        }
        #endregion
        
        /// <summary>
        /// A lot of constraints to make sure the crozzle
        /// is fulfil the requirements.
        /// </summary>
        #region collection of constraints
        // Row constraint, give space to next word.
        public bool rowValidator(int i)
        {
            if (i >= 0 && i <= totalRows - 1)
                return true;
            else
                return false;
        }

        // Column constraint, give space to next word.
        public bool columnValidator(int i)
        {
            if (i >= 0 && i <= totalColumns - 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Row constraint.
        /// Row limitation for every row word located.
        /// </summary>
        /// <param name="row">row</param>
        /// <param name="column">column</param>
        /// <param name="validPointer">the pointer to get correct constraint</param>
        /// <returns></returns>
        public bool RowLimitation(int row, int column, int validPointer)
        {
            if (validPointer == 1)
            {
                int rowMax = row + 1;
                int rowMin = row - 1;
                int columnMin = column - 1;

                if (rowValidator(rowMax))
                    if (rThere[rowMax, column])
                        return false;


                if (rowValidator(rowMin))
                    if (rThere[rowMin, column])
                        return false;


                if (columnValidator(columnMin))
                {
                    if (rowValidator(rowMax))
                        if (rThere[rowMax, columnMin])
                            return false;


                    if (rowValidator(rowMin))
                        if (rThere[rowMin, columnMin])
                            return false;
                }
            }
            else if (validPointer == 2)
            {
                int rowMax = row + 1;
                int rowMin = row - 1;
                int columnMax = column + 1;

                if (rowValidator(rowMax))
                    if (rThere[rowMax, column])
                        return false;

                if (rowValidator(rowMin))
                    if (rThere[rowMin, column])
                        return false;

                if (columnValidator(columnMax))
                {
                    if (rowValidator(rowMax))
                        if (rThere[rowMax, columnMax])
                            return false;

                    if (rowValidator(rowMin))
                        if (rThere[rowMin, columnMax])
                            return false;
                }
            }
            else
            {
                int rowMax = row + 1;
                int rowMin = row - 1;

                if (rowValidator(rowMax))
                    if (rThere[rowMax, column])
                        return false;

                if (rowValidator(rowMin))
                    if (rThere[rowMin, column])
                        return false;
            }
            return true;
        }

        /// <summary>
        /// Column constraint.
        /// Column limitation for every column word located.
        /// </summary>
        /// <param name="row">row</param>
        /// <param name="column">column</param>
        /// <param name="validPointer">pointer to revise the code to right function</param>
        /// <returns></returns>
        public bool ColumnLimitation(int row, int column, int validPointer)
        {
            if (validPointer == 1)
            {
                int columnMax = column + 1;
                int columnMin = column - 1;
                int rowSpace = row - 1;
                int columnSpaceMax = column + 1;
                int columnSpaceMin = column - 1;

                if (columnValidator(columnMax))
                    if (cSc[row, columnMax])
                        return false;

                if (columnValidator(columnMin))
                    if (cSc[row, columnMin])
                        return false;

                if (rowValidator(rowSpace))
                {
                    if (columnValidator(columnSpaceMax))
                        if (cSc[rowSpace, columnSpaceMax])
                            return false;

                    if (columnValidator(columnSpaceMin))
                        if (cSc[rowSpace, columnSpaceMin])
                            return false;
                }
            }
            else if (validPointer == 2)
            {
                int columnMax = column + 1;
                int columnMin = column - 1;
                int rowSpace = row + 1;
                int columnSpaceMax = column + 1;
                int columnSpaceMin = column - 1;

                if (columnValidator(columnMax))
                    if (cSc[row, columnMax])
                        return false;

                if (columnValidator(columnMin))
                    if (cSc[row, columnMin])
                        return false;

                if (rowValidator(rowSpace))
                {
                    if (columnValidator(columnSpaceMax))
                        if (cSc[rowSpace, columnSpaceMax])
                            return false;

                    if (columnValidator(columnSpaceMin))
                        if (cSc[rowSpace, columnSpaceMin])
                            return false;
                }
            }
            else
            {
                int columnMax = column + 1;
                int columnMin = column - 1;

                if (columnValidator(columnMax))
                    if (cSc[row, columnMax])
                        return false;

                if (columnValidator(columnMin))
                    if (cSc[row, columnMin])
                        return false;
            }
            return true;
        }

        /// <summary>
        /// Place and word locator, method for placing the words to crozzle
        /// It will place the words according to the constraint.
        /// </summary>
        /// <returns>boolean condition for validating crozzle</returns>
        public bool WordLocator()
        {
            bool[,] has = new bool[totalRows, totalColumns];
            foreach (var rows in crzR.Where(a => a.isAccurate).ToList())
            {
                foreach (var letter in rows.wordPos)
                {
                    if (letter == rows.wordPos.First())
                    {
                        int rowMax = letter.row + 1;
                        int rowMin = letter.row - 1;
                        int columnMin = letter.column - 1;

                        has[letter.row, letter.column] = true;

                        if (rowValidator(rowMax))
                            if (has[rowMax, letter.column])
                                return false;

                        if (rowValidator(rowMin))
                            if (has[rowMin, letter.column])
                                return false;

                        if (columnValidator(columnMin))
                        {
                            if (rowValidator(rowMax))
                                if (has[rowMax, columnMin])
                                    return false;

                            if (rowValidator(rowMin))
                                if (has[rowMin, columnMin])
                                    return false;
                        }
                    }
                    else if (letter == rows.wordPos.Last())
                    {
                        int rowMax = letter.row + 1;
                        int rowMin = letter.row - 1;
                        int columnMax = letter.column + 1;

                        has[letter.row, letter.column] = true;

                        if (rowValidator(rowMax))
                            if (has[rowMax, letter.column])
                                return false;

                        if (rowValidator(rowMin))
                            if (has[rowMin, letter.column])
                                return false;

                        if (columnValidator(columnMax))
                        {
                            if (rowValidator(rowMax))
                                if (has[rowMax, columnMax])
                                    return false;

                            if (rowValidator(rowMin))
                                if (has[rowMin, columnMax])
                                    return false;
                        }
                    }
                    else
                    {
                        int rowMax = letter.row + 1;
                        int rowMin = letter.row - 1;

                        has[letter.row, letter.column] = true;

                        if (rowValidator(rowMax))
                            if (has[rowMax, letter.column])
                                return false;

                        if (rowValidator(rowMin))
                            if (has[rowMin, letter.column])
                                return false;
                    }
                }
            }

            bool[,] hasNew = new bool[totalRows, totalColumns];
            foreach (var columns in crzC.Where(a => a.isAccurate).ToList())
            {
                foreach (var letter in columns.wordPos)
                {
                    if (letter == columns.wordPos.First())
                    {
                        int columnMax = letter.column + 1;
                        int columnMin = letter.column - 1;
                        int rowSpace = letter.row - 1;
                        int columnSpaceMax = letter.column + 1;
                        int columnSpaceMin = letter.column - 1;

                        hasNew[letter.row, letter.column] = true;

                        if (columnValidator(columnMax))
                            if (hasNew[letter.row, columnMax])
                                return false;

                        if (columnValidator(columnMin))
                            if (hasNew[letter.row, columnMin])
                                return false;

                        if (rowValidator(rowSpace))
                        {
                            if (columnValidator(columnSpaceMax))
                                if (hasNew[rowSpace, columnSpaceMax])
                                    return false;

                            if (columnValidator(columnSpaceMin))
                                if (hasNew[rowSpace, columnSpaceMin])
                                    return false;
                        }
                    }
                    else if (letter == columns.wordPos.Last())
                    {
                        int columnMax = letter.column + 1;
                        int columnMin = letter.column - 1;
                        int rowSpace = letter.row + 1;
                        int columnSpaceMax = letter.column + 1;
                        int columnSpaceMin = letter.column - 1;

                        hasNew[letter.row, letter.column] = true;

                        if (columnValidator(columnMax))
                            if (hasNew[letter.row, columnMax])
                                return false;

                        if (columnValidator(columnMin))
                            if (hasNew[letter.row, columnMin])
                                return false;

                        if (rowValidator(rowSpace))
                        {
                            if (columnValidator(columnSpaceMax))
                                if (hasNew[rowSpace, columnSpaceMax])
                                    return false;

                            if (columnValidator(columnSpaceMin))
                                if (hasNew[rowSpace, columnSpaceMin])
                                    return false;
                        }
                    }
                    else
                    {
                        int columnMax = letter.column + 1;
                        int columnMin = letter.column - 1;

                        hasNew[letter.row, letter.column] = true;

                        if (columnValidator(columnMax))
                            if (hasNew[letter.row, columnMax])
                                return false;

                        if (columnValidator(columnMin))
                            if (hasNew[letter.row, columnMin])
                                return false;
                    }
                }
            }

            return true;
        }

        #endregion
        
        /// <summary>
        /// create the crozzle representation, and it will loads 
        /// all the words that have been located before.
        /// Stopwatch will count the time and has been set for 5 minutes
        /// so the algorithm will find the best crozzle given the time.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">BackgroundWorker handler</param>
        #region creating crozzle representation
        public void CreateCrozzle(object sender, DoWorkEventArgs e)
        {
            // Set time limit using stopwatch.
            var stopWatch = new Stopwatch();
            int time = int.Parse(this.timeContainer.Text) * 60000;
            stopWatch.Start();

            int tick = 0;
            LogFile("Start creating crozzle");
            bool crozzleValid = true;

            do
            {
                for (int a = 0; a < totalRows; a++)
                {
                    for (int b = 0; b < totalColumns; b++)
                    {
                        for (int wList = 0; wList < wordList.Count; wList++)
                        {
                            for (int representation = 0; representation < 2; representation++)
                            {
                                if (representation == 0)
                                    MakeCrozzle(a, b, true, wList);

                                if (representation == 1)
                                    MakeCrozzle(a, b, false, wList);
                                crzR = new List<WordData>();
                                crzC = new List<WordData>();
                                GetCrozzleWords();

                                // Give it tick so the stopwatch working.
                                tick = (int)stopWatch.ElapsedMilliseconds;
                                var percent = (tick * 1.0 / time * 100);
                                if (percent > 100)
                                    percent = 100;

                                computeCrozzle(this, new LongProcessEventArgs((int)percent));
                                BackgroundWorker worker1 = sender as BackgroundWorker;
                                if (worker1.CancellationPending)
                                {
                                    e.Cancel = true;
                                    break;
                                }

                                if (tick > time)
                                    return;

                                // Difficulty level validation.
                                // This section is for EASY.
                                if (crozzleDifficulty == CrozzleDifficulty.EASY)
                                {
                                    var wordScanner = WordScanner();
                                    if (!wordScanner)
                                    {
                                        crozzleValid = false;
                                        continue;
                                    }
                                    else
                                        crozzleValid = true;

                                    var intersectionWordsScanner = IntersectingWordsScanner();
                                    if (!intersectionWordsScanner)
                                    {
                                        crozzleValid = false;
                                        continue;
                                    }
                                    else
                                        crozzleValid = true;

                                    var wordLocator = WordLocator();
                                    if (!wordLocator)
                                    {
                                        crozzleValid = false;
                                        continue;
                                    }
                                    else
                                        crozzleValid = true;

                                    var score = scoringSystem();
                                    if (score > tempSc)
                                    {
                                        Crz2 = Crz1;
                                        tempSc = score;

                                        crzLsT = crzLs;
                                        ijT = ij;
                                    }
                                }

                                // This section is for MEDIUM.
                                else if (crozzleDifficulty == CrozzleDifficulty.MEDIUM)
                                {
                                    var wordScanner = WordScanner();
                                    if (!wordScanner)
                                    {
                                        crozzleValid = false;
                                        continue;
                                    }
                                    else
                                        crozzleValid = true;

                                    var intersectingWordsScanner = IntersectingWordsScanner();
                                    if (!intersectingWordsScanner)
                                    {
                                        crozzleValid = false;
                                        continue;
                                    }
                                    else
                                        crozzleValid = true;

                                    var score = scoringSystem();
                                    if (score > tempSc)
                                    {
                                        Crz2 = Crz1;
                                        tempSc = score;

                                        crzLsT = crzLs;
                                        ijT = ij;
                                    }
                                }

                                // This section is for HARD.
                                else if (crozzleDifficulty == CrozzleDifficulty.HARD)
                                {
                                    var wordScanner = WordScanner();
                                    if (!wordScanner)
                                    {
                                        crozzleValid = false;
                                        continue;
                                    }
                                    else
                                        crozzleValid = true;

                                    var intersectionScanner = IntersectingScanner();
                                    if (!intersectionScanner)
                                    {
                                        crozzleValid = false;
                                        continue;
                                    }
                                    else
                                        crozzleValid = true;

                                    var score = scoringSystem();
                                    if (score > tempSc)
                                    {
                                        Crz2 = Crz1;
                                        tempSc = score;

                                        crzLsT = crzLs;
                                        ijT = ij;
                                    }
                                }

                                // This section is for EXTREME.
                                else
                                {
                                    var wordScanner = WordScanner();
                                    if (!wordScanner)
                                    {
                                        crozzleValid = false;
                                        continue;
                                    }
                                    else
                                        crozzleValid = true;

                                    var intersectionScanner = IntersectingScanner();
                                    if (!intersectionScanner)
                                    {
                                        crozzleValid = false;
                                        continue;
                                    }
                                    else
                                        crozzleValid = true;

                                    var connectivity = WordGroupScanner();
                                    if (!connectivity)
                                    {
                                        crozzleValid = false;
                                        continue;
                                    }
                                    else
                                        crozzleValid = true;

                                    var score = scoringSystem();
                                    if (score > tempSc)
                                    {
                                        Crz2 = Crz1;
                                        tempSc = score;
                                        crzLsT = crzLs;
                                        ijT = ij;
                                    }

                                    if (!(tick < time))
                                        return;
                                }
                            }
                        }
                    }
                }
            }

            // loop until the tick is = 5 minutes.
            while (tick < time);
        }
        #endregion
       
        /// <summary>
        /// Create the crozzle field.
        /// </summary>
        /// <param name="row">row integer</param>
        /// <param name="column">column integer</param>
        /// <param name="seq">seq boolean for conditional checking</param>
        /// <param name="wordTotal">total words inside the crozzle</param>
        #region crozzle run to make
        public void MakeCrozzle(int row, int column, bool seq, int wordTotal)
        {
            crzLs = new List<Crozzle>();
            ij = new List<Position>();
            rThere = new bool[totalRows, totalColumns];
            cSc = new bool[totalRows, totalColumns];
            Crz1 = new string[totalRows, totalColumns];
            foreach (var word in wordList)
            {
                crzLs.Add(new Crozzle() { Word = word, IsExist = false });
            }

            bool valid = false;
            wordTotal = random.Next(0, crzLs.Count() - 1);
            if (!seq)
                if (row + crzLs[wordTotal].Word.Count() <= totalRows - 1)
                    valid = true;

            if (seq)
                if (column + crzLs[wordTotal].Word.Count() <= totalColumns - 1)
                    valid = true;

            if (!valid)
                return;

            // Put each word in the certain row and column position.
            int total = 0;
            foreach (var crozList in crzLs[wordTotal].Word)
            {
                if (!seq)
                {
                    var pos = new Position() { row = row + total, column = column, Letter = crozList.ToString() };
                    ij.Add(pos);
                    Crz1[row + total, column] = crozList.ToString();
                    cSc[row + total, column] = true;
                    crzLs[wordTotal].blockFull.Add(pos);
                }
                else
                {
                    var pos = new Position() { row = row, column = column + total, Letter = crozList.ToString() };
                    ij.Add(pos);
                    Crz1[row, column + total] = crozList.ToString();
                    rThere[row, column + total] = true;
                    crzLs[wordTotal].blockFull.Add(pos);
                }
                total++;
            }

            crzLs[wordTotal].IsExist = true;

            // Load the words one-by-one to the crozzle
            for (int a = 0; a < 100; a++)
            {
                LoadWordsToCrozzle();
            }
        }
        #endregion
      
        /// <summary>
        /// Fill the crozzle with the words from the crozzle list.
        /// Basically, it will prints every recorded words in the crozzle list.
        /// Show it on data grid view.
        /// </summary>
        #region fill crozzle with words
        public void FillCrozzleWithWords()
        {
            Cols = new ObservableCollection<RowList>();
            scoreContainer.Text = tempSc.ToString();
            validContainer.Text = "Valid";

            // Fill the crozzle with words.
            for (int v = 0; v < totalRows; v++)
            {
                var cRows = new RowList();
                for (int w = 0; w < totalColumns; w++)
                {
                    var has = ijT.Where(a => a.column == w && a.row == v).FirstOrDefault();
                    if (has != null)
                    {
                        cRows.Row.Add(has.Letter);
                    }
                    else
                    {
                        cRows.Row.Add(string.Empty);
                    }
                }

                Cols.Add(cRows);
            }

            dGV1.Columns.Clear();

            for (int a = 0; a < totalColumns; a++)
            {
                dGV1.Columns.Add(a.ToString(), a.ToString());
                dGV1.Columns[a].Width = 30;
            }

            for (int a = 0; a < totalRows; a++)
            {
                var dgvRow = new DataGridViewRow();

                for (int b = 0; b < totalColumns; b++)
                {
                    dgvRow.Cells.Add(new DataGridViewTextBoxCell()
                    {
                        Value = Crz2[a, b]
                    });
                }

                dGV1.Rows.Add(dgvRow);
            }

            dGV1.Refresh();
        }
        #endregion
      
        /// <summary>
        /// This class will read and validate the wordlist.
        /// Using StreamReader to read the file, give some separators.
        /// Save the wordlist into a list for later making the crozzle.
        /// Wordlist also will be displayed in the wordlist interface.
        /// </summary>
        #region read and validate wordlist
        public void readAndValidateWordlist()
        {
            this.wordList = new ObservableCollection<string>();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Set up the file.
                string stringLine;
                string[] gameProperties;
                string[] separators = { " ", ",", "~", "\t" };
                String fileName = openFileDialog1.FileName;
                int result;

                // Set log writer.
                logWriter = Path.GetDirectoryName(fileName);
                logWriter += "\\Assignment 2 - Log.txt";

                // Start writing log.
                Console.WriteLine("Start processing wordlist file");
                LogFile("Start processing wordlist file");

                // Start validate wordlist.
                using (StreamReader streamReader = new StreamReader(fileName))
                {
                    while ((stringLine = streamReader.ReadLine()) != null)
                    {
                        // Check whether the block is empty.
                        if (stringLine == string.Empty)
                        {
                            Console.WriteLine("Empty line detected");
                            LogFile("Empty line detected");
                            continue;
                        }

                        gameProperties = stringLine.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        try
                        {
                            // Check and parse the block.
                            if (Int32.TryParse(gameProperties[0], out result))
                            {
                                totalWords = (int)result;
                            }
                            else
                            {
                                Console.WriteLine("Error: not a correct integer");
                                LogFile("Error: not a correct integer");
                                return;
                            }

                            // Check and parse the block.
                            if (Int32.TryParse(gameProperties[1], out result))
                            {
                                totalRows = (int)result;
                            }
                            else
                            {
                                Console.WriteLine("Error: not a correct integer");
                                LogFile("Error: not a correct integer");
                                return;
                            }

                            // Check and parse the block.                         
                            if (Int32.TryParse(gameProperties[2], out result))
                            {
                                totalColumns = (int)result;
                            }
                            else
                            {
                                Console.WriteLine("Error: not a correct integer");
                                LogFile("Error: not a correct integer");
                                return;
                            }

                            if (gameProperties[3] == "EASY")
                                this.crozzleDifficulty = CrozzleDifficulty.EASY;
                            else if (gameProperties[3] == "MEDIUM")
                                this.crozzleDifficulty = CrozzleDifficulty.MEDIUM;
                            else if (gameProperties[3] == "HARD")
                                this.crozzleDifficulty = CrozzleDifficulty.HARD;
                            else if (gameProperties[3] == "EXTREME")
                                this.crozzleDifficulty = CrozzleDifficulty.EXTREME;
                            else
                            {
                                Console.WriteLine("Error: not a difficulty level.");
                                LogFile("Error: not a difficulty level.");
                                return;
                            }
                        }
                        catch
                        {
                            // Notify and write a log about invalid wordlist.
                            MessageBox.Show("Wordlist is Invalid");
                            Console.WriteLine("Wordlist is Invalid");
                            LogFile("Wordlist is Invalid");
                            wordList.Clear();
                            return;
                        }

                        try
                        {
                            // Read and store every word in wordlist.
                            for (int index = 4; index < totalWords + 4; index++)
                            {
                                wordList.Add(gameProperties[index]);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("No wordlist data found.");
                            Console.WriteLine("No wordlist data found.");
                            LogFile("No wordlist data found.");
                            wordList.Clear();
                            return;
                        }
                    }
                }

                // Check the wordlist.
                if (wordList.Count() == 0)
                {
                    MessageBox.Show("Wrong file!");
                    return;
                }

                levelContainer.Text = this.crozzleDifficulty.ToString();
                scoreContainer.Text = "0";

                MessageBox.Show("Wordlist is valid.");

                wordListContainer.DataSource = wordList.ToList();

                Crz1 = new string[totalRows, totalColumns];
                Cols = new ObservableCollection<RowList>();
            }
            else
                MessageBox.Show("No wordlist, please read a correct file.");
        }
        #endregion
  
        /// <summary>
        /// This class will read the crozzle and load it
        /// to the datagridview.
        /// Process the file and validate it.
        /// </summary>
        #region read and validate crozzle
        public void readCrozzle()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Set up reader.
                string stringLine;
                string[] separators = { " " };
                String fileName = openFileDialog1.FileName;
                int res = 0;

                // Start writing log.
                Console.WriteLine("Start processing crozzle.");
                LogFile("Start processing crozzle.");

                using (StreamReader reader = new StreamReader(fileName))
                {
                    while ((stringLine = reader.ReadLine()) != null)
                    {
                        // Check whether the crozzle is empty or not.
                        if (stringLine == string.Empty)
                        {
                            Console.WriteLine("Empty line detected");
                            LogFile("Empty line detected");
                            continue;
                        }

                        // Read Crozzle file.
                        var crozzleLoc = stringLine.ToCharArray();
                        var rowCell = new RowList();

                        for (int a = 0; a < crozzleLoc.Count(); a++)
                        {
                            try
                            {
                                Crz1[res, a] = crozzleLoc[a].ToString();
                            }
                            catch
                            {
                                Crz1[res, a] = "-";
                            }

                            rowCell.Row.Add(crozzleLoc[a].ToString());
                        }
                        Cols.Add(rowCell);
                        res++;
                    }
                }

                MessageBox.Show("Crozzle is valid!");

                // Clear and refresh dgv.
                dGV1.Columns.Clear();
                dGV1.Refresh();

                // Create crozzle grids.
                crzR = new List<WordData>();
                crzC = new List<WordData>();

                scoreContainer.Text = "0";

                // Put words to data grid view.
                for (int a = 0; a < totalColumns; a++)
                {
                    dGV1.Columns.Add(a.ToString(), a.ToString());
                    dGV1.Columns[a].Width = 30;
                }

                for (int a = 0; a < totalRows; a++)
                {
                    var row = new DataGridViewRow();

                    for (int b = 0; b < totalColumns; b++)
                    {
                        if (Crz1[a, b] == " ")
                        {
                            Crz1[a, b] = string.Empty;
                        }

                        row.Cells.Add(new DataGridViewTextBoxCell()
                        {
                            Value = Crz1[a, b]
                        });
                    }
                    dGV1.Rows.Add(row);
                }

                GetCrozzleWords();
                CheckDifficultyLevel();
            }
            else
            {
                MessageBox.Show("Crozzle file is missing!");
            }

        }
        #endregion
       
        /// <summary>
        /// Basic constructor for ReadAndValidateWordlist and ReadCrozzle
        /// </summary>
        #region wordlist and crozzle reader constructors
        private void ReadWordlist()
        {
            readAndValidateWordlist();
        }

        private void ReadCrozzle()
        {
            readCrozzle();
        }

        #endregion
   
        /// <summary>
        /// Log writer for writing the log
        /// checks if the logWriter exist, if yes create new writer.
        /// </summary>
        /// <param name="exceptionErrorLog">The parameter will be ready for writing errors and exceptions.</param>
        #region log writer
        public void LogFile(string exceptionErrorLog)
        {
            if (!File.Exists(logWriter))
                writer = new StreamWriter(logWriter);
            else
                writer = File.AppendText(logWriter);

            // Write and close writer.
            writer.WriteLine(exceptionErrorLog);
            writer.Close();
        }
        #endregion
  
        /// <summary>
        /// Background worker will handle anything that has long process event
        /// for example to find the crozzle takes 5 minutes.
        /// Also handles progress bar and work event.
        /// </summary>
        #region background worker
        public class LongProcessEventArgs : EventArgs
        {
            public int Percent
            {
                get;
                set;
            }
            public LongProcessEventArgs(int percentage)
            {
                this.Percent = percentage;
            }
        }
        private void work(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if ((worker.CancellationPending == true))
            {
                e.Cancel = true;
            }
            else
            {
                BackgroundWorker obj = sender as BackgroundWorker;
                CreateCrozzle(obj, e);
            }
        }
        private void workerFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                MessageBox.Show("Generate crozzle stopped!");
            }
            else if (!(e.Error == null))
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                FillCrozzleWithWords();
                MessageBox.Show("Generate crozzle finished!");
                Console.WriteLine("Generate crozzle finished!");
                LogFile("Generate crozzle finished!");
            }
        }
        private void processHandler(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }
        private BackgroundWorker worker = new BackgroundWorker();
        #endregion

        /// <summary>
        /// Enumerator collections, which makes everything collected in one place
        /// then freely access it without hassle.
        /// </summary>
        #region collections
        public enum CrozzleProcess
        {
            Validating,
            Creating,
        }
        public enum CrozzleDifficulty
        {
            EASY,
            MEDIUM,
            HARD,
            EXTREME,
        }
        public IEnumerable<CrozzleProcess> OptionSources
        {
            get
            {
                return Enum.GetValues(typeof(CrozzleProcess)).Cast<CrozzleProcess>();
            }
        }

        public IEnumerable<CrozzleDifficulty> LevelSources
        {
            get
            {
                return Enum.GetValues(typeof(CrozzleDifficulty)).Cast<CrozzleDifficulty>();
            }
        }
        #endregion

        /// <summary>
        /// Loads the words
        /// This class will continuously loop for 5 minutes non-stop.
        /// Find the best score for generated crozzle.
        /// </summary>
        #region load words to the crozzle
        public void LoadWordsToCrozzle()
        {
            bool valid = false;
            bool seq = false;
            int wordTotal = 0;
            int pointer = 0;
            var crozzleGame = new Crozzle();

            // Start making crozzle possibilities from wordlist.
            do
            {
                seq = random.NextDouble() >= 0.5;
                wordTotal = ij.Count();
                pointer = random.Next(0, wordTotal - 1);
                crozzleGame = crzLs.Where(a => a.blockFull.Contains(ij[pointer])).FirstOrDefault();

                // Scan whether word is there or not.
                if (crozzleGame == null)
                    continue;

                // Scan for EASY & MEDIUM intersections.
                if (crozzleDifficulty == CrozzleDifficulty.EASY || crozzleDifficulty == CrozzleDifficulty.MEDIUM)
                    if (crozzleGame.totalIntersections >= 2)
                        continue;

                // Scan the position of a word sequence.
                if (!seq)
                {
                    var tempColumnWords = crzLs.Where(a => !a.IsExist & a.Word.Contains(ij[pointer].Letter)).ToList();
                    var columnWordsValidator = new List<Crozzle>();

                    foreach (var tempColumn in tempColumnWords)
                    {
                        var columnPosition1 = tempColumn.Word.IndexOf(ij[pointer].Letter);
                        var columnPosition2 = tempColumn.Word.Count() - columnPosition1 - 1;
                        bool available = true;

                        if (ij[pointer].row - columnPosition1 >= 0 && ij[pointer].row + columnPosition2 <= totalRows - 1)
                        {
                            for (int v = 0; v < tempColumn.Word.Count(); v++)
                            {
                                if (crozzleDifficulty == CrozzleDifficulty.EASY)
                                    if (v == 0)
                                    {
                                        if (!ColumnLimitation(ij[pointer].row - columnPosition1 + v, ij[pointer].column, 1))
                                        {
                                            available = false;
                                            break;
                                        }
                                    }
                                    else if (v == tempColumn.Word.Count() - 1)
                                    {
                                        if (!ColumnLimitation(ij[pointer].row - columnPosition1 + v, ij[pointer].column, 2))
                                        {
                                            available = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (!ColumnLimitation(ij[pointer].row - columnPosition1 + v, ij[pointer].column, 3))
                                        {
                                            available = false;
                                            break;
                                        }
                                    }

                                // Scan and check for position.
                                if (ij[pointer].row - columnPosition1 + v != ij[pointer].row)
                                {
                                    var has = ij.Where(a => a.row == ij[pointer].row - columnPosition1 + v && a.column == ij[pointer].column).FirstOrDefault();
                                    if (has != null)
                                    {
                                        available = false;
                                        break;
                                    }

                                    if (ij[pointer].column - 1 >= 0)
                                        if (!string.IsNullOrEmpty(Crz1[ij[pointer].row - columnPosition1 + v, ij[pointer].column - 1]))
                                        {
                                            available = false;
                                            break;
                                        }

                                    if (ij[pointer].column + 1 <= totalColumns - 1)
                                        if (!string.IsNullOrEmpty(Crz1[ij[pointer].row - columnPosition1 + v, ij[pointer].column + 1]))
                                        {
                                            available = false;
                                            break;
                                        }
                                }

                                if (v == 0)
                                {
                                    if (ij[pointer].row - columnPosition1 + v - 1 >= 0)
                                        if (!string.IsNullOrEmpty(Crz1[ij[pointer].row - columnPosition1 + v - 1, ij[pointer].column]))
                                        {
                                            available = false;
                                            break;
                                        }
                                }
                                else if (v == tempColumn.Word.Count() - 1)
                                {
                                    if (ij[pointer].row - columnPosition1 + v + 1 <= totalRows - 1)
                                        if (!string.IsNullOrEmpty(Crz1[ij[pointer].row - columnPosition1 + v + 1, ij[pointer].column]))
                                        {
                                            available = false;
                                            break;
                                        }
                                }
                            }
                        }
                        else
                        {
                            available = false;
                        }

                        if (available)
                            columnWordsValidator.Add(tempColumn);
                    }

                    // Validate column word has proper location.
                    if (columnWordsValidator.Count > 0)
                    {

                        int wordPointer = random.Next(0, columnWordsValidator.Count() - 1);

                        var columnPosition1 = columnWordsValidator[wordPointer].Word.IndexOf(ij[pointer].Letter);
                        var columnPosition2 = columnWordsValidator[wordPointer].Word.Count() - columnPosition1 - 1;

                        columnWordsValidator[wordPointer].IsExist = true;
                        for (int v = 0; v < columnWordsValidator[wordPointer].Word.Count(); v++)
                        {
                            if (ij[pointer].row - columnPosition1 + v > totalRows - 1)
                                Console.WriteLine("unable to do that.");

                            var itHas = ij.Where(a => a.row == ij[pointer].row - columnPosition1 + v && a.column == ij[pointer].column).FirstOrDefault();
                            if (itHas == null)
                            {
                                var pos = new Position() { row = ij[pointer].row - columnPosition1 + v, column = ij[pointer].column, Letter = columnWordsValidator[wordPointer].Word[v].ToString() };
                                ij.Add(pos);
                                columnWordsValidator[wordPointer].blockFull.Add(pos);
                                Crz1[ij[pointer].row - columnPosition1 + v, ij[pointer].column] = columnWordsValidator[wordPointer].Word[v].ToString();
                                cSc[ij[pointer].row - columnPosition1 + v, ij[pointer].column] = true;
                                crozzleGame.totalIntersections++;
                            }
                        }

                        valid = true;
                    }
                    else
                        break;
                }
                else
                {
                    var tempColumnWords = crzLs.Where(a => !a.IsExist & a.Word.Contains(ij[pointer].Letter)).ToList();
                    var columnWordsValidator = new List<Crozzle>();

                    foreach (var tempColumn in tempColumnWords)
                    {
                        var columnPosition1 = tempColumn.Word.IndexOf(ij[pointer].Letter);
                        var columnPosition2 = tempColumn.Word.Count() - columnPosition1 - 1;
                        bool available = true;

                        if (ij[pointer].column - columnPosition1 >= 0 && ij[pointer].column + columnPosition2 <= totalColumns - 1)
                        {
                            for (int v = 0; v < tempColumn.Word.Count(); v++)
                            {
                                if (crozzleDifficulty == CrozzleDifficulty.EASY)
                                    if (v == 0)
                                    {
                                        if (!RowLimitation(ij[pointer].row, ij[pointer].column - columnPosition1 + v, 1))
                                        {
                                            available = false;
                                            break;
                                        }
                                    }
                                    else if (v == tempColumn.Word.Count())
                                    {
                                        if (!RowLimitation(ij[pointer].row, ij[pointer].column - columnPosition1 + v, 2))
                                        {
                                            available = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (!RowLimitation(ij[pointer].row, ij[pointer].column - columnPosition1 + v, 3))
                                        {
                                            available = false;
                                            break;
                                        }
                                    }

                                if (ij[pointer].column - columnPosition1 + v != ij[pointer].column)
                                {
                                    var has = ij.Where(a => a.column == ij[pointer].column - columnPosition1 + v && a.row == ij[pointer].row).FirstOrDefault();
                                    if (has != null)
                                    {
                                        available = false;
                                        break;
                                    }

                                    if (ij[pointer].row - 1 >= 0)
                                        if (!string.IsNullOrEmpty(Crz1[ij[pointer].row - 1, ij[pointer].column - columnPosition1 + v]))
                                        {
                                            available = false;
                                            break;
                                        }

                                    if (ij[pointer].row + 1 <= totalRows - 1)
                                        if (!string.IsNullOrEmpty(Crz1[ij[pointer].row + 1, ij[pointer].column - columnPosition1 + v]))
                                        {
                                            available = false;
                                            break;
                                        }
                                }

                                if (v == 0)
                                {
                                    if (ij[pointer].column - columnPosition1 + v - 1 >= 0)
                                        if (!string.IsNullOrEmpty(Crz1[ij[pointer].row, ij[pointer].column - columnPosition1 + v - 1]))
                                        {
                                            available = false;
                                            break;
                                        }
                                }
                                else if (v == tempColumn.Word.Count() - 1)
                                {
                                    if (ij[pointer].column - columnPosition1 + v + 1 <= totalColumns - 1)
                                        if (!string.IsNullOrEmpty(Crz1[ij[pointer].row, ij[pointer].column - columnPosition1 + v + 1]))
                                        {
                                            available = false;
                                            break;
                                        }
                                }
                            }
                        }
                        else
                        {
                            available = false;
                        }

                        if (available)
                            columnWordsValidator.Add(tempColumn);
                    }

                    if (columnWordsValidator.Count > 0)
                    {
                        int wordPointer = random.Next(0, columnWordsValidator.Count() - 1);
                        var columnPosition1 = columnWordsValidator[wordPointer].Word.IndexOf(ij[pointer].Letter);
                        var columnPosition2 = columnWordsValidator[wordPointer].Word.Count() - columnPosition1 - 1;

                        columnWordsValidator[wordPointer].IsExist = true;

                        // Scan if the position has been used.
                        for (int v = 0; v < columnWordsValidator[wordPointer].Word.Count(); v++)
                        {
                            var itHas = ij.Where(a => a.column == ij[pointer].column - columnPosition1 + v && a.row == ij[pointer].row).FirstOrDefault();

                            if (itHas == null)
                            {
                                var pos = new Position() { column = ij[pointer].column - columnPosition1 + v, row = ij[pointer].row, Letter = columnWordsValidator[wordPointer].Word[v].ToString() };
                                ij.Add(pos);
                                columnWordsValidator[wordPointer].blockFull.Add(pos);
                                Crz1[ij[pointer].row, ij[pointer].column - columnPosition1 + v] = columnWordsValidator[wordPointer].Word[v].ToString();
                                rThere[ij[pointer].row, ij[pointer].column - columnPosition1 + v] = true;
                                crozzleGame.totalIntersections++;
                            }
                        }
                        valid = true;
                    }
                    else
                        break;
                }
            }
            while (!false);
        }
        #endregion 
      
        /// <summary>
        /// Execute crozzle
        /// Activate BackgroundWorker so the create crozzle algorithm
        /// can run in the background without freezing the program.
        /// </summary>
        #region execute crozzle
        private void exec()
        {
            tempSc = 0;
            scoreContainer.Text = "0";

            // Check the wordlist availability.
            if (this.wordList == null)
            {
                MessageBox.Show("Wordlist not found");
                return;
            }

            if (this.wordList.Count == 0)
            {
                MessageBox.Show("Wordlist not found");
                return;
            }

            // Execute background worker for finding crozzle.
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            // Increment the worker for the progress bar.
            this.computeCrozzle += new EventHandler<LongProcessEventArgs>((processSender, args) =>
            {
                worker.ReportProgress(args.Percent);
            });

            // Give the worker some jobs.
            worker.DoWork += new DoWorkEventHandler(work);
            worker.ProgressChanged += new ProgressChangedEventHandler(processHandler);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerFinished);
            worker.RunWorkerAsync();
        }
        #endregion     

        
        /// <summary>
        /// Handler collections.
        /// </summary>
        #region open wordlist handler
        // Open and validate Wordlist File.
        private void openWordlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadWordlist();
            openCrozzleToolStripMenuItem.Enabled = true;
            playToolStripMenuItem.Enabled = true;
        }
        #endregion

        #region open and read crozzle
        // Open and validate Crozzle File.
        private void openCrozzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadCrozzle();
        }
        #endregion

        #region play and validate event handler
        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exec();
            saveToolStripMenuItem.Enabled = true;
        }
        #endregion        

        #region save event handler
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region exit event handler
        // Exit button.
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
