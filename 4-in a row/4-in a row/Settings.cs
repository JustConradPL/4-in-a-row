using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _4_in_a_row
{
    public partial class Settings : Form
    {
        int AILVL = Form1.DefaultAILVL;
        FieldType AIColor;
        public Settings()
        {
            InitializeComponent();
            this.ClientSize = new System.Drawing.Size(300, 270);
            button3.Size = new System.Drawing.Size(20, this.Height);
            radioButton1.CheckedChanged += ChangeDifficult;
            radioButton2.CheckedChanged += ChangeDifficult;
            radioButton3.CheckedChanged += ChangeDifficult;
            radioButton4.CheckedChanged += ChangeDifficult;

            radioButton5.CheckedChanged += ChangeColor;
            radioButton6.CheckedChanged += ChangeColor;
            radioButton7.CheckedChanged += ChangeColor;
            numericUpDown1.Value = Form1.TimeToMove;
            if (Form1.PlayerOne.AI)
                AIColor = Form1.PlayerOne.Color;
            else
                AIColor = Form1.PlayerTwo.Color;

            switch (AILVL)
            {
                case 2:
                    radioButton1.Checked = true;
                    break;
                case 4:
                    radioButton2.Checked = true;
                    break;
                case 5:
                    radioButton3.Checked = true;
                    break;
                case 6:
                    radioButton4.Checked = true;
                    break;
            }
            switch (AIColor)
            {
                case FieldType.yello:
                    radioButton5.Checked = true;
                    break;
                case FieldType.red:
                    radioButton6.Checked = true;
                    break;
                case FieldType.random:
                    radioButton7.Checked = true;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.DefaultAILVL = AILVL;
            UnityTools.SimpleManager.EventManager.Call("ChangeLVL");
            UnityTools.SimpleManager.EventManager.CallWith("ChangeColor", (object)AIColor);
            UnityTools.SimpleManager.EventManager.Call("StartAgain");
            Close();
        }

        private void ChangeDifficult(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    switch (rb.Tag as string)
                    {
                        case "Easy":
                            AILVL = 2;
                            break;
                        case "Middle":
                            AILVL = 4;
                            break;
                        case "Hard":
                            AILVL = 5;
                            break;
                        case "Expert":
                            AILVL = 6;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void ChangeColor(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    switch (rb.Tag as string)
                    {
                        case "Yellow":
                            AIColor = FieldType.yello;
                            break;
                        case "Red":
                            AIColor = FieldType.red;
                            break;
                        case "Random":
                            AIColor = FieldType.random;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.ClientSize = new Size(300, 270);
            button2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            this.ClientSize = new Size(582, 270);
            button3.Location = new Point(this.Width - 35, 0);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Form1.TimeToMove = (int)numericUpDown1.Value;
        }
    }
}
