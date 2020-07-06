using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace HisogramImage
{
    public partial class Form1 : Form
    {
        private static List<Task> workers = new List<Task>();
        public int moved;
        public int mesured;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {           
            runCalc();
            //HistoGram("D:\\VisualStudio\\source\\HisogramImage\\9603571528_.jpg");
        }

        private void runCalc()
        {
            moved = 0;
            mesured = 0;
            string[] files = Directory.GetFiles(textBox1.Text);
            label1.Text = files.Length.ToString();
            int processCount = 4;
            for (int i = 0; i<processCount; i++)
            {
                Task aTask = Task.Run(() => runProc(files, i, processCount));
                workers.Add(aTask);
            }
        }

        private void runProc(string[] files, int procNum, int procCount)
        {
            for (int i=procNum; i< files.Length; i+= procCount)
            {
                bool isBW = HistoGram(files[i]);
                mesured++;
                if (isBW) moved++;

            }
        }
        
        private bool HistoGram(string fileName)
        {
            Bitmap bm = (Bitmap)Image.FromFile(fileName);   
            Dictionary<Color, int> histo = new Dictionary<Color, int>();
            int bw = 0;
            int other = 0;
            bool isBW = false;
            for (int x = 0; x < bm.Width; x+=10)
                for (int y = 0; y < bm.Height; y+=10)
                {
                    Color c = bm.GetPixel(x, y);
                    if (c.Name == "ffffffff" || c.Name == "ff000000")
                        bw++;
                    else
                        other++;

                }
            if (bw > (other * 10)) isBW = true;
            return isBW;
        }
    }
    

}
