using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _4_in_a_row
{
    public class Player
    {
        public FieldType Color;
        public bool AI, AmIYellow;
        public int difficultyLvl;

        public Player(bool AmIYellow, bool AI = false, int maxDepth = 4)
        {
            this.AI = AI;
            this.AmIYellow = AmIYellow;
            difficultyLvl = maxDepth;
        }

        public void BestMove(Game game, FieldType myColor)
        {
            FieldInfo[,] arr = new FieldInfo[7, 6];
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    arr[x, y] = game.infoArr[x, y].Tag as FieldInfo;
                }
            }
            var X = 0;

            //AI to makes it's move

            var bestScore = int.MinValue;
            for (int x = 0; x < 7; x++)
            {
                if (CanGoHere(arr, x) > -1)
                {
                    int row = CanGoHere(arr, x);
                    arr[x, row].fieldColor = myColor;
                    int score = Minimax(0, arr, false);
                    arr[x, row].fieldColor = FieldType.empty;
                    if (score > bestScore)
                    {
                        X = x;
                        bestScore = score;
                    }
                }
            }
            game.MakeMove(X, true);
        }

        private int Minimax(int depth, FieldInfo[,] arr, bool isMaximazing)
        {

            //Check if already won
            FieldType temp = CheckForWin(arr, isMaximazing ? FieldType.yello : FieldType.red);
            var bestScore = 0;

            switch (temp)
            {
                case FieldType.yello:
                    return (AmIYellow ? 100 * (42 - depth) : -100 * (42 - depth));
                case FieldType.red:
                    return (AmIYellow ? -100 * (42 - depth) : 100 * (42 - depth));
            }

            //Check if it's a tie
            if (!isTherePossibleMove(arr))
            {
                return 0;
            }
            //Check if it's maximum depth
            if (depth == difficultyLvl)
                return bestScore;
            //Minimax

            //Maximizing Player
            if (isMaximazing)
            {
                bestScore = int.MinValue;
                for (int i = 0; i < 7; i++)
                {
                    //Is spot available
                    var y = CanGoHere(arr, i);
                    if (y > -1)
                    {
                        arr[i, y].fieldColor = AmIYellow ? FieldType.yello : FieldType.red;
                        var score = Minimax(depth + 1, arr, false);
                        score += FieldCount(arr, i, AmIYellow ? FieldType.yello : FieldType.red);
                        arr[i, y].fieldColor = FieldType.empty;
                        bestScore = Math.Max(score, bestScore);
                    }
                }
                return bestScore;
            }
            //Minimazing Player
            else
            {
                bestScore = int.MaxValue;
                for (int i = 0; i < 7; i++)
                {
                    //Is spot available
                    var y = CanGoHere(arr, i);
                    if (y > -1)
                    {
                        arr[i, y].fieldColor = AmIYellow ? FieldType.red : FieldType.yello;
                        var score = Minimax(depth + 1, arr, true);
                        score -= FieldCount(arr, i, AmIYellow ? FieldType.red : FieldType.yello);
                        arr[i, y].fieldColor = FieldType.empty;
                        bestScore = Math.Min(score, bestScore);
                    }
                }
                return bestScore;
            }
        }

        

        #region Minimax Rules
        //Add Score if it isn't first pawn in a line
        //extra score if you can still win thanks to it
       
        private int FieldCount(FieldInfo[,] arr, int column, FieldType myColor)
        {
            int Count = 1;
            int y = CanGoHere(arr, column);
            int mult = 4;
            #region Don't touch that sh*t
            for (int i = 0; i < 8; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            int tempX = column;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, 1, 0, arr) == myColor)
                            {
                                tempX++;
                                Count += (CanWinFromThisPosition(arr, tempX, tempY, myColor) * mult) + 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            int tempX = column;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, 1, -1, arr) == myColor)
                            {
                                tempX++;
                                tempY--;
                                Count += (CanWinFromThisPosition(arr, tempX, tempY, myColor) * mult) + 1;
                            }
                        }
                        break;
                    case 2:
                        {
                            int tempX = column;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, 0, -1, arr) == myColor)
                            {
                                tempY--;
                                Count += (CanWinFromThisPosition(arr, tempX, tempY, myColor) * mult) + 1;
                            }
                        }
                        break;
                    case 3:
                        {
                            int tempX = column;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, -1, -1, arr) == myColor)
                            {
                                tempX--;
                                tempY--;
                                Count += (CanWinFromThisPosition(arr, tempX, tempY, myColor) * mult) + 1;
                            }
                        }
                        break;
                    case 4:
                        {
                            int tempX = column;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, -1, 0, arr) == myColor)
                            {
                                tempX--;
                                Count += (CanWinFromThisPosition(arr, tempX, tempY, myColor) * mult) + 1;
                            }
                        }
                        break;
                    case 5:
                        {
                            int tempX = column;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, -1, 1, arr) == myColor)
                            {
                                tempX--;
                                tempY--;
                                Count += (CanWinFromThisPosition(arr, tempX, tempY, myColor) * mult) + 1;
                            }
                        }
                        break;
                    case 6:
                        {
                            int tempX = column;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, 0, 1, arr) == myColor)
                            {
                                tempY++;
                                Count += (CanWinFromThisPosition(arr, tempX, tempY, myColor) * mult) + 1;
                            }
                        }
                        break;
                    case 7:
                        {
                            int tempX = column;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, 1, 1, arr) == myColor)
                            {
                                tempX++;
                                tempY++;
                                Count += (CanWinFromThisPosition(arr, tempX, tempY, myColor) * mult) + 1;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            #endregion
            return Count;

        }// -------------------------------------

       
        #endregion
        #region Minimax Rules Help
        // *********************************************************************************
        private int CanWinFromThisPosition(FieldInfo[,] arr, int x, int y, FieldType myColor)
        {
            #region Some Basic Stuff
            if (x < 0)
                return 0;
            if (x >= arr.GetLength(0))
                return 0;
            if (y < 0)
                return 0;
            if (y >= arr.GetLength(1))
                return 0;
            #region Don't touch that sh*t either
            int HowManyWins = 0;
            for (int i = 0; i < 8; i++)
            {
                int count = 1;
                switch (i)
                {
                    case 0:
                        {
                            int tempX = x;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, 1, 0, arr) == myColor)
                            {
                                tempX++;
                                count++;
                                if (count >= 4)
                                    HowManyWins++;
                            }
                            if (count < 4)
                                HowManyWins = 0;
                        }
                        break;
                    case 1:
                        {
                            int tempX = x;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, 1, -1, arr) == myColor)
                            {
                                tempX++;
                                tempY--;
                                count++;
                                if (count >= 4)
                                    HowManyWins++;
                            }
                            if (count < 4)
                                HowManyWins = 0;
                        }
                        break;
                    case 2:
                        {
                            int tempX = x;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, 0, -1, arr) == myColor)
                            {
                                tempY--;
                                count++;
                                if (count >= 4)
                                    HowManyWins++;
                            }
                            if (count < 4)
                                HowManyWins = 0;
                        }
                        break;
                    case 3:
                        {
                            int tempX = x;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, -1, -1, arr) == myColor)
                            {
                                tempX--;
                                tempY--;
                                count++;
                                if (count >= 4)
                                    HowManyWins++;
                            }
                            if (count < 4)
                                HowManyWins = -0;
                        }
                        break;
                    case 4:
                        {
                            int tempX = x;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, -1, 0, arr) == myColor)
                            {
                                tempX--;
                                count++;
                                if (count >= 4)
                                    HowManyWins++;
                            }
                            if (count < 4)
                                HowManyWins = 0;
                        }
                        break;
                    case 5:
                        {
                            int tempX = x;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, -1, 1, arr) == myColor)
                            {
                                tempX--;
                                tempY--;
                                count++;
                                if (count >= 4)
                                    HowManyWins++;
                            }
                            if (count < 4)
                                HowManyWins = 0;
                        }
                        break;
                    case 6:
                        {
                            int tempX = x;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, 0, 1, arr) == myColor)
                            {
                                tempY++;
                                count++;
                                if (count >= 4)
                                    HowManyWins++;
                            }
                            if (count < 4)
                                HowManyWins = 0;
                        }
                        break;
                    case 7:
                        {
                            int tempX = x;
                            int tempY = y;
                            while (WhatIsHere(tempX, tempY, 1, 1, arr) == myColor)
                            {
                                tempX++;
                                tempY++;
                                count++;
                                if (count >= 4)
                                    HowManyWins++;
                            }
                            if (count < 4)
                                HowManyWins = 0;
                        }
                        break;
                }
            #endregion
            }
            #endregion
            return HowManyWins;
        } // ------------------------------------------------------------------------

        private FieldType WhatIsHere(int x, int y, int xOffset, int yOffset, FieldInfo[,] arr)
        {
            if (x + xOffset < 0 || x + xOffset >= arr.GetLength(0))
            {
                return FieldType.empty;
            }
            if (y + yOffset < 0 || y + yOffset >= arr.GetLength(1))
            {
                return FieldType.empty;
            }
            return arr[x + xOffset, yOffset + y].fieldColor;
        }// ------------------------------------------------------------------------

        private int CanGoHere(FieldInfo[,] arr, int column)
        {
            for (int i = 5; i >= 0; i--)
            {
                var info = arr[column, i];
                if (info != null)
                {
                    if (info.fieldColor == FieldType.empty)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }// ------------------------------------------------------------------------


        private bool isTherePossibleMove(FieldInfo[,] arr)
        {
            foreach (var item in arr)
            {
                if (item.fieldColor == FieldType.empty)
                    return true;
            }
            return false;
        }// ----------------------------------------------------------------------

        private FieldType CheckForWin(FieldInfo[,] arr, FieldType color)
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (arr[x, y].fieldColor == color)
                    {
                        bool isGameOver = false;
                        int i = 0;
                        while (i < 4)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        CheckForWinHelp(1, x, y, 0, -1, color, ref isGameOver, arr);
                                        break;
                                    case 1:
                                        CheckForWinHelp(1, x, y, 1, -1, color, ref isGameOver, arr);
                                        break;
                                    case 2:
                                        CheckForWinHelp(1, x, y, 1, 0, color, ref isGameOver, arr);
                                        break;
                                    case 3:
                                        CheckForWinHelp(1, x, y, 1, 1, color, ref isGameOver, arr);
                                        break;
                                    case 4:
                                        CheckForWinHelp(1, x, y, 0, 1, color, ref isGameOver, arr);
                                        break;
                                    case 5:
                                        CheckForWinHelp(1, x, y, -1, 1, color, ref isGameOver, arr);
                                        break;
                                    case 6:
                                        CheckForWinHelp(1, x, y, -1, 0, color, ref isGameOver, arr);
                                        break;
                                    case 7:
                                        CheckForWinHelp(1, x, y, -1, -1, color, ref isGameOver, arr);
                                        break;
                                }
                            }
                            i++;
                        }

                        if (isGameOver)
                        {
                            return color;
                        }

                    }
                }
            }
            return FieldType.empty;
        }// ----------------------------------------------------------

        void CheckForWinHelp(int depth, int x, int y, int xOffset, int yOffset, FieldType myColor, ref bool GameOver, FieldInfo[,] arr)
        {
            if (depth == 4)
            {
                GameOver = true;
            }
            if (x + xOffset < 0 || x + xOffset > 7 - 1)
                return;
            if (y + yOffset < 0 || y + yOffset > 6 - 1)
                return;

            FieldInfo info = arr[x + xOffset, y + yOffset];
            if (info.fieldColor == myColor)
            {
                CheckForWinHelp(depth + 1, x + xOffset, y + yOffset, xOffset, yOffset, myColor, ref GameOver, arr);
            }

        }// ----------------------------------------------------------
        // *********************************************************************8888
        #endregion
    }
}