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
    public partial class GameOver : Form
    {
        public static FieldType winning_color;

        public GameOver()
        {
            InitializeComponent();
        }

        private void GameOver_Load(object sender, EventArgs e)
        {
            switch (winning_color)
            {
                case FieldType.yello:
                    label2.Text = "Żółty";
                    break;
                case FieldType.red:
                    label2.Text = "Czerwony";
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1.Instance.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EventManager.Call("StartAgain");
            this.Close();
        }
    }
}
