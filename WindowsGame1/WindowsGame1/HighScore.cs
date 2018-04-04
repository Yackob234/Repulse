using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace repulse
{
    public class HighScore
    {
        public bool newHighScore = false;
        private string[] _letter = new string[] {"_ ", "_ ", "_ ", "_ ", "_ ", "_ ", "_ ", "_ "};
        private int _letterPos = 0;
        public string PlayerName;
        public double Score;

        public HighScore(EntityDrawData drawData)
        {
            highScoreRead();
        }

        public void highScoreRead()
        {
            string fileContents = File.ReadAllText("C:/Users/Yackob/Desktop/Coding/smartgit/Repulse/WindowsGame1/WindowsGame1Content/score.txt");

            string[] tokens = fileContents.Split(',');
            PlayerName = tokens[0];
            Score = Convert.ToDouble(tokens[1]);
            
            //using (System.IO.StreamReader tr = new System.IO.StreamReader("score.txt"));
            /*
            string[] highScoresText = File.ReadAllLines("highscores.csv");

            HighScore[] highScores = new HighScore[highScoresText.Length];

            for (int index = 0; index < highScoresText.Length; index++)
            {
                string[] tokens = highScoresText[index].Split('h');

                _PlayerName = tokens[0];
                _score = Convert.ToInt32(tokens[1]);

                highScores[index] = new HighScore(name, score);
            }
            */
        }

        public void highScoreWrite(double reaction)
        {
            //using (System.IO.StreamWriter tw = new System.IO.StreamWriter("score.txt"))
            //{
            //    tw.Write(reaction);
            //}
            highScoreRead();
            if (Score < reaction)
            {
                newHighScore = false;
            }
            else if (Score > reaction)
            {
                string newReactionTime = CurrentHighScoreName() + ", " + reaction + "\n";

                File.WriteAllText("C:/Users/Yackob/Desktop/Coding/smartgit/Repulse/WindowsGame1/WindowsGame1Content/score.txt", newReactionTime);
                newHighScore = true;
                highScoreRead();
            }
            
        }

        public void HighScoreUpdate(double reaction)
        {
            highScoreRead();
            if (Score < reaction)
            {
                newHighScore = false;
            }
            else if (Score >= reaction)
            {
                newHighScore = true;
            }
        }

        public string CurrentHighScoreName()
        {
            string currentName = _letter[0] + _letter[1] + _letter[2] + _letter[3] + _letter[4] + _letter[5] + _letter[6] + _letter[7];
            return currentName;
        }
        public void ShiftLetterPosition(string operation)
        {
            switch (operation)
            {
                case "+":
                    _letterPos++;
                    break;
                case "-":
                    _letterPos--;
                    break;
            }
            if(_letterPos < 0)
            {
                _letterPos = 0;
            }
            else if (_letterPos > 7)
            {
                _letterPos = 7;
            }

        }

        public void IncreaseHighScoreLetter()
        {
            switch (_letter[_letterPos])
            {
                case "0 ":
                    _letter[_letterPos] = "Z ";
                    break;
                case "1 ":
                    _letter[_letterPos] = "0 ";
                    break;
                case "2 ":
                    _letter[_letterPos] = "1 ";
                    break;
                case "3 ":
                    _letter[_letterPos] = "2 ";
                    break;
                case "4 ":
                    _letter[_letterPos] = "3 ";
                    break;
                case "5 ":
                    _letter[_letterPos] = "4 ";
                    break;
                case "6 ":
                    _letter[_letterPos] = "5 ";
                    break;
                case "7 ":
                    _letter[_letterPos] = "6 ";
                    break;
                case "8 ":
                    _letter[_letterPos] = "7 ";
                    break;
                case "9 ":
                    _letter[_letterPos] = "8 ";
                    break;
                case "_ ":
                    _letter[_letterPos] = "9 ";
                    break;
                case "A ":
                    _letter[_letterPos] = "_ ";
                    break;
                case "B ":
                    _letter[_letterPos] = "A ";
                    break;
                case "C ":
                    _letter[_letterPos] = "B ";
                    break;
                case "D ":
                    _letter[_letterPos] = "C ";
                    break;
                case "E ":
                    _letter[_letterPos] = "D ";
                    break;
                case "F ":
                    _letter[_letterPos] = "E ";
                    break;
                case "G ":
                    _letter[_letterPos] = "F ";
                    break;
                case "H ":
                    _letter[_letterPos] = "G ";
                    break;
                case "I ":
                    _letter[_letterPos] = "H ";
                    break;
                case "J ":
                    _letter[_letterPos] = "I ";
                    break;
                case "K ":
                    _letter[_letterPos] = "J ";
                    break;
                case "L ":
                    _letter[_letterPos] = "K ";
                    break;
                case "M ":
                    _letter[_letterPos] = "L ";
                    break;
                case "N ":
                    _letter[_letterPos] = "M ";
                    break;
                case "O ":
                    _letter[_letterPos] = "N ";
                    break;
                case "P ":
                    _letter[_letterPos] = "O ";
                    break;
                case "Q ":
                    _letter[_letterPos] = "P ";
                    break;
                case "R ":
                    _letter[_letterPos] = "Q ";
                    break;
                case "S ":
                    _letter[_letterPos] = "R ";
                    break;
                case "T ":
                    _letter[_letterPos] = "S ";
                    break;
                case "U ":
                    _letter[_letterPos] = "T ";
                    break;
                case "V ":
                    _letter[_letterPos] = "U ";
                    break;
                case "W ":
                    _letter[_letterPos] = "V ";
                    break;
                case "X ":
                    _letter[_letterPos] = "W ";
                    break;
                case "Y ":
                    _letter[_letterPos] = "X ";
                    break;
                case "Z ":
                    _letter[_letterPos] = "Y ";
                    break;
            }

        }
        public void DecreaseHighScoreLetter()
        {
            switch (_letter[_letterPos])
            {
                case "0 ":
                    _letter[_letterPos] = "1 ";
                    break;
                case "1 ":
                    _letter[_letterPos] = "2 ";
                    break;
                case "2 ":
                    _letter[_letterPos] = "3 ";
                    break;
                case "3 ":
                    _letter[_letterPos] = "4 ";
                    break;
                case "4 ":
                    _letter[_letterPos] = "5 ";
                    break;
                case "5 ":
                    _letter[_letterPos] = "6 ";
                    break;
                case "6 ":
                    _letter[_letterPos] = "7 ";
                    break;
                case "7 ":
                    _letter[_letterPos] = "8 ";
                    break;
                case "8 ":
                    _letter[_letterPos] = "9 ";
                    break;
                case "9 ":
                    _letter[_letterPos] = "_ ";
                    break;
                case "_ ":
                    _letter[_letterPos] = "A ";
                    break;
                case "A ":
                    _letter[_letterPos] = "B ";
                    break;
                case "B ":
                    _letter[_letterPos] = "C ";
                    break;
                case "C ":
                    _letter[_letterPos] = "D ";
                    break;
                case "D ":
                    _letter[_letterPos] = "E ";
                    break;
                case "E ":
                    _letter[_letterPos] = "F ";
                    break;
                case "F ":
                    _letter[_letterPos] = "G ";
                    break;
                case "G ":
                    _letter[_letterPos] = "H ";
                    break;
                case "H ":
                    _letter[_letterPos] = "I ";
                    break;
                case "I ":
                    _letter[_letterPos] = "J ";
                    break;
                case "J ":
                    _letter[_letterPos] = "K ";
                    break;
                case "K ":
                    _letter[_letterPos] = "L ";
                    break;
                case "L ":
                    _letter[_letterPos] = "M ";
                    break;
                case "M ":
                    _letter[_letterPos] = "N ";
                    break;
                case "N ":
                    _letter[_letterPos] = "O ";
                    break;
                case "O ":
                    _letter[_letterPos] = "P ";
                    break;
                case "P ":
                    _letter[_letterPos] = "Q ";
                    break;
                case "Q ":
                    _letter[_letterPos] = "R ";
                    break;
                case "R ":
                    _letter[_letterPos] = "S ";
                    break;
                case "S ":
                    _letter[_letterPos] = "T ";
                    break;
                case "T ":
                    _letter[_letterPos] = "U ";
                    break;
                case "U ":
                    _letter[_letterPos] = "V ";
                    break;
                case "V ":
                    _letter[_letterPos] = "W ";
                    break;
                case "W ":
                    _letter[_letterPos] = "X ";
                    break;
                case "X ":
                    _letter[_letterPos] = "Y ";
                    break;
                case "Y ":
                    _letter[_letterPos] = "Z ";
                    break;
                case "Z ":
                    _letter[_letterPos] = "0 ";
                    break;
            }

        }

    }
    
}

