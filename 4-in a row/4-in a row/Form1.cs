using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnityTools.SimpleManager;

namespace _4_in_a_row
{
    public partial class Form1 : Form
    {
        public static Form1 Instance;
        Game CurrGame;
        Timer timer = new Timer();
        public static int TimeToMove = 1000;
        public static int DefaultAILVL = 4;
        public static Player PlayerOne, PlayerTwo;
        Random rnd = new Random();
        public Form1()
        {
            Player AI = new Player(false);
            Serializer.Desirialize(out AI, out TimeToMove, "Settings.sett");
            DefaultAILVL = AI.difficultyLvl;
            if (AI.AmIYellow)
            {
                PlayerOne = AI;
                PlayerTwo = new Player(false);
            }
            else
            {
                PlayerOne = new Player(true);
                PlayerTwo = AI;
            }
            //Zrobić aby pionki nie pojawiały się 2 na raz
            timer.Interval = 10;
            timer.Tick += timer_Tick;
            timer.Enabled = true;

            Instance = this;
            InitializeComponent();
            CurrGame = new Game(PlayerOne, PlayerTwo, TimeToMove);
            CurrGame.StartGame();
            EventManager.AddEvent("StartAgain", StartAgain);
            EventManager.AddEvent("ChangeLVL", AILVL_Changed);
            EventManager.AddEvent("ChangeColor", AI_ChangeColor);
        }

        private void StartAgain()
        {
            CurrGame.Dispose();
            if (PlayerOne.Color == FieldType.random)
            {
                int rng = rnd.Next(2);
                PlayerOne.AmIYellow = rng == 0;
                PlayerTwo.AmIYellow = rng != 0;
            }
            else if (PlayerTwo.Color == FieldType.random)
            {
                int rng = rnd.Next(2);
                PlayerTwo.AmIYellow = rng == 0;
                PlayerOne.AmIYellow = rng != 0;
            }
            if (PlayerOne.Color == FieldType.yello)
            {
                PlayerOne.AmIYellow = true;
                PlayerTwo.AmIYellow = false;
            }
            else if (PlayerOne.Color == FieldType.red)
            {
                PlayerOne.AmIYellow = false;
                PlayerTwo.AmIYellow = true;
            }

            if (PlayerTwo.Color == FieldType.yello)
            {
                PlayerTwo.AmIYellow = true;
                PlayerOne.AmIYellow = false;
            }
            else if (PlayerTwo.Color == FieldType.red)
            {
                PlayerTwo.AmIYellow = false;
                PlayerOne.AmIYellow = true;
            }

            CurrGame = new Game(PlayerOne, PlayerTwo, TimeToMove);
            CurrGame.StartGame();
        }
        private void AILVL_Changed()
        {
            if (PlayerOne.AI)
                PlayerOne.difficultyLvl = DefaultAILVL;
            if (PlayerTwo.AI)
                PlayerTwo.difficultyLvl = DefaultAILVL;
        }
        private void AI_ChangeColor(params object[] obj)
        {
            Nullable<FieldType> temp = obj[0] as Nullable<FieldType>;
            if (temp.HasValue)
            {
                if (PlayerOne.AI)
                    PlayerOne.Color = temp.Value;
                if (PlayerTwo.AI)
                    PlayerTwo.Color = temp.Value;
            }
        }
        void timer_Tick(object sender, EventArgs e)
        {
            CurrGame.DrawCircle(PointToClient(Cursor.Position).X);
        }

        private void nowaGraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartAgain();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (PlayerOne.AI)
                Serializer.Serialize(PlayerOne, TimeToMove, "Settings.sett");
            else
                Serializer.Serialize(PlayerTwo, TimeToMove, "Settings.sett");
        }//---------------------------------------------

        private void ustawieniaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings sett = new Settings();
            sett.Show();
        }// ----------------------------------------

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }// ---------------------------------------------------------
    }
}
