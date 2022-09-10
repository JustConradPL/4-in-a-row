using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Drawing;
using System.Windows.Forms;
namespace _4_in_a_row
{
    delegate void Act(params object[] obj);
    public class Game
    {
        static Image emptyField, YellowField, RedField,YellCircle,RedCircle;
        Player firstPlayer, secondPlayer;
        const int xSize = 7, ySize = 6, cellSize = 118;
        bool FirstPlayerTurn;
        public Label[,] infoArr;
        bool AbleToMove;
        Timer aTime = new Timer();
        Timer bTime = new Timer();
        public static Label PlayerPawn = new Label();
        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        public Game(Player firstPlayer,Player secondPlayer,int TimeToMove)
        {
            PlayerPawn.Image = YellCircle;
            PlayerPawn.Visible = true;
            // Timer A
            aTime.Enabled = false;
            aTime.Interval = TimeToMove;
            aTime.Tick += aTime_Tick;

            // Timer B
            bTime.Enabled = false;
            bTime.Interval = TimeToMove;
            bTime.Tick += bTime_Tick;

            AbleToMove = true;
            if (firstPlayer.AmIYellow)
            {
                this.firstPlayer = firstPlayer;
                this.secondPlayer = secondPlayer;
            }
            else if (secondPlayer.AmIYellow)
            {
                this.firstPlayer = secondPlayer;
                this.secondPlayer = firstPlayer;
            }
            FirstPlayerTurn = true;
            infoArr = new Label[xSize, ySize];
            InitTable();
        }

        void bTime_Tick(object sender, EventArgs e)
        {
            secondPlayer.BestMove(this, FieldType.red);
            FirstPlayerTurn = !FirstPlayerTurn;
            PlayerPawn.Image = FirstPlayerTurn ? YellCircle : RedCircle;
            AbleToMove = true;
            PlayerPawn.Visible = true;
            bTime.Stop();
        }

        void aTime_Tick(object sender, EventArgs e)
        {
            firstPlayer.BestMove(this, FieldType.yello);
            FirstPlayerTurn = !FirstPlayerTurn;
            PlayerPawn.Image = FirstPlayerTurn ? YellCircle : RedCircle;
            AbleToMove = true;
            PlayerPawn.Visible = true;
            aTime.Stop();
        }

        static Game()
        {
            Game.emptyField = GraphicResources.pustePole;
            Game.RedField = GraphicResources.CzerwonePole;
            Game.YellowField = GraphicResources.ŻółtePole;
            Game.RedCircle = GraphicResources.RedCircle;
            Game.YellCircle = GraphicResources.YellCircle;

            PlayerPawn.Image = YellCircle;
            PlayerPawn.Size = new Size(cellSize, cellSize);
        }// --------------------------------------------------------

        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        public void StartGame()
        {
            DrawTable(0,125);
            if (this.firstPlayer.AI)
            {
                firstPlayer.BestMove(this, FieldType.yello);
                FirstPlayerTurn = !FirstPlayerTurn;
                PlayerPawn.Image = FirstPlayerTurn ? YellCircle : RedCircle;
            }
        }// ----------------------------------------------------------

        void DrawTable(int xOffset,int yOffset)
        {
            int width = 1200, height = 1200;

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    CreateLabel(x * cellSize+xOffset, y * cellSize+yOffset, x, y);
                    if (y + 1 == ySize)
                        height = (y + 1) * cellSize;
                }
                if (x + 1 == xSize)
                    width = (x + 1) * cellSize;
            }
            Form1.Instance.ClientSize = new Size(width+xOffset, height+yOffset);
            Form1.Instance.Controls.Add(PlayerPawn);
        }// ----------------------------------------------------------

        void CreateLabel(int x, int y,int column,int row)
        {
            Label lbl = new Label();
            lbl.SetBounds(x, y, cellSize, cellSize);
            lbl.Tag = new FieldInfo(column, row);
            lbl.Image = emptyField;
            lbl.Click += OnFieldClick;
            infoArr[column, row] = lbl;
            Form1.Instance.Controls.Add(lbl);
        }// ----------------------------------------------------------

        void InitTable()
        {
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    infoArr[x, y] = new Label();
                }
            }
        }// ------------------------------------------------------

        Label FindLabel(int x)
        {
            for (int i = ySize-1; i >= 0 ; i--)
            {
                var info=infoArr[x,i].Tag as FieldInfo;
                if (info != null)
                {
                    if (info.fieldColor==FieldType.empty)
                    {
                        return infoArr[x, info.ROW];
                    }
                }
            }
            return null;
        }

        public void DrawCircle(int x)
        {
            if (AbleToMove)
            {
                if (x + 50 < Form1.Instance.ClientSize.Width && x > 50)
                {
                    PlayerPawn.Location = new Point(x - cellSize / 2, 20);
                }
            }
            else
            {
                PlayerPawn.Visible = false;
            }
        }// ---------------------------------------------------------

        private bool CheckForWin()
        {
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if ((infoArr[x, y].Tag as FieldInfo).fieldColor != FieldType.empty)
                    {
                        FieldType color = (infoArr[x, y].Tag as FieldInfo).fieldColor;
                        bool isGameOver = false;
                        int i = 0;
                        while (i<4)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        CheckForWinHelp(1,x, y, 0, -1, color, ref isGameOver);
                                        break;
                                    case 1:
                                        CheckForWinHelp(1, x, y, 1, -1, color, ref isGameOver);
                                        break;
                                    case 2:
                                        CheckForWinHelp(1, x, y, 1, 0, color, ref isGameOver);
                                        break;
                                    case 3:
                                        CheckForWinHelp(1, x, y, 1, 1, color, ref isGameOver);
                                        break;
                                    case 4:
                                        CheckForWinHelp(1, x, y, 0, 1, color, ref isGameOver);
                                        break;
                                    case 5:
                                        CheckForWinHelp(1, x, y, -1, 1, color, ref isGameOver);
                                        break;
                                    case 6:
                                        CheckForWinHelp(1, x, y, -1, 0, color, ref isGameOver);
                                        break;
                                    case 7:
                                        CheckForWinHelp(1, x, y, -1, -1, color, ref isGameOver);
                                        break;
                                }
                            }
                            i++;
                        }

                        //show that game is over
                        if (isGameOver)
                        {
                            GameOver.winning_color = color;
                            GameOver window = new GameOver();
                            window.Show();
                            return true;
                        }
                    }
                }
            }
            return false;
        }// ----------------------------------------------------------


        /// <summary>
        /// Don't look at it
        /// </summary>
        /// <param name="depth">set depth to 1</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <param name="myColor"></param>
        /// <param name="GameOver"></param>
        void CheckForWinHelp(int depth,int x, int y, int xOffset, int yOffset,FieldType myColor, ref bool GameOver)
        {
            if (depth == 4)
            {
                GameOver = true;
                AbleToMove = false;
            }
            if (x + xOffset < 0 || x + xOffset > xSize - 1)
                return;
            if (y + yOffset < 0 || y + yOffset > ySize - 1)
                return;
            
            FieldInfo info = infoArr[x+xOffset, y+yOffset].Tag as FieldInfo;
            if (info.fieldColor == myColor)
            {
                CheckForWinHelp(depth + 1, x + xOffset, y + yOffset, xOffset, yOffset, myColor, ref GameOver);
            }
           
        }// ----------------------------------------------------------

        public void Dispose()
        {
            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    infoArr[x, y].Dispose();
                }
            }
        }// -----------------------------------------------------------

        public void MakeMove(int column,bool AmIAI=false)
        {
            bool IsGameOver = CheckForWin();

            if (!IsGameOver)
            {
                Label lbl = FindLabel(column);
                if (lbl != null)
                {
                    if (!AmIAI)
                    {
                        (lbl.Tag as FieldInfo).fieldColor = FirstPlayerTurn ? FieldType.yello : FieldType.red;
                        lbl.Image = FirstPlayerTurn ? YellowField : RedField;
                        FirstPlayerTurn = !FirstPlayerTurn;
                        PlayerPawn.Image = FirstPlayerTurn ? YellCircle : RedCircle;
                        IsGameOver = CheckForWin();
                        if (!IsGameOver)
                        {

                            if (FirstPlayerTurn && firstPlayer.AI)
                            {
                                PlayerPawn.Visible = false;
                                AbleToMove = false;
                                aTime.Start();
                            }
                            else if (secondPlayer.AI)
                            {
                                PlayerPawn.Visible = false;
                                AbleToMove = false;
                                bTime.Start();
                                /*
                                secondPlayer.BestMove(this, FieldType.red);
                                FirstPlayerTurn = !FirstPlayerTurn;
                                PlayerPawn.Image = FirstPlayerTurn ? YellCircle : RedCircle;
                                 */
                            }
                        }
                    }
                    else
                    {
                     
                        (lbl.Tag as FieldInfo).fieldColor = FirstPlayerTurn ? FieldType.yello : FieldType.red;
                        lbl.Image = FirstPlayerTurn ? YellowField : RedField;
                        PlayerPawn.Image = FirstPlayerTurn ? YellCircle : RedCircle;
                        CheckForWin();

                    }
                }
            }
            
        }
        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        private void OnFieldClick(object sender, EventArgs e)
        {
            if (AbleToMove)
            {
                FieldInfo info = (sender as Label).Tag as FieldInfo;
                if (info != null)
                    MakeMove(info.COLUMN);
            }
        }// ------------------------------------------------------------
        
    }//############################## GAME ###########################################
}
