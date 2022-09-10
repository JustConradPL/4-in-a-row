using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace _4_in_a_row
{
    public static class Serializer
    {
        private enum FileSegment
        {
            none,
            AISettings,
            GraphicSettings
        }

        public static bool Serialize(Player AIPlayer, int TimeToMove, string FileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[AISettings]");
            sb.AppendLine("DefaultLVL=" + AIPlayer.difficultyLvl);
            switch (AIPlayer.Color)
            {
                case FieldType.yello:
                    sb.AppendLine("PlayerColor=0");
                    break;
                case FieldType.red:
                    sb.AppendLine("PlayerColor=1");
                    break;
                case FieldType.random:
                    sb.AppendLine("PlayerColor=2");
                    break;
            }
            sb.AppendLine();
            sb.AppendLine("[GraphicSettings]");
            sb.AppendLine("TimeToMove=" + TimeToMove);
            try
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Create))
                {
                    StreamWriter bw = new StreamWriter(fs, Encoding.ASCII);
                    bw.Write(sb.ToString());
                    bw.Dispose();
                    fs.Dispose();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }// ----------------------------------------------

        public static bool Desirialize(out Player AIPlayer, out int TimeToMove, string FileName)
        {
            TimeToMove = 1000;
            AIPlayer = new Player(false, true);
            FileSegment segment;
            try
            {
                AIPlayer.difficultyLvl = 4;
                using (FileStream fs = new FileStream(FileName, FileMode.Open))
                {
                    if (fs.Length > 0)
                    {
                        StreamReader br = new StreamReader(fs, Encoding.ASCII);
                        string text = br.ReadToEnd();
                        br.Dispose();
                        fs.Dispose();
                        string[] block = text.Split('\r', '\n');
                        for (int i = 0; i < block.Length; )
                        {
                            if (block[i] == "[AISettings]")
                            {
                                segment = FileSegment.AISettings;
                                while (segment == FileSegment.AISettings)
                                {
                                    i++;
                                    if (i >= block.Length)
                                        break;
                                    if (block[i].StartsWith("["))
                                    {
                                        segment = FileSegment.none;
                                    }
                                    else if (block[i].StartsWith("DefaultLVL"))
                                    {
                                        int index = block[i].IndexOf('=');
                                        string value = block[i].Substring(index + 1, block[i].Length - index - 1);
                                        AIPlayer.difficultyLvl = int.Parse(value);
                                    }
                                    else if (block[i].StartsWith("PlayerColor"))
                                    {
                                        int index = block[i].IndexOf('=');
                                        string value = block[i].Substring(index + 1, block[i].Length - index - 1);
                                        switch (value)
                                        {
                                            case "0":
                                                AIPlayer.Color = FieldType.yello;
                                                AIPlayer.AmIYellow = true;
                                                break;
                                            case "1":
                                                AIPlayer.Color = FieldType.red;
                                                AIPlayer.AmIYellow = false;
                                                break;
                                            case "2":
                                                AIPlayer.Color = FieldType.random;
                                                Random rnd = new Random();
                                                int rng = rnd.Next(2);
                                                if (rng == 0)
                                                    AIPlayer.AmIYellow = true;
                                                else
                                                    AIPlayer.AmIYellow = false;
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                            else if (block[i] == "[GraphicSettings]")
                            {
                                segment = FileSegment.GraphicSettings;
                                while (segment == FileSegment.GraphicSettings)
                                {
                                    i++;
                                    if (i >= block.Length)
                                        break;
                                    if (block[i].StartsWith("["))
                                    {
                                        segment = FileSegment.none;
                                    }
                                    else if (block[i].StartsWith("TimeToMove"))
                                    {
                                        int index = block[i].IndexOf('=');
                                        string value = block[i].Substring(index + 1, block[i].Length - index - 1);
                                        TimeToMove = int.Parse(value);
                                        if (TimeToMove < 1)
                                            TimeToMove = 1;
                                    }
                                }
                            }
                            else
                            {
                                i++;
                            }
                        }



                    }
                }

                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                AIPlayer.difficultyLvl = 4;
                return false;
            }
        }
    }
}
